using System.Collections.Generic;
using System.Threading.Tasks;
using HotelBooking.Core.Models;
using HotelBooking.Core.Services;

namespace HotelBooking.AvaloniaApp.Services;

public class GuestServiceWrapper
{
    private readonly GuestService guestService;

    public GuestServiceWrapper(GuestService guestService)
    {
        this.guestService = guestService;
    }

    public async Task<IEnumerable<Guest>> GetAllGuestsAsync()
    {
        return await Task.Run(() => guestService.GetAllGuests());
    }

    public async Task<bool> AddGuestAsync(Guest guest)
    {
        return await Task.Run(() => guestService.AddGuest(guest));
    }

    public async Task<bool> UpdateGuestAsync(Guest guest)
    {
        return await Task.Run(() => guestService.UpdateGuest(guest));
    }

    public async Task<bool> DeleteGuestAsync(int id)
    {
        return await Task.Run(() => guestService.DeleteGuest(id));
    }

    public async Task<int> GetTotalGuestsAsync()
    {
        return await Task.Run(() => guestService.GetTotalGuests());
    }
}
