using HotelBooking.Core.Backup;
using HotelBooking.Core.Database;
using HotelBooking.Core.Models;
using HotelBooking.Core.Utilities;

namespace HotelBooking.Core.Services;

public class UserService : IBackupService<User>
{
    private readonly DatabaseConnection _db;

    public UserService(DatabaseConnection db)
    {
        _db = db;
    }
    public static bool IsAdmin(User user)
    {
        return  user.Role == Role.Admin;
    }

    public async Task<bool> UpdateUserAsync(User user, string json)
    {
        if (user.ApplyUpdatesFromJson(json))
        {
            return await _db.UpdateAsync(user);
        }
        return false;
    }

    public async Task<bool> UpdateUserAsync(User user)
    {
        return await _db.UpdateAsync(user);
    }
    
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _db.GetAllAsync<User>();
    }

    public async Task<IEnumerable<User>> GetAllForBackupAsync()
    {
        return await _db.GetAllAsync<User>();
    }

    public async Task InsertManyAsync(IEnumerable<User> items)
    {
        foreach (var item in items)
        {
            await _db.InsertAsync(item);
        }
    }

    public async Task DeleteAllAsync()
    {
        await _db.DeleteAllAsync<Guest>();
    }


    public async Task<bool> AddUserAsync(User user)
    {
        return await _db.InsertAsync(user);
    }
    
    
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _db.GetOneAsync<User>("Email",email);
    }

    
    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _db.GetOneAsync<User>("UserId", id);
    }

    
    public async Task<bool> ValidatePasswordAsync(User user, string? password)
    {
        var userData = await _db.GetOneAsync<User>("UserId",  user.UserId);
        if (password != null && userData != null && userData.Password.Equals(password))
        {
            return true;
        }
        return false;
    }

    
    public async Task<bool> ChangePasswordAsync(User user, string oldPassword, string newPassword)
    {
        var json = $"{{ \"Password\": \"{newPassword}\" }}";

        if (await ValidatePasswordAsync(user, oldPassword))
        {
            await UpdateUserAsync(user, json);
            return true;
        }
        return false;
    }
    
    
    public async Task<bool> DeleteUserAsync(User user)
    {
        return await _db.DeleteAsync(user);
    }
    
    
    public async Task<User?> GetUserByNameAsync(string name)
    {
        
        return await _db.GetOneAsync<User>("Name",name);
    }
}