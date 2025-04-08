using HotelBooking.Core.Database;
using HotelBooking.Core.Models;
using HotelBooking.Core.Utilities;

namespace HotelBooking.Core.Services;

public class RoomService(DatabaseConnection db)
{
    public static bool AddRoom(Room room)
    {
        return DatabaseConnection.Instance.Insert(room);
    }
    
    public static bool UpdateRoom(Room room, string json)
    {
        if (room.ApplyUpdatesFromJson(json))
        {
            return DatabaseConnection.Instance.Update(room);
        }
        
        return false;
    }

    public static bool UpateRoom(Room room)
    {
        return DatabaseConnection.Instance.Update(room);
    }

    public static bool DeleteRoom(Room room)
    {
        return DatabaseConnection.Instance.Delete(room);
    }

    public static List<Room> GetAllRooms()
    {
        return DatabaseConnection.Instance.GetAll<Room>();
    }


    public static List<Room> GetAvailableRooms()
    {
        var rooms = GetAllRooms();
        return rooms.Where(x => x.IsAvailable).ToList();
    }
    
    public static Room? GetRoomById(int id)
    {
        return DatabaseConnection.Instance.GetOne<Room>("RoomId", id);
    }

    public static Room? GetRoomByNumber(string number)
    {
        return DatabaseConnection.Instance.GetOne<Room>("RoomNumber", number);
    }

    public static List<Room> GetRoomsByHotelId(int id)
    {
        var rooms = GetAllRooms();
        return rooms.Where(x => x.HotelId == id).ToList();
    }

    public static bool IsRoomAvailable(Room room)
    {
        return room.IsAvailable;
    }
    
    public static bool UpdateRoomAvailability(Room room, bool availability)
    {
        room.IsAvailable = availability;
        return DatabaseConnection.Instance.Update(room);
    }
}