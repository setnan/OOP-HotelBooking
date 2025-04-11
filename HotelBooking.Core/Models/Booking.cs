using System.Runtime.Serialization;
using HotelBooking.Core.Services;

namespace HotelBooking.Core.Models;

public enum BookingStatus
{
    Confirmed,
    CheckedIn,
    CheckedOut,
    Cancelled
}

public class Booking
{
    public int BookingId { get; set; }
    public int GuestId { get; set; }
    public int RoomId { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.Confirmed;

    [IgnoreDataMember]
    public Guest? Guest { get; set; }

    [IgnoreDataMember]
    public Room? Room { get; set; }
}