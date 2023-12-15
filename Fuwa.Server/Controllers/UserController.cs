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
    }
}
