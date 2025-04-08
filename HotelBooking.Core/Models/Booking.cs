using HotelBooking.Core.Services;

namespace HotelBooking.Core.Models;

public class Booking
{
    public int BookingId { get; set; }

    // Foreign keys for insert/update
    public int GuestId { get; set; }
    public int RoomId { get; set; }

    // Navigation properties for reading / mapping
    private Guest _guest;
    private Room _room;
    
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    
    public Guest? GetGuest() => GuestService.GetGuestById(GuestId);
    public Room? GetRoom() => RoomService.GetRoomById(RoomId);

}
