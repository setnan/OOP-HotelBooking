using System.Collections;
using HotelBooking.Database;
using HotelBooking.Models;

namespace HotelBooking.Services;

public class RoomService(DatabaseConnection db)
{
    // public static List<Room> GetAvailableRooms()
    // {
    //     string query = "SELECT * FROM Room WHERE is_available = 1";
    //
    //     return _db.ExecuteQuery(query, reader => new Room
    //     {
    //         RoomId = reader.GetInt32("room_id"),
    //         HotelId = reader.GetInt32("hotel_id"),
    //         RoomNumber = reader.GetString("room_number"),
    //         Type = reader.GetString("type"),
    //         Price = reader.GetDecimal("price"),
    //         IsAvailable = reader.GetBoolean("is_available")
    //     });
    // }

    public static void AddRoom(Room room)
    {
        var insertQuery = @"
        INSERT INTO Room (HotelId, RoomNumber, Type, Price, IsAvailable) VALUES (@HotelId, @RoomNumber, @Type, @Price,  @IsAvailable)";
        DatabaseConnection.Instance.ExecuteSql(insertQuery, room);

    }

    public static List<Room> GetAllRooms()
    {
        return DatabaseConnection.Instance.GetAll<Room>("Room");
    }


    public IEnumerable GetAvailableRooms()
    {
        throw new NotImplementedException();
    }
}