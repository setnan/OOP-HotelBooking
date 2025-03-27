using OOP_HotelBooking.Models;

namespace OOP_HotelBooking.Services;

public class RoomService
{
    private readonly DatabaseConnection _db;

    public RoomService(DatabaseConnection db)
    {
        _db = db;
    }

    public List<Room> GetAvailableRooms()
    {
        string query = "SELECT * FROM Room WHERE is_available = 1";

        return _db.ExecuteQuery(query, reader => new Room
        {
            RoomId = reader.GetInt32("room_id"),
            HotelId = reader.GetInt32("hotel_id"),
            RoomNumber = reader.GetString("room_number"),
            Type = reader.GetString("type"),
            Price = reader.GetDecimal("price"),
            IsAvailable = reader.GetBoolean("is_available")
        });
    }
}