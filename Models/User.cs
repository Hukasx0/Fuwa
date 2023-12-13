using System.ComponentModel.DataAnnotations;

namespace Fuwa.Models
{
    public class User
    {
        [Key]
        [Required]
        public string? Tag { get; set; }
        [Required]
        public string? Username {  get; set; }
        [Required]
        // TODO: avatar
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public Rank Rank { get; set; }
        public string? Bio { get; set; }
        public string? GithubProfile { get; set; }
        public string? Company { get; set; }
        public string? Location { get; set; }
        public string? PersonalWebsite { get; set; }
        public ICollection<CodeSnippet> CodeSnippets { get; set; } = null!;
        public ICollection<Post> Posts { get; set; } = null!;
        public ICollection<PostComment> PostComments { get; set; } = null!;
        public ICollection<CodeSnippet> LikedSnippets { get; set; } = null!;
    }

    public enum Rank
    {
        User,
        Bot,
        Admin
    }
}
