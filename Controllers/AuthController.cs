using Longfunctie.api.Data;
using Longfunctie.api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Longfunctie.api.Controllers
{

    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private string _key { get; set; }

        public AuthController(IConfiguration config, AppDbContext context)
        {
            _context = context;
            _key = config["Jwt:Key"] ?? throw new Exception("JWT Key missing");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            // Check if parent already exists
            var existingParent = await _context.Parents
                .AnyAsync(p => p.Name == request.Name);

            if (existingParent)
            {
                return BadRequest(new
                {
                    message = "A parent with this name already exists."
                });
            }

            var parent = new Parent
            {
                Name = request.Name,
                PinHash = BCrypt.Net.BCrypt.HashPassword(request.Pin),
                CreatedAt = DateTime.Now
            };

            _context.Parents.Add(parent);
            await _context.SaveChangesAsync();

            return Ok(new { parent.Name });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var parent = await _context.Parents.FindAsync(request.Name);

            if (parent == null || !BCrypt.Net.BCrypt.Verify(request.Pin, parent.PinHash))
                return Unauthorized();

            var token = GenerateJwt(parent.Name);

            return Ok(new { token });
        }

        private string GenerateJwt(string parentName)
        {
            if (string.IsNullOrEmpty(parentName))
                throw new ArgumentException("parentName cannot be null or empty");

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, parentName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key.PadRight(32, '0')));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(12),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
