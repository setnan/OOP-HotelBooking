using Dapper;
using HotelBooking.Core.Database;
using HotelBooking.Core.Models;
using HotelBooking.Core.Utilities;

namespace HotelBooking.Core.Services;

public class RoomService(DatabaseConnection db)
{
    public static async Task<bool> AddRoomAsync(Room room)
    {
        return await DatabaseConnection.Instance.InsertAsync(room);
    }
    
    public static async Task<bool> UpdateRoomAsync(Room room, string json)
    {
        if (room.ApplyUpdatesFromJson(json))
        {
            return await DatabaseConnection.Instance.UpdateAsync(room);
        }
        
        return false;
    }

    public static async Task<bool> UpdateRoomAsync(Room room)
    {
        return await DatabaseConnection.Instance.UpdateAsync(room);
    }

    public static async Task<bool> DeleteRoomAsync(Room room)
    {
        return await DatabaseConnection.Instance.DeleteAsync(room);
    }

    public static async Task<List<Room>> GetAllRoomsAsync()
    {
        return await DatabaseConnection.Instance.GetAllAsync<Room>();
    }


    public static async Task<List<Room>> GetAvailableRoomsAsync(DateTime? checkIn = null, DateTime? checkOut = null)
    {
        var checkInReal = checkIn ?? DateTime.Now;
        var checkOutReal = checkOut ?? DateTime.Now.AddDays(1);

        return await DatabaseConnection.Instance.GetAvailableRoomsAsync(checkInReal, checkOutReal);
    }
    
    public static async Task<Room?> GetRoomByIdAsync(int id)
    {
        return await DatabaseConnection.Instance.GetOneAsync<Room>("RoomId", id);
    }

    public static async Task<Room?> GetRoomByNumberAsync(string number)
    {
        return await DatabaseConnection.Instance.GetOneAsync<Room>("RoomNumber", number);
    }

    public static async Task<List<Room>> GetRoomsByHotelIdAsync(int id)
    {
        return await DatabaseConnection.Instance.GetAllWhereAsync<Room>("HotelId", id);
    }

    public static bool IsRoomAvailable(Room room)
    {
        return room.IsAvailable;
    }
    
    public static async Task<bool> UpdateRoomAvailabilityAsync(Room room, bool availability)
    {
        room.IsAvailable = availability;
        return await DatabaseConnection.Instance.UpdateAsync(room);
    }
    
    
}