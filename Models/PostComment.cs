using System.ComponentModel.DataAnnotations;

namespace Fuwa.Models
{
    public class PostComment
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public int PostId { get; set; }
        [Required]
        public string? AuthorTag { get; set; }
        [Required]
        public string? Text { get; set; }
        [Required]
        public string? CreatedDate { get; set; }
        [Required]
        public string? LastModifiedDate { get; set; }
    }
}
