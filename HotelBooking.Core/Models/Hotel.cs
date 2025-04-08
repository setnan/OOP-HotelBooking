using DateTime = System.DateTime;

namespace HotelBooking.Core.Models
{
    public class Hotel
    {
        public int HotelId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public List<Room> Rooms { get; set; } = new();

        public List<Room> GetAvailableRooms()
        {
            return Rooms.Where(room => room.IsAvailable).ToList();
        }

        public Booking? BookRoom(Room room, Guest guest, DateTime start, DateTime end)
        {
            if (room.IsAvailable)
            {
                room.Reserve();
                return new Booking
                {
                    Room = room,
                    Guest = guest,
                    CheckIn = start,
                    CheckOut = end
                };
            }

            return null;
        }
    }
}