using HotelBooking.Core.Database;
using HotelBooking.Core.Models;
using HotelBooking.Core.Utilities;

namespace HotelBooking.Core.Services;

public class GuestService(DatabaseConnection connection)
{

    public static async Task<bool> AddGuestAsync(Guest guest)
    {
        return await DatabaseConnection.Instance.InsertAsync(guest);
    }

    public static async Task<bool> UpdateGuestAsync(Guest guest, string json)
    {
        if (guest.ApplyUpdatesFromJson(json))
        {
            return await DatabaseConnection.Instance.UpdateAsync(guest);
        }
        return false;
    }

    public static async Task<bool> DeleteGuestAsync(Guest guest)
    {
        return await DatabaseConnection.Instance.DeleteAsync(guest);
    }

    public static async Task<Guest?> GetGuestByIdAsync(int id)
    {
        return await DatabaseConnection.Instance.GetOneAsync<Guest>("GuestId", id);
    }

    public static async Task<Guest?> GetGuestByEmailAsync(string email)
    {
        return await DatabaseConnection.Instance.GetOneAsync<Guest>("Email", email);
    }

    public static async Task<Guest?> GetGuestByNameAsync(string name)
    {
        return await DatabaseConnection.Instance.GetOneAsync<Guest>("Name", name);
    }
}