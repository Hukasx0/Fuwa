using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fuwa.Models
{
    public class PostComment
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public int PostId { get; set; }

        [ForeignKey(nameof(PostId))]
        public Post? Post { get; set; }

        [Required]
        public string? AuthorTag { get; set; }

        [ForeignKey(nameof(AuthorTag))]
        public User? Author { get; set; }

        [Required]
        public string? Text { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime LastModifiedDate { get; set; }
    }
}
