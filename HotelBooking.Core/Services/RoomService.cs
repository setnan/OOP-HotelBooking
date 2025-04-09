using Dapper;
using HotelBooking.Core.Database;
using HotelBooking.Core.Models;
using HotelBooking.Core.Utilities;

namespace HotelBooking.Core.Services;

public class RoomService
{
    private readonly DatabaseConnection _db;

    public RoomService(DatabaseConnection db)
    {
        _db = db;
    }

    public async Task<bool> AddRoomAsync(Room room)
    {
        return await _db.InsertAsync(room);
    }
    
    public async Task<bool> UpdateRoomAsync(Room room, string json)
    {
        if (room.ApplyUpdatesFromJson(json))
        {
            return await _db.UpdateAsync(room);
        }
        
        return false;
    }

    public async Task<bool> UpdateRoomAsync(Room room)
    {
        return await _db.UpdateAsync(room);
    }

    public async Task<bool> DeleteRoomAsync(Room room)
    {
        return await _db.DeleteAsync(room);
    }

    public async Task<List<Room>> GetAllRoomsAsync()
    {
        return await _db.GetAllAsync<Room>();
    }


    public async Task<List<Room>> GetAvailableRoomsAsync(DateTime? checkIn = null, DateTime? checkOut = null)
    {
        var checkInReal = checkIn ?? DateTime.Now;
        var checkOutReal = checkOut ?? DateTime.Now.AddDays(1);

        return await _db.GetAvailableRoomsAsync(checkInReal, checkOutReal);
    }
    
    public async Task<Room?> GetRoomByIdAsync(int id)
    {
        return await _db.GetOneAsync<Room>("RoomId", id);
    }

    public async Task<Room?> GetRoomByNumberAsync(string number)
    {
        return await _db.GetOneAsync<Room>("RoomNumber", number);
    }

    public async Task<List<Room>> GetRoomsByHotelIdAsync(int id)
    {
        return await _db.GetAllWhereAsync<Room>("HotelId", id);
    }

    public static bool IsRoomAvailable(Room room)
    {
        return room.IsAvailable;
    }
    
    public async Task<bool> UpdateRoomAvailabilityAsync(Room room, bool availability)
    {
        room.IsAvailable = availability;
        return await _db.UpdateAsync(room);
    }
    
    
}