using Dumpify;
using HotelBooking.Core.Services;
using HotelBooking.Core.Models;


List<User> users = UserService.GetAllUsers();
foreach (var user in users)
{
    Console.WriteLine(user.Dump());
}

List<Room> availableRooms = RoomService.GetAvailableRooms(DateTime.Parse("2025-04-02"), DateTime.Parse("2025-04-04"));
foreach (var room in availableRooms)
{
    Console.WriteLine(room.Dump());
}

var newBooking = new Booking
{
    GuestId = 1,
    RoomId = 1,
    CheckIn = DateTime.Parse("2025-04-02"),
    CheckOut = DateTime.Parse("2025-04-04")
};
BookingService.AddBooking(newBooking);


BookingService.AddBooking(newBooking);

List<Room> availableRooms1 = RoomService.GetAvailableRooms(DateTime.Parse("2025-04-02"), DateTime.Parse("2025-04-04"));
foreach (var room in availableRooms1)
{
    Console.WriteLine(room.Dump());
}