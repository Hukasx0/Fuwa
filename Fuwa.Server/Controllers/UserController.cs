using Fuwa.Data;
using Fuwa.Models;
using Fuwa.Server.Models;
using Fuwa.Server.ViewModels;
using Fuwa.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Fuwa.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly FuwaDbContext _context;

        public UserController(FuwaDbContext context)
        {
            _context = context;
        }

        [HttpGet("{usertag}", Name = "GetUserByTag")]
        public async Task<IActionResult> GetUserByTag([FromRoute] string usertag)
        {
            var user = await _context.Users
                .Include(u => u.CodeSnippets)
                .Include(u => u.Posts)
                .Include(u => u.PostComments)
                .FirstOrDefaultAsync(u => u.Tag == usertag);
            if (user == null)
            {
                return NotFound();
            }
            var displayUser = new UserViewModel
            {
                Tag = user.Tag,
                Username = user.Username,
                Rank = user.Rank,
                Bio = user.Bio,
                GithubProfile = user.GithubProfile,
                Company = user.Company,
                Location = user.Location,
                PersonalWebsite = user.PersonalWebsite,
            };
            return Ok(displayUser);
        }

        [HttpGet("{usertag}/codeSnippets")]
        public async Task<IActionResult> GetUserCodeSnippets(string usertag, [FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 10)
        {
            var user = await _context.Users
                .Include(u => u.CodeSnippets)
                .ThenInclude(cs => cs.MixedFrom)
                .FirstOrDefaultAsync(u => u.Tag  == usertag);
            if (user == null)
            {
                return NotFound($"User with tag {usertag} not found.");
            }
            pageSize = Math.Min(pageSize, 10);
            var displaySnippets = user.CodeSnippets
                .OrderByDescending(cs => cs.Id)
                .Skip(pageIndex)
                .Take(pageSize)
                .Select(cs => new ShortCodeSnippetViewModel
                {
                    Id = cs.Id,
                    AuthorTag = cs.AuthorTag,
                    Title = cs.Title,
                    Description = cs.Description,
                    MixedFrom = cs.MixedFrom != null
                ? new MixedFromViewModel
                {
                    Title = cs.MixedFrom.Title,
                    AuthorTag = cs.MixedFrom.Author?.Tag
                }
                : null,
                    CreatedDate = cs.CreatedDate,
                    LastModifiedDate = cs.LastModifiedDate,
                    CodeLanguage = cs.CodeLanguage
                });
            return Ok(displaySnippets);
        }

        [HttpGet("{usertag}/posts")]
        public async Task<IActionResult> GetUserPosts(string usertag, [FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 10)
        {
            var user = await _context.Users
                .Include(u => u.Posts)
                .FirstOrDefaultAsync(u => u.Tag == usertag);
            if (user == null)
            {
                return NotFound($"User with tag {usertag} not found.");
            }
            pageSize = Math.Min(pageSize, 10);
            var displayPosts = user.Posts
                .OrderByDescending(p => p.Id)
                .Skip(pageIndex)
                .Take(pageSize)
                .Select(p => new PostViewModel
            {
                Id = p.Id,
                PostedBy = new ShortUserDataViewModel
                {
                    Tag = user.Tag,
                    Username = user.Username
                },
                Title = p.Title,
                Text = p.Text,
                CreatedDate = p.CreatedDate,
                LastModifiedDate = p.LastModifiedDate
            }).ToList();
            return Ok(displayPosts);
        }

        [HttpGet("{usertag}/comments")]
        public async Task<IActionResult> GetUserComments(string usertag, [FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 10)
        {
            var user = await _context.Users
                .Include(u => u.PostComments)
                .FirstOrDefaultAsync(u => u.Tag == usertag);
            if (user == null)
            {
                return NotFound($"User with tag {usertag} not found.");
            }
            pageSize = Math.Min(pageSize, 10);
            var displayComments = user.PostComments
                .OrderByDescending(pc => pc.Id)
                .Skip(pageIndex)
                .Take(pageSize)
                .Select(pc => new PostCommentViewModel
            {
                Id = pc.Id,
                PostedBy = new ShortUserDataViewModel
                {
                    Tag = user.Tag,
                    Username = user.Username
                },
                Text = pc.Text,
                CreatedDate = pc.CreatedDate,
                LastModifiedDate = pc.LastModifiedDate
            }).ToList();
            return Ok(displayComments);
        }

        [HttpGet("{usertag}/likes")]
        public async Task<IActionResult> GetUserLikes(string usertag, [FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 10)
        {
            var user = await _context.Users
                .Include(u => u.LikedSnippets)
                .FirstOrDefaultAsync(u => u.Tag == usertag);
            if (user == null)
            {
                return NotFound($"User with tag {usertag} not found.");
            }
            pageSize = Math.Min(pageSize, 10);
            var displayLikedSnippets = user.LikedSnippets
                .OrderByDescending(cs => cs.Id)
                .Skip(pageIndex)
                .Take(pageSize)
                .Select(cs => new ShortCodeSnippetViewModel
                {
                    Id = cs.Id,
                    AuthorTag = cs.AuthorTag,
                    Title = cs.Title,
                    Description = cs.Description,
                    MixedFrom = cs.MixedFrom != null
                ? new MixedFromViewModel
                {
                    Title = cs.MixedFrom.Title,
                    AuthorTag = cs.MixedFrom.Author?.Tag
                }
                : null,
                    CreatedDate = cs.CreatedDate,
                    LastModifiedDate = cs.LastModifiedDate,
                    CodeLanguage = cs.CodeLanguage
                });
            return Ok(displayLikedSnippets);
        }

        [HttpPut("{usertag}")]
        [Authorize]
        public async Task<IActionResult> UpdateUserData(string usertag, [FromBody] UpdateUserModel newUserData)
        {
            var jwtUserTag = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (jwtUserTag != usertag)
            {
                return Forbid();
            }
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Tag == usertag);
            if (user == null)
            {
                return NotFound();
            }
            if (!string.IsNullOrWhiteSpace(newUserData.username))
            {
                user.Username = newUserData.username;
            }
            if (!string.IsNullOrWhiteSpace(newUserData.bio))
            {
                user.Bio = newUserData.bio;
            }
            if (!string.IsNullOrWhiteSpace(newUserData.githubProfile))
            {
                user.GithubProfile = newUserData.githubProfile;
            }
            if (!string.IsNullOrWhiteSpace(newUserData.company))
            {
                user.Company = newUserData.company;
            }
            if (!string.IsNullOrWhiteSpace(newUserData.location))
            {
                user.Location = newUserData.location;
            }
            if (!string.IsNullOrWhiteSpace(newUserData.personalWebsite))
            {
                user.PersonalWebsite = newUserData.personalWebsite;
            }
            await _context.SaveChangesAsync();
            return Ok($"User {user.Tag} data updated successfully");
        }

        [HttpPut("{usertag}/password")]
        [Authorize]
        public async Task<IActionResult> UpdateUserPassword(string usertag, [FromBody] ChangePasswordModel passwords)
        {
            var jwtUserTag = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (jwtUserTag != usertag)
            {
                return Forbid();
            }
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Tag == usertag);
            if (user == null) 
            {
                return NotFound();
            }
            if (!BCrypt.Net.BCrypt.EnhancedVerify(passwords.oldPassword, user.Password))
            {
                return BadRequest("incorrect old password was entered");
            }
            string newPasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(passwords.newPassword, 13);
            user.Password = newPasswordHash;
            await _context.SaveChangesAsync();
            return Ok($"Password for user {user.Tag} changed successfully");
        }
    }
}

public class UpdateUserModel
{
    public string? username { get; set; }
    public string? bio { get; set; }
    public string? githubProfile { get; set; }
    public string? company { get; set; }
    public string? location { get; set; }
    public string? personalWebsite { get; set; }
}

public class ChangePasswordModel
{
    public string? oldPassword { get; set; }
    public string? newPassword { get; set; }
}
