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
    
    
}