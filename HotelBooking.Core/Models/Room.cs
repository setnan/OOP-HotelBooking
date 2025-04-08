namespace HotelBooking.Core.Models;

public class Room
{
    public int RoomId { get; set; }
    public int HotelId { get; set; }
    public string RoomNumber { get; set; }
    public string Type { get; set; }
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; }
    
}