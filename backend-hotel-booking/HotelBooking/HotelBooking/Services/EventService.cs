using HotelBooking;
using HotelBooking.Models;

public class EventService
{
    private readonly DatabaseConnection _db;

    public EventService(DatabaseConnection db)
    {
        _db = db;
    }

    public void AddEvent(Event e)
    {
        string insertEvent = @"
            INSERT INTO Event (ClientId, StartDate, EndDate, StartTime, EndTime)
            VALUES (@ClientId, @StartDate, @EndDate, @StartTime, @EndTime)";
        _db.ExecuteSql(insertEvent, e);
    }

    public void LinkEventToRoom(int eventId, int roomId)
    {
        string query = "INSERT INTO EventRoom (EventId, RoomId) VALUES (@eventId, @roomId)";
        _db.ExecuteSql(query, new { eventId, roomId });
    }
}