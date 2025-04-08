namespace HotelBooking.Core.Models;

public class Event
{
    public int EventId { get; set; }
    public Client Organiser { get; set; }
    public List<Room> Rooms { get; set; } = new();
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }

    public string GetEventDetails()
    {
        return $"{Organiser?.Name} arrangerer et event fra {StartDate:dd.MM} {StartTime} til {EndDate:dd.MM} {EndTime}.";
    }
}