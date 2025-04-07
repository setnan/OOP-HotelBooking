using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Controllers;

[ApiController]
[Route("[controller]")]
public class BookingController : ControllerBase
{
    private static List<Booking> bookings = new List<Booking>();

    [HttpGet]
    public IActionResult GetAll() => Ok(bookings);

    [HttpPost]
    public IActionResult Create([FromBody] Booking booking)
    {
        bookings.Add(booking);
        return Ok(booking);
    }
}