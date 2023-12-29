using API.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly string Key;

        public AuthenticationController(IConfiguration config)
        {
            Key = config.GetSection("settings").GetSection("Key").ToString();
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] User request)
        {
            if (request.UserName == "admin" && request.Password == "123")
            {
                var keyBytes = Encoding.ASCII.GetBytes(Key);
                var claims = new ClaimsIdentity();

                claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, request.UserName));

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = claims,
                    Expires = DateTime.UtcNow.AddHours(2),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature),
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

                string createdToken = tokenHandler.WriteToken(tokenConfig);

                return StatusCode(StatusCodes.Status200OK, new { token = createdToken });
                //return Ok(createdToken);
            } else
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { token = "" });
                //return Unauthorized();
            }
        }
    }
}
