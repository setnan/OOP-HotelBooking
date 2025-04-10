using HotelBooking.Core.Services;

namespace HotelBooking.Core.Models;

public class Booking
{
    public int BookingId { get; set; }
    public int GuestId { get; set; }
    public int RoomId { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public Guest? Guest { get; set; }
    public Room? Room { get; set; }
}

