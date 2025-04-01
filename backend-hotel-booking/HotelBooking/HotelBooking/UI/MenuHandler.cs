using HotelBooking.Models;
using HotelBooking.Services;
using OOP_HotelBooking.Services;

namespace HotelBooking.UI
{

    public static class MenuHandler
    {
        public static void RunMainMenu(GuestService guestService, RoomService roomService,
            BookingService bookingService,
            DatabaseConnection db)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Hotel Booking System ===");
                Console.WriteLine("1. Vis alle gjester");
                Console.WriteLine("2. Vis tilgjengelige rom");
                Console.WriteLine("3. Lag ny booking");
                Console.WriteLine("4. Vis alle bookinger");
                Console.WriteLine("5. Avslutt");
                Console.Write("\nVelg et alternativ: ");

                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        Console.WriteLine("\\n Gjester:");
                        var guests = guestService.GetAllGuests();
                        foreach (var g in guests)
                        {
                            Console.WriteLine(
                                $"ID: {g.GuestId}, Navn: {g.Name}, Tlf: {g.ContactNumber}, Epost: {g.Email}");
                        }

                        break;

                    case "2":
                        Console.WriteLine("\\n Tilgjengelige rom:");
                        var rooms = roomService.GetAvailableRooms();
                        foreach (var r in rooms)
                        {
                            Console.WriteLine(
                                $"ID: {r.RoomId}, Rom: {r.RoomNumber}, Type: {r.Type}, Pris: {r.Price} kr");
                        }

                        break;

                    case "3":
                        Console.WriteLine("\\n Registrer ny booking:");

                        Console.Write("Gjest-ID: ");
                        int guestId = int.Parse(Console.ReadLine());

                        Console.Write("Rom-ID: ");
                        int roomId = int.Parse(Console.ReadLine());

                        Console.Write("Innsjekksdato (yyyy-MM-dd): ");
                        DateTime checkIn = DateTime.Parse(Console.ReadLine());

                        Console.Write("Utsjekksdato (yyyy-MM-dd): ");
                        DateTime checkOut = DateTime.Parse(Console.ReadLine());

                        var newGuest = new Guest { GuestId = guestId };
                        var newRoom = new Room { RoomId = roomId };

                        var booking = new Booking
                        {
                            Guest = newGuest,
                            Room = newRoom,
                            CheckIn = checkIn,
                            CheckOut = checkOut
                        };

                        bookingService.CreateBooking(booking);
                        Console.WriteLine("Booking lagret!");
                        break;

                    case "4":
                        Console.WriteLine("\\n Registrerte bookinger:");
                        var bookings = bookingService.GetAllBookings();

                        foreach (var b in bookings)
                        {
                            Console.WriteLine($"#{b.BookingId}: {b.Guest.Name} → Rom {b.Room.RoomNumber} " +
                                              $"fra {b.CheckIn:yyyy-MM-dd} til {b.CheckOut:yyyy-MM-dd}");
                        }

                        break;

                    case "5":
                        db.Close();
                        Console.WriteLine("Programmet avsluttes...");
                        return;

                    default:
                        Console.WriteLine("Ugyldig valg. Prøv igjen.");
                        break;
                }

                Console.WriteLine("\\nTrykk en tast for å fortsette...");
                Console.ReadKey();
            }
        }
    }
}
