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

namespace Fuwa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CodeSnippetController : ControllerBase
    {
        private readonly FuwaDbContext _context;

        public CodeSnippetController(FuwaDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCodeSnippetById(int id)
        {
            var codeSnippet = await _context.CodeSnippets
                .Include(cs => cs.Author)
                .Include(cs => cs.LikedBy)
                .Include(cs => cs.MixedFrom)
                .Include(cs => cs.Mixes)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (codeSnippet == null)
            {
                return NotFound();
            }
            var displaySnippet = new CodeSnippetViewModel
            {
                Id = codeSnippet.Id,
                PostedBy = new ShortUserDataViewModel
                {
                    Tag = codeSnippet.AuthorTag,
                    Username = codeSnippet.Author?.Username,
                },
                Title = codeSnippet.Title,
                Description = codeSnippet.Description,
                Code = codeSnippet.Code,
                MixedFrom = codeSnippet.MixedFrom != null
                    ? new MixViewModel
                    {
                        Title = codeSnippet.MixedFrom.Title,
                        Author = new ShortUserDataViewModel
                        {
                            Tag = codeSnippet.MixedFrom.AuthorTag,
                            Username = codeSnippet.MixedFrom.Author?.Username,
                        },
                        CreatedDate = codeSnippet.MixedFrom.CreatedDate
                    }
                    : null,
                CreatedDate = codeSnippet.CreatedDate,
                LastModifiedDate = codeSnippet.LastModifiedDate,
                CodeLanguage = codeSnippet.CodeLanguage,
                Mixes = codeSnippet.Mixes?.Select(m => new MixViewModel
                {
                    Title = m.Title,
                    Author = new ShortUserDataViewModel
                    {
                        Tag = m.AuthorTag,
                        Username = m.Author?.Username,
                    },
                    CreatedDate = m.CreatedDate
                }).ToList(),
                LikedBy = codeSnippet.LikedBy.Select(lb => new ShortUserDataViewModel
                {
                    Tag = lb.Tag,
                    Username = lb.Username,
                }).ToList()
            };
            return Ok(displaySnippet);
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
    }
}

public class AddCodeSnippetBody
{
    public string? title { get; set; }
    public string? description { get; set; }
    public string? code { get; set; }
}