using HotelBooking.Models;
using HotelBooking.Services;
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
            return BadRequest("Brukernavn og passord må fylles ut.");
        }

        var user = UserService.GetUserFromEmail(request.Username);
        if (user == null || user.Password != request.Password)
        {
            return Unauthorized("Feil brukernavn eller passord.");
        }

        return Ok(new LoginResponse
        {
            UserId = user.UserId,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role.ToString()
        });
    }

    [HttpPost("forgot")]
    public IActionResult ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
            return BadRequest("E-post må fylles ut.");

        var user = UserService.GetUserFromEmail(request.Email);
        if (user == null)
            return NotFound("Fant ingen bruker med denne e-posten.");

        // Her kan vi eventuelt generert en token og sendt e-post senere hvis vi får tid
        // – men for nå returnerer vi bare en beskjed
        return Ok($"Passord-tilbakestillingslenke sendt til {request.Email}");
    }
}

// Request/Response modeller

public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class ForgotPasswordRequest
{
    public string Email { get; set; } = string.Empty;
}

public class LoginResponse
{
    public int UserId { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string Role { get; set; } = "";
}