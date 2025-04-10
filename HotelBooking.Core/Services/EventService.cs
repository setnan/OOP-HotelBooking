using Dapper;
using HotelBooking.Core.Backup;
using HotelBooking.Core.Database;
using HotelBooking.Core.Models;

namespace HotelBooking.Core.Services;

public class EventService : IBackupService<Event>
{
    private readonly DatabaseConnection _db;
    private readonly RoomService _roomService;
    private readonly ClientService _clientService;
    private readonly EventClientService _eventClientService;
    private readonly EventRoomService _eventRoomService;

    public EventService(DatabaseConnection db, 
        RoomService roomService, 
        ClientService clientService, 
        EventClientService eventClientService,  
        EventRoomService eventRoomService)
    {
        _db = db;
        _roomService = roomService;
        _clientService = clientService;
        _eventClientService = eventClientService;
        _eventRoomService = eventRoomService;
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

    public async Task<IEnumerable<EventRoom>?> GetRoomsByEventIdAsync(int eventId)
    {
        return await _db.GetAllByColumnValueAsync<EventRoom>("EventId", eventId);
    }

    public async Task<IEnumerable<EventClient>?> GetClientsByEventIdAsync(int eventId)
    {
        return await _db.GetAllByColumnValueAsync<EventClient>("EventId", eventId);
    }

    public async Task<IEnumerable<Event>?> GetAllAsync()
    {
        return await _db.GetAllAsync<Event>();
    }

    public async Task<IEnumerable<Event>> GetAllForBackupAsync()
    {
        return await _db.GetAllAsync<Event>();
    }

    public async Task InsertManyAsync(IEnumerable<Event> items)
    {
        foreach (var item in items)
        {
            await _db.InsertAsync(item);
        }
    }

    public async Task DeleteAllAsync()
    {
        await _db.DeleteAllAsync<Event>();
    }

    public async Task<IEnumerable<Event>> GetEventsWithDetailsAsync()
    {
        var events = (await _db.GetAllAsync<Event>()).ToList();

        foreach (var eventen in events)
        {
            eventen.EventClients = (await _db.GetAllByColumnValueAsync<EventClient>("EventId", eventen.EventId)).ToList();
            eventen.EventRooms = (await _db.GetAllByColumnValueAsync<EventRoom>("EventId", eventen.EventId)).ToList();
        }

        return events;
    }
    
    public async Task<IEnumerable<Client>> GetAllClientsForEventAsync(int eventId)
    {
        var eventClients = await _eventClientService.GetAllByEventIdAsync(eventId);
        List<Client> clientEvents = new List<Client>();
        foreach (var eventClient in eventClients)
        {
            var currentClient = await _db.GetOneAsync<Client>("ClientId", eventClient.ClientId);
            if (currentClient != null) clientEvents.Add(currentClient);
        }
        return clientEvents;
    }
    
    public async Task<IEnumerable<Room>> GetAllRoomsForEventAsync(int eventId)
    {
        var eventRooms = await _eventRoomService.GetAllByEventIdAsync(eventId);
        List<Room> roomsForEvent = new List<Room>();

        foreach (var eventRoom in eventRooms)
        {
            var currentRoom = await _db.GetOneAsync<Room>("RoomId", eventRoom.RoomId);
            if (currentRoom != null) roomsForEvent.Add(currentRoom);
        }

        return roomsForEvent;
    }



}