using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using NeverlandEvolved.Domain.Interfaces;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly IUserRepository _userRepository;

    public AuthController(IConfiguration config, IUserRepository userRepository)
    {
        _config = config;
        _userRepository = userRepository;
    }

    // POST: api/auth/login
    // Tar emot användarnamn och lösenord, validerar mot databasen
    // och returnerar en JWT-token om inloggningen lyckas.
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel login)
    {
        // Hämta alla användare och leta upp en som matchar uppgifterna
        var users = await _userRepository.GetAllAsync();
        var user = users.FirstOrDefault(u => u.Username == login.Username && u.Password == login.Password);

        // Om ingen matchning hittas returneras 401 Unauthorized
        if (user == null) return Unauthorized("Fel användarnamn eller lösenord");

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]!);

        // Bygger upp token-definitionen med claims (användardata inbakad i token)
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role) // Rollen (Admin/User) skickas med i token
            }),
            Expires = DateTime.UtcNow.AddHours(1),   // Token är giltig i 1 timme
            Issuer = _config["Jwt:Issuer"],
            Audience = _config["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature) // Signerar med HMAC SHA-256
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        // Returnerar den färdiga token-strängen till klienten
        return Ok(new { Token = tokenHandler.WriteToken(token) });
    }
}

// Modell för inloggningsformuläret som skickas i request-bodyn
public class LoginModel
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
}
