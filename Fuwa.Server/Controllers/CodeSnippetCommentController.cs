using Fuwa.Server.Models;
using Fuwa.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Fuwa.Server.Controllers
{
    public partial class CodeSnippetController : ControllerBase
    {
        [HttpGet("{usertag}/{snippetName}/comments")]
        public async Task<IActionResult> GetCodeSnippetComments(string usertag, string snippetName, [FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 10)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.CodeSnippets)
                    .ThenInclude(cs => cs.Comments)
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

                pageSize = Math.Min(pageSize, 10);
                var comments = codeSnippet.Comments
                    .OrderBy(cs => cs.Id)
                    .Skip(pageIndex)
                    .Take(pageSize)
                    .Select(comment => new PostCommentViewModel
                {
                    Id = comment.Id,
                    PostedBy = new ShortUserDataViewModel
                    {
                        Tag = user.Tag,
                        Username = user.Username
                    },
                    Text = comment.Text,
                    CreatedDate = comment.CreatedDate,
                    LastModifiedDate = comment.LastModifiedDate
                });
                return Ok(comments);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("{usertag}/{snippetName}/comments")]
        [Authorize]
        public async Task<IActionResult> AddCodeSnippetComment(string usertag, string snippetName, [FromBody] CodeSnippetCommentModel commentModel)
        {
            try
            {
                var userTag = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var user = await _context.Users
                    .Include(u => u.CodeSnippets)
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

                var newComment = new CodeSnippetComment
                {
                    AuthorTag = userTag,
                    Author = user,
                    Text = commentModel.text,
                    CreatedDate = DateTime.UtcNow
                };

                codeSnippet.Comments.Add(newComment);
                await _context.SaveChangesAsync();

                return Ok("Comment added successfully");
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{usertag}/{snippetName}/comments/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateCodeSnippetComment(string usertag, string snippetName, int id, [FromBody] CodeSnippetCommentModel commentModel)
        {
            try
            {
                var userTag = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var user = await _context.Users
                    .Include(u => u.CodeSnippets)
                    .ThenInclude(cs => cs.Comments)
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

                var comment = codeSnippet.Comments.FirstOrDefault(c => c.Id == id);

                if (comment == null)
                {
                    return NotFound($"Comment with ID {id} not found for code snippet {snippetName}.");
                }

                comment.Text = commentModel.text;
                comment.LastModifiedDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok($"Comment with ID {comment.Id} for Post ID {id} updated successfully.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{usertag}/{snippetName}/comments/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteCodeSnippetComment(string usertag, string snippetName, int id)
        {
            try
            {
                var userTag = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var user = await _context.Users
                    .Include(u => u.CodeSnippets)
                    .ThenInclude(cs => cs.Comments)
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

                var comment = codeSnippet.Comments.FirstOrDefault(c => c.Id == id);

                if (comment == null)
                {
                    return NotFound($"Comment with ID {id} not found for code snippet {snippetName}.");
                }

                codeSnippet.Comments.Remove(comment);
                await _context.SaveChangesAsync();

                return Ok($"Comment with ID {id} for code snippet {snippetName} deleted successfully.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

public class CodeSnippetCommentModel
{
    public string? text { get; set; }
}
