using Dapper;
using HotelBooking.Core.Database;
using HotelBooking.Core.Models;
using HotelBooking.Core.Utilities;

namespace HotelBooking.Core.Services;

public class BookingService
{
    public static async Task<bool> AddBookingAsync(Booking booking)
    {
        if (await DatabaseConnection.Instance.InsertAsync(booking))
        {
            var room = await RoomService.GetRoomByIdAsync(booking.RoomId);
            return room != null && await RoomService.UpdateRoomAvailabilityAsync(room, false);
        }
        return false;
    }


    public static async Task<bool> UpdateBookingAsync(Booking booking, string json)
    {
        if (booking.ApplyUpdatesFromJson(json))
        {
            return await DatabaseConnection.Instance.UpdateAsync(booking);
        }

        return false;
    }

    public static async Task<bool> UpdateBookingAsync(Booking booking)
    {
        var oldBooking = await GetBookingByIdAsync(booking.BookingId);
        if (oldBooking == null)
        {
            return false;
        }
        var oldRoom = await RoomService.GetRoomByIdAsync(oldBooking.RoomId);
        if (oldRoom == null || oldRoom.RoomId == booking.RoomId)
        {
            return await DatabaseConnection.Instance.UpdateAsync(booking);
        }

        if (await DatabaseConnection.Instance.UpdateAsync(booking))
        {
            await RoomService.UpdateRoomAvailabilityAsync(oldRoom, true);
            var room = await RoomService.GetRoomByIdAsync(oldBooking.RoomId);
            await RoomService.UpdateRoomAvailabilityAsync(room, false);
        }
        return true;
    }

    public static async Task<bool> DeleteBookingAsync(Booking booking)
    {
        var room = await RoomService.GetRoomByIdAsync(booking.RoomId);
        if (await DatabaseConnection.Instance.DeleteAsync(booking))
        {
            return  room != null && await RoomService.UpdateRoomAvailabilityAsync(room, true);
        }
        return false;
    }


    public static async Task<Booking?> GetBookingByIdAsync(int id)
    {
        return await DatabaseConnection.Instance.GetOneAsync<Booking>("BookingId", id);
    }


    public static async Task<List<Booking>> GetAllBookingsAsync()
    {
        return await DatabaseConnection.Instance.GetAllAsync<Booking>();
    }

    public static async Task<List<Booking>> GetBookingsByRoomIdAsync(int roomId)
    {
        return await DatabaseConnection.Instance.GetAllWhereAsync<Booking>("RoomId", roomId);
    }

    public static async Task<List<Booking>> GetBookingsByGuestIdAsync(int guestId)
    {
        return await DatabaseConnection.Instance.GetAllWhereAsync<Booking>("GuestId", guestId);
    }
    

}