using Fuwa.Data;
using Fuwa.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Fuwa.Controllers
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

        [HttpGet("{tag}", Name = "GetUserByTag")]
        public async Task<IActionResult> GetUserByTag([FromRoute] string tag)
        {
            var user = await _context.Users
                .Include(u => u.CodeSnippets)
                .Include(u => u.Posts)
                .Include(u => u.PostComments)
                .FirstOrDefaultAsync(u => u.Tag == tag);
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
                CodeSnippets = user.CodeSnippets.Select(cs => new CodeSnippetViewModel
                {
                    Id = cs.Id,
                    PostedBy = new ShortUserDataViewModel
                    {
                        Tag = user.Tag,
                        Username = user.Username
                    },
                    Title = cs.Title,
                    Description = cs.Description,
                    Code = cs.Code,
                    CreatedDate = cs.CreatedDate,
                    LastModifiedDate = cs.LastModifiedDate,
                    CodeLanguage = cs.CodeLanguage,
                    LikedBy = cs.LikedBy.Select(lb => new ShortUserDataViewModel
                    {
                        Tag = lb.Tag,
                        Username = lb.Username
                    }).ToList()
                }).ToList(),
                Posts = user.Posts.Select(post => new PostViewModel
                {
                    Id = post.Id,
                    PostedBy = new ShortUserDataViewModel
                    {
                        Tag = user.Tag,
                        Username = user.Username
                    },
                    Title = post.Title,
                    Text = post.Text,
                    CreatedDate = post.CreatedDate,
                    LastModifiedDate= post.LastModifiedDate
                }).ToList(),
                PostComments = user.PostComments.Select(postComment => new PostCommentViewModel
                {
                    Id = postComment.Id,
                    PostedBy = new ShortUserDataViewModel
                    {
                        Tag = user.Tag,
                        Username = user.Username
                    },
                    Text = postComment.Text,
                    CreatedDate = postComment.CreatedDate,
                    LastModifiedDate = postComment.LastModifiedDate
                }).ToList()
            };
            return Ok(displayUser);
        }

        [HttpGet("{usertag}/codeSnippets")]
        public async Task<IActionResult> GetUserCodeSnippets(string usertag)
        {
            var user = await _context.Users
                .Include(u => u.CodeSnippets)
                .FirstOrDefaultAsync(u => u.Tag  == usertag);
            if (user == null)
            {
                return NotFound($"User with tag {usertag} not found.");
            }
            var displaySnippets = user.CodeSnippets.Select(cs => new CodeSnippetViewModel
            {
                Id = cs.Id,
                PostedBy = new ShortUserDataViewModel
                {
                    Tag = user.Tag,
                    Username = user.Username
                },
                Title = cs.Title,
                Description = cs.Description,
                Code = cs.Code,
                CreatedDate = cs.CreatedDate,
                LastModifiedDate = cs.LastModifiedDate,
                CodeLanguage = cs.CodeLanguage,
                LikedBy = cs.LikedBy.Select(lb => new ShortUserDataViewModel
                {
                    Tag = lb.Tag,
                    Username = lb.Username
                }).ToList(),
            }).ToList();
            return Ok(displaySnippets);
        }

        [HttpGet("{usertag}/codeSnippets/{snippetName}")]
        public async Task<IActionResult> GetUserCodeSnippetBySlug(string usertag, string snippetName)
        {
            var user = await _context.Users
                .Include(u => u.CodeSnippets)
                .FirstOrDefaultAsync(u => u.Tag == usertag);
            if (user == null)
            {
                return NotFound($"User with tag {usertag} not found.");
            }
            var codeSnippet = user.CodeSnippets.FirstOrDefault(cs => cs.Title == snippetName);
            if (codeSnippet == null)
            {
                return NotFound($"Code snippet with title {snippetName} not found for user {usertag}.");
            }
            var displaySnippet = new CodeSnippetViewModel
            {
                Id = codeSnippet.Id,
                PostedBy = new ShortUserDataViewModel
                {
                    Tag = user.Tag,
                    Username = user.Username
                },
                Title = codeSnippet.Title,
                Description = codeSnippet.Description,
                Code = codeSnippet.Code,
                CreatedDate = codeSnippet.CreatedDate,
                LastModifiedDate = codeSnippet.LastModifiedDate,
                CodeLanguage = codeSnippet.CodeLanguage,
                LikedBy = codeSnippet.LikedBy.Select(lb => new ShortUserDataViewModel
                {
                    Tag = lb.Tag,
                    Username = lb.Username
                }).ToList()
            };
            return Ok(displaySnippet);
        }

        [HttpPut("{usertag}/codeSnippets/{snippetName}")]
        [Authorize]
        public async Task<IActionResult> UpdateUserCodeSnippetBySlug(string usertag, string snippetName, [FromBody] EditCodeSnippetModel editData)
        {
            var user = await _context.Users
                .Include(u => u.CodeSnippets)
                .FirstOrDefaultAsync(u => u.Tag == usertag);
            var userTag = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (user == null)
            {
                return NotFound($"User with tag {usertag} not found.");
            }
            else if (userTag != user.Tag)
            {
                return Unauthorized();
            }
            var codeSnippet = user.CodeSnippets.FirstOrDefault(cs => cs.Title == snippetName);
            if (codeSnippet == null)
            {
                return NotFound($"Code snippet with title {snippetName} not found for user {usertag}.");
            }
            if (!string.IsNullOrWhiteSpace(editData.description))
            {
                codeSnippet.Description = editData.description;
            }
            if (!string.IsNullOrWhiteSpace(editData.code))
            {
                codeSnippet.Code = editData.code;
            }
            codeSnippet.LastModifiedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return Ok($"Code snippet {snippetName} for user {usertag} updated successfully.");
        }

        [HttpDelete("{usertag}/codeSnippets/{snippetName}")]
        [Authorize]
        public async Task<IActionResult> RemoveUserCodeSnippetBySlug(string usertag, string snippetName)
        {
            var user = await _context.Users
                .Include(u => u.CodeSnippets)
                .FirstOrDefaultAsync(u => u.Tag == usertag);
            var userTag = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (user == null)
            {
                return NotFound($"User with tag {usertag} not found.");
            }
            else if (userTag != user.Tag)
            {
                return Unauthorized();
            }
            var codeSnippet = user.CodeSnippets.FirstOrDefault(cs => cs.Title == snippetName);

            if (codeSnippet == null)
            {
                return NotFound($"Code snippet with title {snippetName} not found for user {usertag}.");
            }

            _context.CodeSnippets.Remove(codeSnippet);
            await _context.SaveChangesAsync();

            return Ok($"Code snippet {snippetName} for user {usertag} deleted successfully.");
        }

        [HttpPost("{usertag}/codeSnippets/{snippetName}/like")]
        [Authorize]
        public async Task<IActionResult> LikeCodeSnippet(string usertag, string snippetName)
        {
            var userTag = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _context.Users
                .Include(u => u.LikedSnippets)
                .FirstOrDefaultAsync(u => u.Tag == userTag);

            if (user == null)
            {
                return NotFound($"User with tag {userTag} not found.");
            }

            var codeSnippet = await _context.CodeSnippets
                .FirstOrDefaultAsync(cs => cs.AuthorTag == usertag && cs.Title == snippetName);

            if (codeSnippet == null)
            {
                return NotFound($"Code snippet with title {snippetName} not found for user {usertag}.");
            }

            if (!user.LikedSnippets.Contains(codeSnippet))
            {
                user.LikedSnippets.Add(codeSnippet);
                await _context.SaveChangesAsync();
                return Ok($"Code snippet {snippetName} for user {usertag} liked successfully.");
            }
            else
            {
                user.LikedSnippets.Remove(codeSnippet);
                await _context.SaveChangesAsync();
                return Ok($"Code snippet {snippetName} for user {usertag} has been removed from liked snippets.");
            }
        }

        [HttpGet("{usertag}/posts")]
        public async Task<IActionResult> GetUserPosts(string usertag)
        {
            var user = await _context.Users
                .Include(u => u.Posts)
                .FirstOrDefaultAsync(u => u.Tag == usertag);
            if (user == null)
            {
                return NotFound($"User with tag {usertag} not found.");
            }
            var displayPosts = user.Posts.Select(p => new PostViewModel
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
        public async Task<IActionResult> GetUserComments(string usertag)
        {
            var user = await _context.Users
                .Include(u => u.PostComments)
                .FirstOrDefaultAsync(u => u.Tag == usertag);
            if (user == null)
            {
                return NotFound($"User with tag {usertag} not found.");
            }
            var displayComments = user.PostComments.Select(pc => new PostCommentViewModel
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

        [HttpPut("{tag}")]
        [Authorize]
        public async Task<IActionResult> UpdateUserData(string tag, [FromBody] UpdateUserModel newUserData)
        {
            var jwtUserTag = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (jwtUserTag != tag)
            {
                return Forbid();
            }
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Tag == tag);
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

        [HttpPut("{tag}/password")]
        [Authorize]
        public async Task<IActionResult> UpdateUserPassword(string tag, [FromBody] ChangePasswordModel passwords)
        {
            var jwtUserTag = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (jwtUserTag != tag)
            {
                return Forbid();
            }
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Tag == tag);
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

public class EditCodeSnippetModel
{
    public string? description { get; set; }
    public string? code { get; set; }
}