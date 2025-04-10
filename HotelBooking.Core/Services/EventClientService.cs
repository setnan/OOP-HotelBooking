using HotelBooking.Core.Backup;
using HotelBooking.Core.Database;
using HotelBooking.Core.Models;
using HotelBooking.Core.Utilities;

namespace HotelBooking.Core.Services;

public class EventClientService : IBackupService<EventClient>
{
    private readonly DatabaseConnection _db;

    public EventClientService(DatabaseConnection db)
    {
        _db = db;
    }
    
    public async Task<IEnumerable<EventClient>> GetAllAsync()
    {
        return await _db.GetAllAsync<EventClient>();
    }

    public async Task<IEnumerable<EventClient>> GetAllForBackupAsync()
    {
        return await _db.GetAllAsync<EventClient>();
    }

    public async Task InsertManyAsync(IEnumerable<EventClient> items)
    {
        foreach (var item in items)
        {
            await _db.InsertAsync<EventClient>(item);
        }
    }

    public async Task<bool> AddAsync(EventClient item)
    {
       return await _db.InsertAsync(item);
    }

    public async Task<bool> UpdateAsync(EventClient item)
    {
        return await _db.UpdateAsync(item);
    }

    public async Task<bool> DeleteAsync(EventClient item)
    {
        return await _db.DeleteAsync(item);
    }

    public async Task<bool> DeleteAllAsync(IEnumerable<EventClient> items)
    {
        foreach (var item in items)
        {
            await _db.DeleteAsync<EventClient>(item);
        }
        return true;
    }

    public async Task<IEnumerable<EventClient>> GetAllByEventIdAsync(int eventId)
    {
        return await _db.GetAllByColumnValueAsync<EventClient>("EventId", eventId);
    }

    public async Task<IEnumerable<EventClient>> GetAllByClientIdAsync(int clientId)
    {
        return await _db.GetAllByColumnValueAsync<EventClient>("ClientId", clientId);
    }
    
}