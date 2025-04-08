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
            var room = RoomService.GetRoomById(booking.Room.RoomId);
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

    public static bool DeleteBooking(Booking booking)
    {
        return DatabaseConnection.Instance.Delete(booking);
    }


    public static Booking? GetBookingById(int id)
    {
        return DatabaseConnection.Instance.GetOne<Booking>("BookingId", id);
    }


    public static List<Booking> GetAllBookings()
    {
        var query = GetBookingMappingQuery();

        var bookings = GetBookingMapping(query);
        return bookings;
    }

    public static List<Booking>? GetBookingByRoomId(int roomId)
    {
        var query = GetBookingMappingQuery() + $"WHERE b.RoomId = @RoomId";
        return GetBookingMapping(query, new { RoomId = roomId });
    }

    public static List<Booking>? GetBookingByGuestId(int guestId)
    {
        var query = GetBookingMappingQuery() + $"WHERE b.GuestId = @GuestId";
        return GetBookingMapping(query, new {GuestId = guestId});
    }

    private static string GetBookingMappingQuery()
    {
        return @"
            SELECT * 
            FROM Booking b
            JOIN Guest g ON b.GuestId = g.GuestId
            JOIN Room r ON b.RoomId = r.RoomId;";
    }

    private static List<Booking> GetBookingMapping(string query, object? parameters = null)
    {
        var connection = DatabaseConnection.Instance.GetConnection();
        return connection.Query<Booking, Guest, Room, Booking>(
            query,
            (booking, guest, room) =>
            {
                booking.Guest = guest;
                booking.Room = room;
                return booking;
            },
            splitOn: "GuestId,RoomId"
        ).ToList();
    }

}