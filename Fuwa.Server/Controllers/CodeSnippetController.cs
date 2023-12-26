using Fuwa.Data;
using Fuwa.Models;
using Fuwa.Server.ViewModels;
using Fuwa.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Fuwa.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class CodeSnippetController : ControllerBase
    {
        private readonly FuwaDbContext _context;

        public CodeSnippetController(FuwaDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddCodeSnippet([FromBody] AddCodeSnippetBody snippetData)
        {
            if (snippetData == null ||
                string.IsNullOrWhiteSpace(snippetData.title) ||
                string.IsNullOrWhiteSpace(snippetData.description) ||
                string.IsNullOrWhiteSpace(snippetData.code))
            {
                return BadRequest("Invalid data");
            }

            try
            {
                var userTag = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var user = await _context.Users
                    .Include(u => u.CodeSnippets)
                    .SingleOrDefaultAsync(u => u.Tag == userTag);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                if (user.CodeSnippets.Any(cs => cs.Title == snippetData.title))
                {
                    return BadRequest($"User {userTag} already has a snippet titled '{snippetData.title}'");
                }

                var newSnippet = new CodeSnippet
                {
                    AuthorTag = userTag,
                    Author = user,
                    Title = snippetData.title,
                    Description = snippetData.description,
                    Code = snippetData.code,
                    CreatedDate = DateTime.UtcNow,
                    // LastModifiedDate = DateTime.Now,
                    CodeLanguage = CodeLanguage.C,
                    LikedBy = new List<User>()
                };

                _context.CodeSnippets.Add(newSnippet);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{usertag}/{snippetName}")]
        public async Task<IActionResult> GetUserCodeSnippetByName(string usertag, string snippetName)
        {
            var user = await _context.Users
                .Include(u => u.CodeSnippets)
                .ThenInclude(cs => cs.MixedFrom)
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
            };
            if (codeSnippet.MixedFrom != null)
            {
                displaySnippet.MixedFrom = new MixedFromViewModel
                {
                    Title = codeSnippet.MixedFrom.Title,
                    AuthorTag = codeSnippet.MixedFrom.Author.Tag
                };
            }
            return Ok(displaySnippet);
        }

        [HttpPut("{usertag}/{snippetName}")]
        [Authorize]
        public async Task<IActionResult> UpdateUserCodeSnippetByName(string usertag, string snippetName, [FromBody] EditCodeSnippetModel editData)
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

        [HttpDelete("{usertag}/{snippetName}")]
        [Authorize]
        public async Task<IActionResult> RemoveUserCodeSnippetByName(string usertag, string snippetName)
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

        [HttpGet("{usertag}/{snippetName}/likes")]
        public async Task<IActionResult>  GetCodeSnippetLikes(string usertag, string snippetName, [FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 10)
        {
            var user = await _context.Users
                .Include(u => u.CodeSnippets)
                .ThenInclude(cs => cs.LikedBy)
                .FirstOrDefaultAsync(u => u.Tag == usertag);
            if (user == null)
            {
                return NotFound($"User with tag {usertag} not found.");
            }
            var codeSnippet = user.CodeSnippets
                .FirstOrDefault(cs => cs.Title == snippetName);
            if (codeSnippet == null)
            {
                return NotFound($"Code snippet with title {snippetName} not found for user {usertag}.");
            }
            pageSize = Math.Min(pageSize, 10);
            var codeSnippetLikes = codeSnippet.LikedBy
                .OrderBy(csu => csu.Tag)
                .Skip(pageIndex)
                .Take(pageSize)
                .Select(userData => new ShortUserDataViewModel
                {
                    Tag = userData.Tag,
                    Username = userData.Tag
                });
            return Ok(codeSnippetLikes);
        }

        [HttpGet("{usertag}/{snippetName}/mixes")]
        public async Task<IActionResult> GetCodeSnippetMixes(string usertag, string snippetName, [FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 10)
        {
            var user = await _context.Users
                .Include(u => u.CodeSnippets)
                .ThenInclude(cs => cs.Mixes)
                .FirstOrDefaultAsync(u => u.Tag == usertag);
            if (user == null)
            {
                return NotFound($"User with tag {usertag} not found.");
            }
            var codeSnippet = user.CodeSnippets
                .FirstOrDefault(cs => cs.Title == snippetName);
            if (codeSnippet == null)
            {
                return NotFound($"Code snippet with title {snippetName} not found for user {usertag}.");
            }
            pageSize = Math.Min(pageSize, 10);
            var codeSnippetMixes = codeSnippet.Mixes
                .OrderByDescending(csm => csm.Id)
                .Skip(pageIndex)
                .Take(pageSize)
                .Select(mixData => new MixViewModel
                {
                    Title = mixData.Title,
                    Author = new ShortUserDataViewModel
                    {
                        Tag = mixData.AuthorTag,
                        Username = mixData.Author.Username
                    },
                    CreatedDate = mixData.CreatedDate
                });
            return Ok(codeSnippetMixes);
        }

        [HttpPost("{usertag}/{snippetName}/like")]
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
                .Include(cs => cs.LikedBy)
                .FirstOrDefaultAsync(cs => cs.AuthorTag == usertag && cs.Title == snippetName);

            if (codeSnippet == null)
            {
                return NotFound($"Code snippet with title {snippetName} not found for user {usertag}.");
            }

            if (user.LikedSnippets.Contains(codeSnippet))
            {
                user.LikedSnippets.Remove(codeSnippet);
                codeSnippet.LikedBy.Remove(user);
                await _context.SaveChangesAsync();
                return Ok($"Code snippet {snippetName} for user {usertag} has been removed from liked snippets.");
            }
            else
            {
                user.LikedSnippets.Add(codeSnippet);
                codeSnippet.LikedBy.Add(user);
                await _context.SaveChangesAsync();
                return Ok($"Code snippet {snippetName} for user {usertag} liked successfully.");
            }
        }

        [HttpPost("{usertag}/{snippetName}/mix")]
        [Authorize]
        public async Task<IActionResult> MixCodeSnippet(string usertag, string snippetName, [FromBody] MixCodeSnippetModel mixData)
        {
            try
            {
                var userTag = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var user = await _context.Users
                    .Include(u => u.CodeSnippets)
                    .FirstOrDefaultAsync(u => u.Tag == userTag);

                if (user is null)
                {
                    return NotFound($"User with tag {userTag} not found.");
                }

                var codeSnippet = await _context.CodeSnippets
                    .FirstOrDefaultAsync(cs => cs.AuthorTag == usertag && cs.Title == snippetName);

                if (codeSnippet is null)
                {
                    return NotFound($"Code snippet with title {snippetName} not found for user {usertag}.");
                }

                string newTitle = "";

                if (mixData.title is null)
                {
                    var existingSnippets = user.CodeSnippets
                        .Where(cs => cs.Title.StartsWith($"{snippetName}-mix-"))
                        .Select(cs => cs.Title)
                        .ToList();

                    var maxCounter = existingSnippets
                        .Select(title => int.TryParse(title.Substring($"{snippetName}-mix-".Length), out int counter) ? counter : 0)
                        .DefaultIfEmpty(0)
                        .Max();

                    newTitle = $"{snippetName}-mix-{maxCounter + 1}";
                }
                else
                {
                    newTitle = mixData.title;
                }

                var newCodeSnippet = new CodeSnippet
                {
                    AuthorTag = userTag,
                    Author = user,
                    Title = newTitle,
                    Description = string.IsNullOrWhiteSpace(mixData.description) ? codeSnippet.Description : mixData.description,
                    Code = codeSnippet.Code,
                    CreatedDate = DateTime.UtcNow,
                    CodeLanguage = codeSnippet.CodeLanguage,
                    MixedFrom = codeSnippet
                };

                user.CodeSnippets.Add(newCodeSnippet);
                codeSnippet.Mixes.Add(newCodeSnippet);
                await _context.SaveChangesAsync();

                return Ok($"Code snippet {snippetName} forked successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

public class AddCodeSnippetBody
{
    public string? title { get; set; }
    public string? description { get; set; }
    public string? code { get; set; }
}

public class EditCodeSnippetModel
{
    public string? description { get; set; }
    public string? code { get; set; }
}

public class MixCodeSnippetModel
{
    public string? title { get; set; }
    public string? description { get; set; }
}
