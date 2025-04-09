using HotelBooking.Core.Database;
using HotelBooking.Core.Models;
using HotelBooking.Core.Utilities;

namespace HotelBooking.Core.Services;

public class GuestService
{
    private readonly DatabaseConnection _db;

    public GuestService(DatabaseConnection db)
    {
        _db = db;
    }


    public async Task<bool> AddGuestAsync(Guest guest)
    {
        return await _db.InsertAsync(guest);
    }

    public async Task<bool> UpdateGuestAsync(Guest guest, string json)
    {
        if (guest.ApplyUpdatesFromJson(json))
        {
            return await _db.UpdateAsync(guest);
        }
        return false;
    }

    public async Task<bool> DeleteGuestAsync(Guest guest)
    {
        return await _db.DeleteAsync(guest);
    }

    public async Task<Guest?> GetGuestByIdAsync(int id)
    {
        return await _db.GetOneAsync<Guest>("GuestId", id);
    }

    public async Task<Guest?> GetGuestByEmailAsync(string email)
    {
        return await _db.GetOneAsync<Guest>("Email", email);
    }

    public async Task<Guest?> GetGuestByNameAsync(string name)
    {
        return await _db.GetOneAsync<Guest>("Name", name);
    }
}