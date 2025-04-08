namespace HotelBooking.Core.Models;

public class Client
{
    public int ClientId { get; set; }
    public string Name { get; set; }
    public string BillingAddress { get; set; } // Eller e-post
    public string ContactPerson { get; set; }
    public string ContactNumber { get; set; }
}