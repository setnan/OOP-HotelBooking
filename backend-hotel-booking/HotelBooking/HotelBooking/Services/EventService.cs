using HotelBooking;
using HotelBooking.Database;
using HotelBooking.Models;

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
    
    
}