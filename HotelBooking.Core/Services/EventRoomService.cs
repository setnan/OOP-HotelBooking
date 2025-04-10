using HotelBooking.Core.Backup;
using HotelBooking.Core.Database;
using HotelBooking.Core.Models;
using HotelBooking.Core.Utilities;

namespace HotelBooking.Core.Services;

public class EventRoomService : IBackupService<EventRoom>
{
    private readonly DatabaseConnection _db;

    public EventRoomService(DatabaseConnection db)
    {
        _db = db;
    }
    
    public async Task<IEnumerable<EventRoom>> GetAllAsync()
    {
        return await _db.GetAllAsync<EventRoom>();
    }

    public async Task<IEnumerable<EventRoom>> GetAllForBackupAsync()
    {
        return await _db.GetAllAsync<EventRoom>();
    }

    public async Task InsertManyAsync(IEnumerable<EventRoom> items)
    {
        foreach (var item in items)
        {
            await _db.InsertAsync<EventRoom>(item);
        }
    }

    public async Task DeleteAllAsync()
    {
        await _db.DeleteAllAsync<EventRoom>();
    }

    public async Task<bool> AddAsync(EventRoom item)
    {
        return await _db.InsertAsync(item);
    }

    public async Task<bool> UpdateAsync(EventRoom item)
    {
        return await _db.UpdateAsync(item);
    }

    public async Task<bool> DeleteAsync(EventRoom item)
    {
        return await _db.DeleteAsync(item);
    }

    public async Task<bool> DeleteAllAsync(IEnumerable<EventRoom> items)
    {
        foreach (var item in items)
        {
            await _db.DeleteAsync<EventRoom>(item);
        }
        return true;
    }

    public async Task<IEnumerable<EventRoom>> GetAllByEventIdAsync(int eventId)
    {
        return await _db.GetAllByColumnValueAsync<EventRoom>("EventId", eventId);
    }

    public async Task<IEnumerable<EventRoom>> GetAllByRoomIdAsync(int roomId)
    {
        return await _db.GetAllByColumnValueAsync<EventRoom>("RoomId", roomId);
    }
}