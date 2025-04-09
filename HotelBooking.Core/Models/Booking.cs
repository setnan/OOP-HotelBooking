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
    
    public async Task<Guest?> GetGuestAsync() => await GuestService.GetGuestByIdAsync(GuestId);
    public async Task<Room?> GetRoomAsync() => await RoomService.GetRoomByIdAsync(RoomId);

}
