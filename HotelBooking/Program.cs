using Dumpify;
using HotelBooking.Core.Database;
using HotelBooking.Core.Services;
using HotelBooking.Core.Models;
using HotelBooking.Core.Services;
using HotelBooking.Core.Models;

List<User> users = UserService.GetAllUsers();
foreach (var user in users)
{
    Console.WriteLine(user.Dump());
}