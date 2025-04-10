using Dapper;
using HotelBooking.Core.Database;
using HotelBooking.Core.Models;
using HotelBooking.Core.Utilities;

namespace HotelBooking.Core.Services;

public class BookingService
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
        if (await _db.InsertAsync(booking))
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
        {
            return false;
        }
        var oldRoom = await _roomService.GetRoomByIdAsync(oldBooking.RoomId);
        if (oldRoom == null || oldRoom.RoomId == booking.RoomId)
        {
            return await _db.UpdateAsync(booking);
        }

        if (await _db.UpdateAsync(booking))
        {
            await _roomService.UpdateRoomAvailabilityAsync(oldRoom, true);
            var room = await _roomService.GetRoomByIdAsync(oldBooking.RoomId);
            await _roomService.UpdateRoomAvailabilityAsync(room, false);
        }
        return true;
    }

    public async Task<bool> DeleteBookingAsync(Booking booking)
    {
        var room = await _roomService.GetRoomByIdAsync(booking.RoomId);
        if (await _db.DeleteAsync(booking))
        {
            return  room != null && await _roomService.UpdateRoomAvailabilityAsync(room, true);
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

    public async Task<List<Booking>> GetAllBookingsAsync()
    {
        var bookings = await _db.GetAllAsync<Booking>();

        foreach (var booking in bookings)
        {
            booking.Room = await _roomService.GetRoomByIdAsync(booking.RoomId);
            booking.Guest = await _guestService.GetGuestByIdAsync(booking.GuestId);
        }

        return bookings;
    }

    public async Task<List<Booking>> GetBookingsByRoomIdAsync(int roomId)
    {
        return await _db.GetAllWhereAsync<Booking>("RoomId", roomId);
    }

    public async Task<List<Booking>> GetBookingsByGuestIdAsync(int guestId)
    {
        return await _db.GetAllWhereAsync<Booking>("GuestId", guestId);
    }
} 
