
namespace HotelBooking.Core.Models;

public class Event
{
    public int EventId { get; set; }
    public string Name { get; set; }
    public int HotelId { get; set; }
    public int OrganiserId { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }

    // Relasjoner, data-only
    public List<EventClient>? EventClients { get; set; }
    public List<EventRoom>? EventRooms { get; set; }
}
