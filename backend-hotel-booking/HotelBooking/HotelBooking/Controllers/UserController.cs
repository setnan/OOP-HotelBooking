using HotelBooking.Models;
using HotelBooking.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    // GETING: api/users
    [HttpGet]
    public IActionResult GetAllUsers()
    {
        var users = UserService.GetAllUsers();
        return Ok(users);
    }

    // GETING: api/users/1
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var user = UserService.GetUserById(id);
        return user == null ? NotFound() : Ok(user);
    }

    // DELETE: api/users/1
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        User? user = UserService.GetUserById(id);
        if (user == null)
        {
            return NotFound();
        }
        var success = UserService.DeleteUser(user);
        return success ? Ok() : NotFound();
    }
}
