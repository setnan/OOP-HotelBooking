namespace OOP_HotelBooking.Models;

public class Booking
{
    public int BookingId { get; set; }
    public Guest Guest { get; set; }
    public Room Room { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }

    public string GetBookingDetails()
    {
        return $"{Guest?.Name} har booket rom {Room?.RoomNumber} fra {CheckIn:dd.MM} til {CheckOut:dd.MM}";
    }
}