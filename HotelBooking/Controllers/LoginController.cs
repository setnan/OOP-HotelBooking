using HotelBooking.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    [HttpPost]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest("Brukernavn eller e-post og passord må fylles ut.");
        }

        // Pålogging via email eller brukervavn
        var user = UserService.GetUserFromEmail(request.Username) 
                   ?? UserService.GetUserFromName(request.Username);

        if (user == null || user.Password != request.Password)
        {
            return Unauthorized("Feil brukernavn/e-post eller passord.");
        }

        return Ok(new LoginResponse
        {
            UserId = user.UserId,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role.ToString()
        });
    }
}

public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginResponse
{
    public int UserId { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string Role { get; set; } = "";
}