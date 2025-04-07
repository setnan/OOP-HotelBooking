using System.Data;
using HotelBooking;
using HotelBooking.Database;
using HotelBooking.Models;

namespace HotelBooking.Services;

public class BookingService(DatabaseConnection _db)
{
    
    public void CreateBooking(Booking booking)
    {
        string query = $@"
            INSERT INTO Booking (GuestId, RoomId, CheckIn, CheckOut)
            VALUES ({booking.Guest.GuestId}, {booking.Room.RoomId}, 
                    '{booking.CheckIn:yyyy-MM-dd}', '{booking.CheckOut:yyyy-MM-dd}');

            UPDATE Room SET IsAvailable = 0 WHERE RoomId = {booking.Room.RoomId};
        ";

        _db.ExecuteNonQuery(query);
    }
    public List<Booking> GetAllBookings()
    {
        string query = @"
        SELECT 
            b.BookingId,
            b.CheckIn,
            b.CheckOut,
            g.GuestId,
            g.name AS Name,
            g.ContactNumber,
            g.Email,
            r.RoomId,
            r.RoomNumber,
            r.Type,
            r.Price
        FROM Booking b
        JOIN Guest g ON b.GuestId = g.GuestId
        JOIN Room r ON b.RoomId = r.RoomId";

        return _db.ExecuteQuery(query, reader => new Booking
        {
            BookingId = reader.GetInt32("BookingId"),
            CheckIn = reader.GetDateTime("CheckIn"),
            CheckOut = reader.GetDateTime("CheckOut"),
            Guest = new Guest
            {
                GuestId = reader.GetInt32("GuestId"),
                Name = reader.GetString("Name"),
                ContactNumber = reader.GetString("ContactNumber"),
                Email = reader.GetString("Email")
            },
            Room = new Room
            {
                RoomId = reader.GetInt32("RoomId"),
                RoomNumber = reader.GetString("RoomNumber"),
                Type = reader.GetString("Type"),
                Price = reader.GetDecimal("Price")
            }
        });
    }

}