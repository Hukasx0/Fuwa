using Fuwa.Data;
using Fuwa.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("{tag}")]
        public async Task<IActionResult> GetUserByTag(string tag)
        {
            var user = await _context.Users.FindAsync(tag);
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
                CodeSnippets = (ICollection<CodeSnippetViewModel>)user.CodeSnippets,
                Posts = (ICollection<PostViewModel>)user.Posts,
                PostComments = (ICollection<PostCommentViewModel>)user.PostComments
            };
            return Ok(displayUser);
        }
    }
}
