using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [InverseProperty("Author")]
        public ICollection<CodeSnippet> CodeSnippets { get; set; } = new List<CodeSnippet>();

        [InverseProperty("Author")]
        public ICollection<Post> Posts { get; set; } = new List<Post>();

        [InverseProperty("Author")]
        public ICollection<PostComment> PostComments { get; set; } = new List<PostComment>();

        [InverseProperty("LikedBy")]
        public ICollection<CodeSnippet> LikedSnippets { get; set; } = new List<CodeSnippet>();
    }

    public enum Rank
    {
        User,
        Bot,
        Admin
    }
}
