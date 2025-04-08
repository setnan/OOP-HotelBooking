namespace HotelBooking.Core.Models;

public class Booking
{
    public int BookingId { get; set; }
    public Guest Guest { get; set; }
    public Room Room { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }

}