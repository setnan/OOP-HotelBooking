namespace HotelBooking.Models;

public enum Role
{
    Admin,
    Receptionist
}
public class User(int UserID, string name, string email, string password)
{
    public int UserID { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; }
    
}