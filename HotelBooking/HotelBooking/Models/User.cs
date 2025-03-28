namespace HotelBooking.Models;

public enum Role
{
    Admin,
    Receptionist,
    Unknown
}
public class User()
{
    public int UserID { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; }


    public User(int id, string name, string email, string password, Role admin) : this()
    {
        UserID = id;
        Name = name;
        Email = email;
        Password = password;
        Role = Role.Unknown;
    }
}