using System;

namespace HotelBooking.Models;

public class Catering
{
    public int CateringId { get; set; }
    public Client Organiser { get; set; }
    public Room Room { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int ExpectedAttendees { get; set; }
    public string DietaryNotes { get; set; } 
}