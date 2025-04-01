using HotelBooking.Models;

namespace HotelBooking.Services;

public static class UserService
{
    public static bool IsAdmin(User user)
    {
        return  user.Role == Role.Admin;
    }

    public static void UpdateUserEmail(int userId, string email)
    {
        var query = "UPDATE User SET Email = @email WHERE UserId = @userId";
        
        DatabaseConnection.Instance.ExecuteSql(query, new {email, userId});
    }

    public static void UpdateUserPassword(int userId, string password)
    {
        var query = "UPDATE User SET Password = @password WHERE UserId = @userId";
        DatabaseConnection.Instance.ExecuteSql(query, new {password, userId});
    }

    public static List<User> GetAllUsers()
    {
        return DatabaseConnection.Instance.GetAll<User>("Users");
    }

    public static void AddUser(User user)
    {
        string insertQuery = @"INSERT INTO User (Name, Email, Password, Role)  VALUES (@Name, @Email, @Password, @Role)";
        DatabaseConnection.Instance.ExecuteSql(insertQuery, user);
    }
    
    public static User? GetUserFromEmail(string email)
    {
        var query = @"SELECT * FROM User WHERE Email = @email";
        return DatabaseConnection.Instance.GetOne<User>(query, new { email });
    }

    public static bool ValidatePassword(int userId, string? password)
    {
        var validateQuery = @"SELECT * FROM User WHERE UserId = @userId";
        var userData = DatabaseConnection.Instance.GetOne<User>(validateQuery, new { userId});
        if (password != null && userData != null && !userData.Password.Equals(password))
        {
            return false;
        }
        return true;
    }

    public static bool ChangePassword(int userId, string oldPassword, string newPassword)
    {
        if (ValidatePassword(userId, oldPassword))
        {
            UpdateUserPassword(userId, newPassword);
            return true;
        }
        return false;
    }
    
}