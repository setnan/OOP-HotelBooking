using Dapper;
using HotelBooking.Core.Database;
using HotelBooking.Core.Models;

namespace HotelBooking.Core.Services;

public class EventService(DatabaseConnection connection)
{
    
    public static bool AddEvent(Event thisevent)
    {
        return DatabaseConnection.Instance.Insert(thisevent);
    }

    public static bool CreateEvent(Event thisevent, List<string> clientBillingAddress, List<string> roomNumbers)
    {
        if (!AddEvent(thisevent)) return false;
        var lastEvent = GetEventByNameAndDate(thisevent.Name, thisevent.StartDate);
        
        if (lastEvent == null) return false;

        foreach (var billingaddress in clientBillingAddress)
        {
            Client? client = ClientService.GetClientByBillingAddress(billingaddress);
            if (client == null) return false;
            EventClient newEventClient = new EventClient
            {
                EventId = lastEvent.EventId,
                ClientId = client.ClientId
            };
            DatabaseConnection.Instance.Insert(newEventClient);
        }

        foreach (var roomNumber in roomNumbers)
        {
            Room? room = RoomService.GetRoomByNumber(roomNumber);
            if (room == null) return false;
            EventRoom newEventRoom = new EventRoom
            {
                EventId = lastEvent.EventId,
                RoomId = room.RoomId,
            };
            DatabaseConnection.Instance.Insert(newEventRoom);
        }
        
        return true;
    }

    public static Event? GetEventByNameAndDate(string name, DateTime startDate)
    {
        var connection = DatabaseConnection.Instance.GetConnection();
        var query = @"SELECT * FROM Event WHERE Name = @Name AND StartDate = @StartDate";
        return connection.QuerySingleOrDefault<Event>(query, new { Name = name, StartDate = startDate });
    }

    public static bool UpdateEvent(Event thisevent)
    {
        return DatabaseConnection.Instance.Update(thisevent);
    }

    public static bool DeleteEvent(Event thisevent)
    {
        return DatabaseConnection.Instance.Delete(thisevent);
    }

    public static Event? GetEventById(int id)
    {
        return DatabaseConnection.Instance.GetOne<Event>("EventId", id);
    }

    public static List<EventRoom>? GetRoomsByEventId(int eventId)
    {
        return DatabaseConnection.Instance.GetAllWhere<EventRoom>("EventId", eventId);
    }

    public static List<EventClient>? GetClientsByEventId(int eventId)
    {
        return DatabaseConnection.Instance.GetAllWhere<EventClient>("EventId", eventId);
    }
    
}