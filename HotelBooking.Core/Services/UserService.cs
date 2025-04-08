using HotelBooking.Core.Database;
using HotelBooking.Core.Models;
using HotelBooking.Core.Utilities;

namespace HotelBooking.Core.Services;

public static class UserService
{
    public static bool IsAdmin(User user)
    {
        return  user.Role == Role.Admin;
    }

    public static bool UpdateUser(User user, string json)
    {
        if (user.ApplyUpdatesFromJson(json))
        {
            return DatabaseConnection.Instance.Update(user);
        }
        return false;
    }
    
    public static List<User> GetAllUsers()
    {
        return DatabaseConnection.Instance.GetAll<User>();
    }

    
    public static bool AddUser(User user)
    {
        return DatabaseConnection.Instance.Insert(user);
    }
    
    
    public static User? GetUserByEmail(string email)
    {
        return DatabaseConnection.Instance.GetOne<User>("Email",email);
    }

    
    public static User? GetUserById(int id)
    {
        return DatabaseConnection.Instance.GetOne<User>("UserId", id);
    }

    
    public static bool ValidatePassword(User user, string? password)
    {
        var userData = DatabaseConnection.Instance.GetOne<User>("UserId",  user.UserId);
        if (password != null && userData != null && userData.Password.Equals(password))
        {
            return true;
        }
        return false;
    }

    
    public static bool ChangePassword(User user, string oldPassword, string newPassword)
    {
        var json = $"{{ \"Password\": \"{newPassword}\" }}";

        if (ValidatePassword(user, oldPassword))
        {
            UpdateUser(user, json);
            return true;
        }
        return false;
    }
    
    
    public static bool DeleteUser(User user)
    {
        return DatabaseConnection.Instance.Delete(user);
    }
    
    
    public static User? GetUserFromName(string name)
    {
        
        return DatabaseConnection.Instance.GetOne<User>("Name",name);
    }
}