using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.Annotations;
using ServerKlocki.DTOs;
using ServerKlocki.Examples;
using Microsoft.AspNetCore.Authorization;

namespace ServerKlocki.Controllers
{
    [ApiController]
    [Route("/test")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult TestHello()
        {
            var response = new[] {
                new { message = "Hello from test" },
                new { message = "Another hello" }
            };

            return Ok(response);
        }

        [HttpGet("{param}")]
        public IActionResult TestParamSquared(int param)
        {
            return Ok(param * param);
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Returns number multiplied by 2",
            Description = "Return only if valid number, else returns error 500"
        )]
        [SwaggerRequestExample(typeof(NumberDTO), typeof(NumberDTOExample))]
        [ProducesResponseType(typeof(NumberDTO), 200)]
        [ProducesResponseType(typeof(string), 500)]
        [SwaggerResponseExample(200, typeof(NumberDTOExample))]
        [SwaggerResponseExample(500, typeof(Error500Example))]
        public IActionResult TestPost([FromBody] NumberDTO number)
        {
            if (number == null)
                return BadRequest("Incorrect number data");

            return Ok(new NumberDTO() { Number = number.Number * 2 });
        }

        [Authorize]
        [HttpGet("secured")]
        public IActionResult SecuredTestGet()
        {
            return Ok("To jest zabezpieczony endpoint!");
        }
    }
}
