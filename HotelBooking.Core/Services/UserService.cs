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

    public static async Task<bool> UpdateUserAsync(User user, string json)
    {
        if (user.ApplyUpdatesFromJson(json))
        {
            return await DatabaseConnection.Instance.UpdateAsync(user);
        }
        return false;
    }
    
    public static async Task<List<User>> GetAllUsersAsync()
    {
        return await DatabaseConnection.Instance.GetAllAsync<User>();
    }

    
    public static async Task<bool> AddUserAsync(User user)
    {
        return await DatabaseConnection.Instance.InsertAsync(user);
    }
    
    
    public static async Task<User?> GetUserByEmailAsync(string email)
    {
        return await DatabaseConnection.Instance.GetOneAsync<User>("Email",email);
    }

    
    public static async Task<User?> GetUserByIdAsync(int id)
    {
        return await DatabaseConnection.Instance.GetOneAsync<User>("UserId", id);
    }

    
    public static async Task<bool> ValidatePasswordAsync(User user, string? password)
    {
        var userData = await DatabaseConnection.Instance.GetOneAsync<User>("UserId",  user.UserId);
        if (password != null && userData != null && userData.Password.Equals(password))
        {
            return true;
        }
        return false;
    }

    
    public static async Task<bool> ChangePasswordAsync(User user, string oldPassword, string newPassword)
    {
        var json = $"{{ \"Password\": \"{newPassword}\" }}";

        if (await ValidatePasswordAsync(user, oldPassword))
        {
            await UpdateUserAsync(user, json);
            return true;
        }
        return false;
    }
    
    
    public static async Task<bool> DeleteUserAsync(User user)
    {
        return await DatabaseConnection.Instance.DeleteAsync(user);
    }
    
    
    public static async Task<User?> GetUserByNameAsync(string name)
    {
        
        return await DatabaseConnection.Instance.GetOneAsync<User>("Name",name);
    }
}