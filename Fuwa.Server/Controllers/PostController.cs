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

        [HttpGet]
        public async Task<IActionResult> GetPosts([FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 10)
        {
            pageSize = Math.Min(pageSize, 10);
            var posts = await _context.Posts
                .Include(p => p.Author)
                .OrderByDescending(p => p.Id)
                .Skip(pageIndex)
                .Take(pageSize)
                .Select(p => new PostViewModel
                {
                    Id = p.Id,
                    PostedBy = new ShortUserDataViewModel
                    {
                        Tag = p.AuthorTag,
                        Username = p.Author.Username
                    },
                    Title = p.Title,
                    Text = p.Text,
                    CreatedDate = p.CreatedDate,
                    LastModifiedDate = p.LastModifiedDate
                }).ToListAsync();
            return Ok(posts);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPost([FromBody] PostModel postData)
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
                return Ok(new { postId = newPost.Id });
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
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

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdatePost([FromRoute] int id, [FromBody] PostModel editData)
        {
            var userTag = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var post = await _context.Posts
                .Include(p => p.Author)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
            {
                return NotFound($"Post with id {id} not found.");
            }

            if (post.Author.Tag != userTag)
            {
                return Forbid();
            }

            if (!string.IsNullOrWhiteSpace(editData.title))
            {
                post.Title = editData.title;
            }

            if (!string.IsNullOrWhiteSpace(editData.text))
            {
                post.Text = editData.text;
            }

            post.LastModifiedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok($"Post {id} updated successfully.");
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> RemovePost(int id)
        {
            var userTag = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var post = await _context.Posts
                .Include(p => p.Author)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
            {
                return NotFound($"Post with ID {id} not found.");
            }

            if (userTag != post.AuthorTag)
            {
                return Unauthorized();
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return Ok($"Post with ID {id} deleted successfully.");
        }

        [HttpGet("{id}/comments")]
        public async Task<IActionResult> GetPostComments([FromRoute] int id)
        {
            var post = await _context.Posts
                .Include(p => p.PostComments)
                .ThenInclude(pc => pc.Author)
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

        [HttpPost("{id}/comments")]
        [Authorize]
        public async Task<IActionResult> AddPostComment([FromRoute] int id, [FromBody] CommentModel commentData)
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

        [HttpPut("{id}/comments/{commentId}")]
        [Authorize]
        public async Task<IActionResult> UpdatePostComment(int id, int commentId, [FromBody] CommentModel editData)
        {
            var userTag = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var postComment = await _context.PostComments
                .Include(pc => pc.Author)
                .FirstOrDefaultAsync(pc => pc.Id == commentId && pc.PostId == id);

            if (postComment == null)
            {
                return NotFound($"Comment with ID {commentId} for Post ID {id} not found.");
            }

            if (userTag != postComment.AuthorTag)
            {
                return Unauthorized();
            }

            if (!string.IsNullOrWhiteSpace(editData.text))
            {
                postComment.Text = editData.text;
            }

            postComment.LastModifiedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok($"Comment with ID {commentId} for Post ID {id} updated successfully.");
        }

        [HttpDelete("{id}/comments/{commentId}")]
        [Authorize]
        public async Task<IActionResult> RemovePostComment(int id, int commentId)
        {
            var userTag = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var postComment = await _context.PostComments
                .Include(pc => pc.Author)
                .FirstOrDefaultAsync(pc => pc.Id == commentId && pc.PostId == id);

            if (postComment == null)
            {
                return NotFound($"Comment with ID {commentId} for Post ID {id} not found.");
            }

            if (userTag != postComment.AuthorTag)
            {
                return Unauthorized();
            }

            _context.PostComments.Remove(postComment);
            await _context.SaveChangesAsync();

            return Ok($"Comment with ID {commentId} for Post ID {id} deleted successfully.");
        }
    }
}

public class PostModel
{
    public string? title { get; set; }
    public string? text { get; set; }
}

public class CommentModel
{
    public string? text { get; set; }
}
