using Dapper;
using HotelBooking.Core.Backup;
using HotelBooking.Core.Database;
using HotelBooking.Core.Models;
using HotelBooking.Core.Utilities;

namespace HotelBooking.Core.Services;

public class RoomService : IBackupService<Room>
{
    private readonly DatabaseConnection _db;
    private readonly EventRoomService _eventRoomService;

    public RoomService(DatabaseConnection db,  
        EventRoomService eventRoomService)
    {
        _db = db;
        _eventRoomService = eventRoomService;
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

    public async Task<IEnumerable<Room>> GetAllAsync()
    {
        return await _db.GetAllAsync<Room>();
    }

    public async Task<IEnumerable<Room>> GetAllForBackupAsync()
    {
        return await _db.GetAllAsync<Room>();
    }

    public async Task InsertManyAsync(IEnumerable<Room> items)
    {
        foreach (var item in items)
        {
            await  _db.InsertAsync(item);
        }
    }


    public async Task<IEnumerable<Room>> GetAvailableRoomsAsync(DateTime? checkIn = null, DateTime? checkOut = null)
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

    public async Task<IEnumerable<Room>> GetRoomsByHotelIdAsync(int id)
    {
        return await _db.GetAllByColumnValueAsync<Room>("HotelId", id);
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
    
    public async Task<IEnumerable<Event>> GetAllEventsForRoomAsync(int roomId)
    {
        var eventRooms = await _eventRoomService.GetAllByRoomIdAsync(roomId);
        List<Event> eventsForRoom = new List<Event>();

        foreach (var eventRoom in eventRooms)
        {
            var currentEvent = await _db.GetOneAsync<Event>("EventId", eventRoom.EventId);
            if (currentEvent != null) eventsForRoom.Add(currentEvent);
        }

        return eventsForRoom;
    }

}