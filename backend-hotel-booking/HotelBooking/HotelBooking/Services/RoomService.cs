using System.Collections;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using HotelBooking.Database;
using HotelBooking.Models;
using Newtonsoft.Json;

namespace HotelBooking.Services;

public class RoomService(DatabaseConnection db)
{
    public static bool AddRoom(Room ro
    {
        return DatabaseConnection.Instance.Insert(room);
    }
    
    public static bool UpdateRoom(Room room, string json)
    {
        var updatedRoomData = JsonConvert.DeserializeObject<Room>(json);
        foreach (var property in typeof(Room).GetProperties())
        {
            var newProperty = property.GetValue(updatedRoomData);
            if (newProperty != null && !newProperty.Equals(property.GetValue(room)))
            {
                property.SetValue(room, newProperty);
            }
        }
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
    
}