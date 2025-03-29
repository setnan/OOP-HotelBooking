using HotelBooking;
using OOP_HotelBooking;

using OOP_HotelBooking.Services;

namespace HotelBooking
{
    

class Program
{
    static void Main()
    {
        var db = DatabaseConnection.Instance;
        db.Open();

        var guestService = new GuestService(db);
        var roomService = new RoomService(db);
        var bookingService = new BookingService(db);

        MenuHandler.RunMainMenu(guestService, roomService, bookingService, db);
        return;
    }
}
}