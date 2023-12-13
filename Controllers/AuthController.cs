using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fuwa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Register([FromBody] UserRegister registerData)
        {
            return Ok("This is a placeholder function");
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Login([FromBody] UserLogin loginData)
        {
            return Ok("This is a placeholder function");
        }
    }
}

public class UserRegister
{
    public string? userTag { get; set; }
    public string? email { get; set; }
    public string? username { get; set; }
    public string? password { get; set; }
}

public class UserLogin
{
    // Ability to log in via user tag or email
    public string? userTag { get; set; }
    public string? email { set; get; }
    public string? password { get; set; }
}