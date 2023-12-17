using Fuwa.Data;
using Fuwa.Models;
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
    public class PostController : ControllerBase
    {
        private readonly FuwaDbContext _context;

        public PostController(FuwaDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById([FromRoute] int id)
        {
            var post = await _context.Posts
                .Include(p => p.Author)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
            {
                return NotFound();
            }
            var displayPost = new PostViewModel
            {
                Id = id,
                PostedBy = new ShortUserDataViewModel
                {
                    Tag = post.AuthorTag,
                    Username = post.Author.Username
                },
                Title = post.Title,
                Text = post.Text,
                CreatedDate = post.CreatedDate,
                LastModifiedDate = post.LastModifiedDate
            };
            return Ok(displayPost);
        }

        [HttpGet("{id}/comments")]
        public async Task<IActionResult> GetPostComments([FromRoute] int id)
        {
            var post = await _context.Posts
                .Include(p => p.PostComments)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
            {
                return NotFound();
            }

            var comments = post.PostComments
                .Select(comment => new PostCommentViewModel
                {
                    Id = comment.Id,
                    PostedBy = new ShortUserDataViewModel
                    {
                        Tag = comment.AuthorTag,
                        Username = comment.Author.Username
                    },
                    Text = comment.Text,
                    CreatedDate = comment.CreatedDate,
                    LastModifiedDate = comment.LastModifiedDate
                })
                .ToList();

            return Ok(comments);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPost([FromBody] AddPostModel postData)
        {
            if (postData == null ||
                string.IsNullOrWhiteSpace(postData.title) ||
                string.IsNullOrWhiteSpace(postData.text))
            {
                return BadRequest("Invalid data");
            }
            try
            {
                var userTag = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var user = await _context.Users
                    .SingleOrDefaultAsync(u => u.Tag == userTag);

                if (user == null)
                {
                    return NotFound("User not found");
                }
                var newPost = new Post
                {
                    AuthorTag = userTag,
                    Author = user,
                    Title = postData.title,
                    Text = postData.text,
                    CreatedDate = DateTime.UtcNow
                };
                _context.Posts.Add(newPost);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception) 
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("{id}/comment")]
        [Authorize]
        public async Task<IActionResult> AddPostComment([FromRoute] int id, [FromBody] AddCommentModel commentData)
        {
            if (commentData == null ||
                string.IsNullOrWhiteSpace(commentData.text))
            {
                return BadRequest("Invalid data");
            }

            try
            {
                var userTag = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var user = await _context.Users.SingleOrDefaultAsync(u => u.Tag == userTag);
                if (user == null)
                {
                    return NotFound("User not found");
                }

                var post = await _context.Posts
                    .Include(p => p.PostComments)
                    .SingleOrDefaultAsync(p => p.Id == id);

                if (post == null)
                {
                    return NotFound($"Post with ID {id} not found");
                }

                var newComment = new PostComment
                {
                    PostId = post.Id,
                    Post = post,
                    AuthorTag = userTag,
                    Author = user,
                    Text = commentData.text,
                    CreatedDate = DateTime.UtcNow
                };

                post.PostComments.Add(newComment);
                await _context.SaveChangesAsync();

                return Ok("Comment added successfully");
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

public class AddPostModel
{
    public string? title { get; set; }
    public string? text { get; set; }
}

public class AddCommentModel
{
    public string? text { get; set; }
}
