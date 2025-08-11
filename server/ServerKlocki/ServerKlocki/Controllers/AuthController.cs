using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ServerKlocki.Database;
using ServerKlocki.Database.Models;
using ServerKlocki.DTOs;
using System.Diagnostics;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ServerKlocki.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IConfiguration _config;

        public AuthController(DatabaseContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // generates token with some user information
        private string GenerateJWTToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.Sha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim("id", user.Id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("register")]
        public IActionResult RegisterUser([FromBody] RegisterRequestDTO registerData)
        {
            // can't be empty
            if (string.IsNullOrWhiteSpace(registerData.Email)       ||
                string.IsNullOrWhiteSpace(registerData.Username)    ||
                string.IsNullOrWhiteSpace(registerData.Password)    ||
                string.IsNullOrWhiteSpace(registerData.Password2))
            {
                return BadRequest();
            }

            // can't be different
            if (registerData.Password != registerData.Password2)
                return BadRequest();

            // can't already exist
            var users = _context.Users.
                Where(u => u.Email == registerData.Email || u.Username == registerData.Username).ToList();

            if (users.Count != 0)
                return BadRequest();

            // TODO - hash password
            User u = new User() { 
                Email = registerData.Email,
                Username = registerData.Username,
                Password = registerData.Password
            };

            var newUser = _context.Users.Add(u);
            _context.SaveChanges();

            return Ok($"UserRegistered: noice");
        }

        [HttpPost("login")]
        public IActionResult LoginUser([FromBody] LoginRequestDTO loginData)
        {
            if (string.IsNullOrEmpty(loginData.Email)       ||
                string.IsNullOrEmpty(loginData.Password))
            {
                return BadRequest();
            }

            // TODO - hash password
            var user = _context.Users.FirstOrDefault(
                u => u.Email == loginData.Email && 
                u.Email == loginData.Password
            );

            if (user == null)
            {
                return NotFound();
            }

            string newToken = GenerateJWTToken(user);
            TokenResponseDTO tokenResponseDTO = new TokenResponseDTO() { message = "Login Successful", token = newToken };
            return Ok(tokenResponseDTO);
        }
    }
}
