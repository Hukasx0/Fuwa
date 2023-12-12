using System.ComponentModel.DataAnnotations;

namespace Fuwa.Models
{
    public class Post
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string? AuthorTag { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Text { get; set; }
        [Required]
        public string? CreatedDate { get; set; }
        [Required]
        public string? LastModifiedDate { get; set; }
    }
}
