using HotelBooking.Models;

namespace OOP_HotelBooking.Services;

public class UserSession
{
    private static UserSession  _instance;
    
    public int? UserId { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public bool IsLoggedIn { get; set; }
    public bool IsAdmin { get; set; }
    
    private UserSession() {}
    
    public static UserSession Instance => _instance ??= new UserSession();


    public void Login(User user)
    {
        UserId = user.UserId;
        Name = user.Name;
        Email = user.Email;
        IsLoggedIn = true;
        IsAdmin = user.Role == Role.Admin;
    }

    public void Logout()
    {
        IsLoggedIn = false;
        IsAdmin = false;
        UserId = null;
        Name = null;
        Email = null;
    }
    
}