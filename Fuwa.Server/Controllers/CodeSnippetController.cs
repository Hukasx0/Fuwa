using Fuwa.Data;
using Fuwa.Models;
using Fuwa.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            var codeSnippet = await _context.CodeSnippets.FindAsync(id);
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
                    Username = codeSnippet.Author.Username
                },
                Title = codeSnippet.Title,
                Description = codeSnippet.Description,
                Code = codeSnippet.Code,
                CreatedDate = codeSnippet.CreatedDate,
                LastModifiedDate = codeSnippet.LastModifiedDate,
                CodeLanguage = codeSnippet.CodeLanguage,
                LikedBy = (ICollection<ShortUserDataViewModel>)codeSnippet.LikedBy
            };
            return Ok(displaySnippet);
        }

        [HttpPost]
        public async Task<IActionResult> AddCodeSnippet([FromBody] AddCodeSnippetBody snippetData)
        {
            if (snippetData == null)
            {
                return BadRequest("Invalid data");
            }
            try
            {
                var newSnippet = new CodeSnippet
                {
                    AuthorTag = "test",
                    Title = snippetData.title,
                    Description = snippetData.title,
                    Code = snippetData.code,
                    CreatedDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                    CodeLanguage = CodeLanguage.C,
                };

                _context.CodeSnippets.Add(newSnippet);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetCodeSnippetById), new { newSnippet.Id });

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
    public string title { get; set; }
    public string description { get; set; }
    public string code { get; set; }
}