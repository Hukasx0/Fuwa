using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fuwa.Models
{
    public class Post
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public string? AuthorTag { get; set; }

        [ForeignKey(nameof(AuthorTag))]
        public User? Author { get; set; }

        [Required]
        public string? Title { get; set; }

        [Required]
        public string? Text { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime LastModifiedDate { get; set; }
    }
}
