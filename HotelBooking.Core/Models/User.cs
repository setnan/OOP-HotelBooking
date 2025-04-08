namespace HotelBooking.Core.Models;

public enum Role
{
    Admin,
    Receptionist
}
public class User
{
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; }
    
}