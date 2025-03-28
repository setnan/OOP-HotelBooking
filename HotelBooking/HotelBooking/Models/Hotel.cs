using DateTime = System.DateTime;

using System.Collections.Generic;
using System.Linq;
using HotelBooking;
using HotelBooking.Models;

namespace HotelBooking
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