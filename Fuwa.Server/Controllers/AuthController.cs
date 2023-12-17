using Fuwa.Data;
using Fuwa.Identity;
using Fuwa.Models;
using Fuwa.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace Fuwa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly FuwaDbContext _context;

        public AuthController(FuwaDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] UserRegister registerData)
        {
            if (registerData is null)
            {
                return BadRequest("Invalid data");
            }
            else if (!UserTagValidator(registerData.userTag))
            {
                return BadRequest("The user tag can only consist of lowercase letters, uppercase letters and numbers");
            }
            else if (!EmailValidator(registerData.email))
            {
                return BadRequest("Invalid email address format");
            }
            else if (await _context.Users.AnyAsync(u => u.Email == registerData.email || u.Tag == registerData.userTag))
            {
                return BadRequest("The user with the specified e-mail address or user tag already exists.");
            }
            string passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(registerData.password, 13);
            var newUser = new User
            {
                Tag = registerData.userTag,
                Email = registerData.email,
                Username = registerData.username,
                Password = passwordHash,
                Rank = Rank.User,
            };
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
            var viewModel = new ShortUserDataViewModel
            {
                Tag = newUser.Tag,
                Username = newUser.Username,
            };
            return CreatedAtRoute("GetUserByTag", new { controller = "User", tag = registerData.userTag }, viewModel);
        }

        private bool UserTagValidator(string userTag)
        {
            return Regex.IsMatch(userTag, "^[a-zA-Z0-9]+$");
        }

        private bool EmailValidator(string email)
        {
            var valid = true;
            try
            {
                var mailAddr = new MailAddress(email);
            }
            catch
            {
                valid = false;
            }
            return valid;
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] UserLogin loginData)
        {
            if (loginData is null)
            {
                return BadRequest("Invalid data");
            }

            if (string.IsNullOrWhiteSpace(loginData.userTagOrMail) || string.IsNullOrWhiteSpace(loginData.password))
            {
                return BadRequest("Both identifier (user tag or email address) and password must be provided");
            }

            User? user = null;

            if (!EmailValidator(loginData.userTagOrMail))
            {
                user = await _context.Users.FirstOrDefaultAsync(u => u.Tag == loginData.userTagOrMail);
            }
            else
            {
                user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginData.userTagOrMail);
            }

            if (user is null || !BCrypt.Net.BCrypt.EnhancedVerify(loginData.password, user.Password))
            {
                return Unauthorized("Incorrect identifier (user tag or email address) or password");
            }

            var token = GenerateJwtToken(user.Tag);
            return Ok(new { token });
        }

        private string GenerateJwtToken(string userTag)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, userTag),
            };
            var token = new JwtSecurityToken(
                issuer: JwtSettings.Issuer,
                audience: JwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );
            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenStr;
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
    public string? userTagOrMail { get; set; }
    public string? password { get; set; }
}
