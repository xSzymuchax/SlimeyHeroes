using Microsoft.AspNetCore.Mvc;
using ServerKlocki.DTOs;

namespace ServerKlocki.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpPost("register")]
        public IActionResult RegisterUser([FromBody] RegisterDataDTO registerData)
        {
            return Ok();
        }

        [HttpPost("login")]
        public IActionResult LoginUser([FromBody] LoginDataDTO loginData)
        {
            return Ok();
        }
    }
}
