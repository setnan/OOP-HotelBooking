using HotelBooking.Models;
using HotelBooking.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllRooms()
    {
        var rooms = RoomService.GetAllRooms();
        return Ok(rooms);
    }

    [HttpGet("available")]
    public IActionResult GetAvailableRooms()
    {
        var available = RoomService.GetAvailableRooms();
        return Ok(available);
    }

    [HttpPost]
    public IActionResult AddRoom([FromBody] Room room)
    {
        var success = RoomService.AddRoom(room);
        if (!success) return BadRequest("Klarte ikke legge til rom.");
        return Ok(room);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteRoom(int id)
    {
        var room = RoomService.GetAllRooms().FirstOrDefault(r => r.RoomId == id);
        if (room == null) return NotFound("Rommet finnes ikke.");

        var success = RoomService.DeleteRoom(room);
        return success ? Ok() : StatusCode(500, "Kunne ikke slette rom.");
    }
}