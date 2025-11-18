using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KeepFit.Backend.API.Controller;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpGet("/me")]
    public IActionResult Me()
    {
        var authHeader = Request.Headers["Authorization"].ToString();
        var token = authHeader.Substring("Bearer ".Length).Trim();
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        return Ok(new
        {
            Id = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value,
            Email = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value,
        });
    }
}