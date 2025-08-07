using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
namespace ServerKlocki.Controllers
{
    [ApiController]
    [Route("/test")]
    public class TestController : ControllerBase
    {
        public class NumberDTO
        {
            public int Number { get; set; }
        }

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
        public IActionResult TestPost([FromBody] NumberDTO number)
        {
            return Ok(number.Number * 2);
        }
    }
}
