using HotelBooking.Core.Services;

namespace HotelBooking.Core.Models;

public class Event
{
    public int EventId { get; set; }
    
    //Navigation properties for reading / mapping
    public string HotelId { get; set; }
    private List<EventClient> EventClients = new();
    private List<EventRoom> EventRooms = new();
    
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }


    public List<EventClient>? GetEventClients() => EventService.GetClientsByEventId(EventId);
    public List<EventRoom>? GetEventRooms() => EventService.GetRoomsByEventId(EventId);

    
}