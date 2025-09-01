using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ServerKlocki.Database;
using ServerKlocki.Database.Models;
using ServerKlocki.DTOs;
using System.Diagnostics;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ServerKlocki.Helpers;

namespace ServerKlocki.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private JWTTokenGenerator _tokenGenerator;

        public AuthController(DatabaseContext context, IConfiguration config)
        {
            _context = context;
            _tokenGenerator = new JWTTokenGenerator(config);
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
                u => 
                u.Email == loginData.Email && 
                u.Password == loginData.Password
            );

            if (user == null)
            {
                return NotFound();
            }

            string newToken = _tokenGenerator.GenerateJWTToken(user);
            //Debug.WriteLine(newToken);
            TokenResponseDTO tokenResponseDTO = new TokenResponseDTO() { message = "Login Successful", token = newToken };
            return Ok(tokenResponseDTO);
        }
    }
}



/*
 * [HttpPost]
        [SwaggerOperation(
            Summary = "Returns number multiplied by 2",
            Description = "Return only if valid number, else returns error 500"
        )]
        [SwaggerRequestExample(typeof(NumberDTO), typeof(NumberDTOExample))]
        [ProducesResponseType(typeof(NumberDTO), 200)]
        [ProducesResponseType(typeof(string), 500)]
        [SwaggerResponseExample(200, typeof(NumberDTOExample))]
        [SwaggerResponseExample(500, typeof(Error500Example))]
 * */