using HotelBooking.Core.Database;
using HotelBooking.Core.Models;
using HotelBooking.Core.Utilities;

namespace HotelBooking.Core.Services;

public class GuestService(DatabaseConnection connection)
{

    public static bool AddGuest(Guest guest)
    {
        return DatabaseConnection.Instance.Insert(guest);
    }

    public static bool UpdateGuest(Guest guest, string json)
    {
        if (guest.ApplyUpdatesFromJson(json))
        {
            return DatabaseConnection.Instance.Update(guest);
        }
        return false;
    }

    public static bool DeleteGuest(Guest guest)
    {
        return DatabaseConnection.Instance.Delete(guest);
    }

    public static Guest? GetGuestById(int id)
    {
        return DatabaseConnection.Instance.GetOne<Guest>("GuestId", id);
    }

    public static Guest? GetGuestByEmail(string email)
    {
        return DatabaseConnection.Instance.GetOne<Guest>("Email", email);
    }

    public static Guest? GetGuestByName(string name)
    {
        return DatabaseConnection.Instance.GetOne<Guest>("Name", name);
    }
}