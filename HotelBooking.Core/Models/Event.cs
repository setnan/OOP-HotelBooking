using HotelBooking.Core.Services;

namespace HotelBooking.Core.Models;

public class Event
{
    public int EventId { get; set; }
    public string Name { get; set; }
    
    //Navigation properties for reading / mapping
    public string HotelId { get; set; }
    public List<EventClient> EventClients { get; private set; } = new();
    public List<EventRoom> EventRooms { get; private set; } = new();
    
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }


    public List<EventClient>? GetEventClients() => EventService.GetClientsByEventId(EventId);
    public void AddEventClient(EventClient client)
    {
        EventClients.Add(client);
    }
    public void AddAllEventClients(List<EventClient> clients)
    {
        EventClients = clients;
    }
    
    public List<EventRoom>? GetEventRooms() => EventService.GetRoomsByEventId(EventId);

    public void AddEventRoom(EventRoom room)
    {
        EventRooms.Add(room);
    }

    public void AddAllEventRooms(List<EventRoom> rooms)
    {
        EventRooms = rooms;
    }

    
}