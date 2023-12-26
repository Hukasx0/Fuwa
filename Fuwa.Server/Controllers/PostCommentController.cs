using Fuwa.Models;
using Fuwa.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Fuwa.Server.Controllers
{
    public partial class PostController : ControllerBase
    {
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

public class CommentModel
{
    public string? text { get; set; }
}