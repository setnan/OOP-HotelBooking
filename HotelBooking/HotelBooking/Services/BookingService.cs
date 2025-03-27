using OOP_HotelBooking.Models;

namespace OOP_HotelBooking.Services;

public class BookingService
{
    private readonly DatabaseConnection _db;

    public BookingService(DatabaseConnection db)
    {
        _db = db;
    }

    public void CreateBooking(Booking booking)
    {
        string query = $@"
            INSERT INTO Booking (guest_id, room_id, check_in, check_out)
            VALUES ({booking.Guest.GuestId}, {booking.Room.RoomId}, 
                    '{booking.CheckIn:yyyy-MM-dd}', '{booking.CheckOut:yyyy-MM-dd}');

            UPDATE Room SET is_available = 0 WHERE room_id = {booking.Room.RoomId};
        ";

        _db.ExecuteNonQuery(query);
    }
    public List<Booking> GetAllBookings()
    {
        string query = @"
        SELECT 
            b.booking_id,
            b.check_in,
            b.check_out,
            g.guest_id,
            g.name AS guest_name,
            g.contact_number,
            g.email,
            r.room_id,
            r.room_number,
            r.type,
            r.price
        FROM Booking b
        JOIN Guest g ON b.guest_id = g.guest_id
        JOIN Room r ON b.room_id = r.room_id";

        return _db.ExecuteQuery(query, reader => new Booking
        {
            BookingId = reader.GetInt32("booking_id"),
            CheckIn = reader.GetDateTime("check_in"),
            CheckOut = reader.GetDateTime("check_out"),
            Guest = new Guest
            {
                GuestId = reader.GetInt32("guest_id"),
                Name = reader.GetString("guest_name"),
                ContactNumber = reader.GetString("contact_number"),
                Email = reader.GetString("email")
            },
            Room = new Room
            {
                RoomId = reader.GetInt32("room_id"),
                RoomNumber = reader.GetString("room_number"),
                Type = reader.GetString("type"),
                Price = reader.GetDecimal("price")
            }
        });
    }

}