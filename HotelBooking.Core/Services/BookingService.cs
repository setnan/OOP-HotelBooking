using Dapper;
using HotelBooking.Core.Database;
using HotelBooking.Core.Models;
using HotelBooking.Core.Utilities;

namespace HotelBooking.Core.Services;

public class BookingService
{
    public static bool AddBooking(Booking booking)
    {
        if (DatabaseConnection.Instance.Insert(booking))
        {
            var room = RoomService.GetRoomById(booking.RoomId);
            return room != null && RoomService.UpdateRoomAvailability(room, false);
        }
        return false;
    }


    public static bool UpdateBooking(Booking booking, string json)
    {
        if (booking.ApplyUpdatesFromJson(json))
        {
            return DatabaseConnection.Instance.Update(booking);
        }

        return false;
    }

    public static bool UpdateBooking(Booking booking)
    {
        var oldBooking = GetBookingById(booking.BookingId);
        if (oldBooking == null)
        {
            return false;
        }
        var oldRoom = RoomService.GetRoomById(oldBooking.RoomId);
        if (oldRoom == null || oldRoom.RoomId == booking.RoomId)
        {
            return DatabaseConnection.Instance.Update(booking);
        }

        if (DatabaseConnection.Instance.Update(booking))
        {
            RoomService.UpdateRoomAvailability(oldRoom, true);
            RoomService.UpdateRoomAvailability(booking.GetRoom(), false);
        }
        return true;
    }

    public static bool DeleteBooking(Booking booking)
    {
        var room = RoomService.GetRoomById(booking.RoomId);
        if (DatabaseConnection.Instance.Delete(booking))
        {
            return  room != null && RoomService.UpdateRoomAvailability(room, true);
        }
        return false;
    }


    public static Booking? GetBookingById(int id)
    {
        return DatabaseConnection.Instance.GetOne<Booking>("BookingId", id);
    }


    public static List<Booking> GetAllBookings()
    {
        return DatabaseConnection.Instance.GetAll<Booking>();
    }

    public static List<Booking>? GetBookingsByRoomId(int roomId)
    {
        return DatabaseConnection.Instance.GetAllWhere<Booking>("RoomId", roomId);
    }

    public static List<Booking>? GetBookingsByGuestId(int guestId)
    {
        return DatabaseConnection.Instance.GetAllWhere<Booking>("GuestId", guestId);
    }
    

}