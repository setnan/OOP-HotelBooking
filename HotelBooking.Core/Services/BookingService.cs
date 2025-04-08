using Dapper;
using HotelBooking.Core.Database;
using HotelBooking.Core.Models;
using HotelBooking.Core.Utilities;

namespace HotelBooking.Core.Services;

public class BookingService
{
    public static bool AddBooking(Booking booking)
    {
        return DatabaseConnection.Instance.Insert(booking);
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
        var connection = DatabaseConnection.Instance.GetConnection();

        var query = @"
                    SELECT * 
                    FROM ""Booking"" b
                    JOIN ""Guest"" g ON b.""GuestId"" = g.""GuestId""
                    JOIN ""Room"" r ON b.""RoomId"" = r.""RoomId"";";

        var bookings = Enumerable.ToList(connection.Query<Booking, Guest, Room, Booking>(
            query,
            (booking, guest, room) =>
            {
                booking.Guest = guest;
                booking.Room = room;
                return booking;
            },
            splitOn: "GuestId,RoomId"
        ));
        return bookings;
    }
    
    
}