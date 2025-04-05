using System.Data;
using HotelBooking;
using HotelBooking.Database;
using HotelBooking.Models;

namespace OOP_HotelBooking.Services;

public class GuestService
{
    private readonly DatabaseConnection _db;

    public GuestService(DatabaseConnection db)
    {
        _db = db;
    }

    public List<Guest> GetAllGuests()
    {
        string query = "SELECT * FROM Guest";

        return _db.ExecuteQuery(query, reader => new Guest
        {
            GuestId = reader.GetInt32("guest_id"),
            Name = reader.GetString("name"),
            ContactNumber = reader.GetString("contact_number"),
            Email = reader.GetString("email")
        });
    }
}