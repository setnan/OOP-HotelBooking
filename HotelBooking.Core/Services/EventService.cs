using HotelBooking.Core.Database;
using HotelBooking.Core.Models;

namespace HotelBooking.Core.Services;

public class EventService(DatabaseConnection connection)
{
    
    public static bool AddEvent(Event thisevent)
    {
        return DatabaseConnection.Instance.Insert(thisevent);
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