using Dapper;
using HotelBooking.Core.Database;
using HotelBooking.Core.Models;

namespace HotelBooking.Core.Services;

public class EventService
{
    private readonly DatabaseConnection _db;
    private readonly RoomService _roomService;
    private readonly ClientService _clientService;

    public EventService(DatabaseConnection db, RoomService roomService, ClientService clientService)
    {
        _db = db;
        _roomService = roomService;
        _clientService = clientService;
    }
    
    public async Task<bool> AddEventAsync(Event thisevent)
    {
        return await _db.InsertAsync(thisevent);
    }

    public async Task<bool> CreateEventAsync(Event thisevent, List<string> clientBillingAddress, List<string> roomNumbers)
    {
        if (!await AddEventAsync(thisevent)) return false;
        var lastEvent = await GetEventByNameAndDateAsync(thisevent.Name, thisevent.StartDate);
        
        if (lastEvent == null) return false;

        foreach (var billingaddress in clientBillingAddress)
        {
            Client? client = await _clientService.GetClientByBillingAddressAsync(billingaddress);
            if (client == null) return false;
            EventClient newEventClient = new EventClient
            {
                EventId = lastEvent.EventId,
                ClientId = client.ClientId
            };
            await _db.InsertAsync(newEventClient);
        }

        foreach (var roomNumber in roomNumbers)
        {
            Room? room = await _roomService.GetRoomByNumberAsync(roomNumber);
            if (room == null) return false;
            EventRoom newEventRoom = new EventRoom
            {
                EventId = lastEvent.EventId,
                RoomId = room.RoomId,
            };
            await _db.InsertAsync(newEventRoom);
        }
        
        return true;
    }

    public async Task<Event?> GetEventByNameAndDateAsync(string name, DateTime startDate)
    {
        var connection = _db.GetConnection();
        var query = @"SELECT * FROM Event WHERE Name = @Name AND StartDate = @StartDate";
        return await connection.QuerySingleOrDefaultAsync<Event>(query, new { Name = name, StartDate = startDate });
    }

    public async Task<bool> UpdateEventAsync(Event thisevent)
    {
        return await _db.UpdateAsync(thisevent);
    }

    public async Task<bool> DeleteEventAsync(Event thisevent)
    {
        return await _db.DeleteAsync(thisevent);
    }

    public async Task<Event?> GetEventByIdAsync(int id)
    {
        return await _db.GetOneAsync<Event>("EventId", id);
    }

    public async Task<List<EventRoom>?> GetRoomsByEventIdAsync(int eventId)
    {
        return await _db.GetAllWhereAsync<EventRoom>("EventId", eventId);
    }

    public async Task<List<EventClient>?> GetClientsByEventIdAsync(int eventId)
    {
        return await _db.GetAllWhereAsync<EventClient>("EventId", eventId);
    }

    public async Task<List<Event>?> GetAllEventsAsync()
    {
        return await _db.GetAllAsync<Event>();
    }

    public async Task<List<Event>> GetEventsWithDetailsAsync()
    {
        var events = await _db.GetAllAsync<Event>();

        foreach (var eventen in events)
        {
            eventen.EventClients = await _db.GetAllWhereAsync<EventClient>("EventId", eventen.EventId);
            eventen.EventRooms = await _db.GetAllWhereAsync<EventRoom>("EventId", eventen.EventId);
        }
        return events;
    }
}