using Dapper;
using HotelBooking.Core.Backup;
using HotelBooking.Core.Database;
using HotelBooking.Core.Models;
using HotelBooking.Core.Utilities;

namespace HotelBooking.Core.Services;

public class BookingService : IBackupService<Booking>
{
    private readonly DatabaseConnection _db;
    private readonly RoomService _roomService;
    private readonly GuestService _guestService;

    public BookingService(DatabaseConnection db, RoomService roomService, GuestService guestService)
    {
        _db = db;
        _roomService = roomService;
        _guestService = guestService;
    }

    public async Task<bool> AddBookingAsync(Booking booking)
    {
        var cleanBooking = new Booking
        {
            GuestId = booking.GuestId,
            RoomId = booking.RoomId,
            CheckIn = booking.CheckIn,
            CheckOut = booking.CheckOut,
            Status = booking.Status
        };

        if (await _db.InsertAsync(cleanBooking))
        {
            var room = await _roomService.GetRoomByIdAsync(booking.RoomId);
            return room != null && await _roomService.UpdateRoomAvailabilityAsync(room, false);
        }

        return false;
    }

    public async Task<bool> UpdateBookingAsync(Booking booking, string json)
    {
        if (booking.ApplyUpdatesFromJson(json))
        {
            return await _db.UpdateAsync(booking);
        }

        return false;
    }

    public async Task<bool> UpdateBookingAsync(Booking booking)
    {
        var oldBooking = await GetBookingByIdAsync(booking.BookingId);
        if (oldBooking == null)
            return false;

        var oldRoom = await _roomService.GetRoomByIdAsync(oldBooking.RoomId);
        if (oldRoom == null || oldRoom.RoomId == booking.RoomId)
        {
            return await _db.UpdateAsync(booking);
        }

        if (await _db.UpdateAsync(booking))
        {
            await _roomService.UpdateRoomAvailabilityAsync(oldRoom, true);
            var newRoom = await _roomService.GetRoomByIdAsync(booking.RoomId);
            if (newRoom != null)
                await _roomService.UpdateRoomAvailabilityAsync(newRoom, false);
        }

        return true;
    }

    public async Task<bool> DeleteBookingAsync(Booking booking)
    {
        var room = await _roomService.GetRoomByIdAsync(booking.RoomId);
        if (await _db.DeleteAsync(booking))
        {
            return room != null && await _roomService.UpdateRoomAvailabilityAsync(room, true);
        }

        return false;
    }

    public async Task<Booking?> GetBookingByIdAsync(int id)
    {
        var booking = await _db.GetOneAsync<Booking>("BookingId", id);
        if (booking != null)
        {
            booking.Room = await _roomService.GetRoomByIdAsync(booking.RoomId);
            booking.Guest = await _guestService.GetGuestByIdAsync(booking.GuestId);
        }

        return booking;
    }

    public async Task<IEnumerable<Booking>> GetAllAsync()
    {
        var bookings = (await _db.GetAllAsync<Booking>()).ToList();

        foreach (var booking in bookings)
        {
            booking.Room = await _roomService.GetRoomByIdAsync(booking.RoomId) ?? new Room();
            booking.Guest = await _guestService.GetGuestByIdAsync(booking.GuestId) ?? new Guest();
        }

        return bookings;
    }

    public async Task<IEnumerable<Booking>> GetAllForBackupAsync()
    {
        return await _db.GetAllAsync<Booking>();
    }

    public async Task InsertManyAsync(IEnumerable<Booking> items)
    {
        foreach (var item in items)
        {
            await _db.InsertAsync(new Booking
            {
                GuestId = item.GuestId,
                RoomId = item.RoomId,
                CheckIn = item.CheckIn,
                CheckOut = item.CheckOut,
                Status = item.Status
            });
        }
    }

    public async Task DeleteAllAsync()
    {
        await _db.DeleteAllAsync<Booking>();
    }

    public async Task<IEnumerable<Booking>> GetBookingsByRoomIdAsync(int roomId)
    {
        return await _db.GetAllByColumnValueAsync<Booking>("RoomId", roomId);
    }

    public async Task<IEnumerable<Booking>> GetBookingsByGuestIdAsync(int guestId)
    {
        return await _db.GetAllByColumnValueAsync<Booking>("GuestId", guestId);
    }
}
