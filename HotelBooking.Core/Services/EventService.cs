using Dapper;
using HotelBooking.Core.Database;
using HotelBooking.Core.Models;

namespace HotelBooking.Core.Services;

public class EventService
{
    
    public static async Task<bool> AddEventAsync(Event thisevent)
    {
        return await DatabaseConnection.Instance.InsertAsync(thisevent);
    }

    public static async Task<bool> CreateEventAsync(Event thisevent, List<string> clientBillingAddress, List<string> roomNumbers)
    {
        if (!await AddEventAsync(thisevent)) return false;
        var lastEvent = await GetEventByNameAndDateAsync(thisevent.Name, thisevent.StartDate);
        
        if (lastEvent == null) return false;

        foreach (var billingaddress in clientBillingAddress)
        {
            Client? client = await ClientService.GetClientByBillingAddressAsync(billingaddress);
            if (client == null) return false;
            EventClient newEventClient = new EventClient
            {
                EventId = lastEvent.EventId,
                ClientId = client.ClientId
            };
            await DatabaseConnection.Instance.InsertAsync(newEventClient);
        }

        foreach (var roomNumber in roomNumbers)
        {
            Room? room = await RoomService.GetRoomByNumberAsync(roomNumber);
            if (room == null) return false;
            EventRoom newEventRoom = new EventRoom
            {
                EventId = lastEvent.EventId,
                RoomId = room.RoomId,
            };
            await DatabaseConnection.Instance.InsertAsync(newEventRoom);
        }
        
        return true;
    }

    public static async Task<Event?> GetEventByNameAndDateAsync(string name, DateTime startDate)
    {
        var connection = DatabaseConnection.Instance.GetConnection();
        var query = @"SELECT * FROM Event WHERE Name = @Name AND StartDate = @StartDate";
        return await connection.QuerySingleOrDefaultAsync<Event>(query, new { Name = name, StartDate = startDate });
    }

    public static async Task<bool> UpdateEventAsync(Event thisevent)
    {
        return await DatabaseConnection.Instance.UpdateAsync(thisevent);
    }

    public static async Task<bool> DeleteEventAsync(Event thisevent)
    {
        return await DatabaseConnection.Instance.DeleteAsync(thisevent);
    }

    public static async Task<Event?> GetEventByIdAsync(int id)
    {
        return await DatabaseConnection.Instance.GetOneAsync<Event>("EventId", id);
    }

    public static async Task<List<EventRoom>?> GetRoomsByEventIdAsync(int eventId)
    {
        return await DatabaseConnection.Instance.GetAllWhereAsync<EventRoom>("EventId", eventId);
    }

    public static async Task<List<EventClient>?> GetClientsByEventIdAsync(int eventId)
    {
        return await DatabaseConnection.Instance.GetAllWhereAsync<EventClient>("EventId", eventId);
    }

    public static async Task<List<Event>?> GetAllEventsAsync()
    {
        return await DatabaseConnection.Instance.GetAllAsync<Event>();
    }

    public static async Task<List<Event>> GetEventsWithDetailsAsync()
    {
        var events = await DatabaseConnection.Instance.GetAllAsync<Event>();

        foreach (var eventen in events)
        {
            eventen.AddAllEventClients(await DatabaseConnection.Instance.GetAllWhereAsync<EventClient>("EventId", eventen.EventId));
            eventen.AddAllEventRooms(await DatabaseConnection.Instance.GetAllWhereAsync<EventRoom>("EventId", eventen.EventId));
        }
        return events;
    }
}