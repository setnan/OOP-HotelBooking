using System.Data;
using HotelBooking;
using HotelBooking.Database;
using HotelBooking.Models;
using HotelBooking.Utilities;

namespace OOP_HotelBooking.Services;

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

    public static bool DeteGuest(Guest guest)
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