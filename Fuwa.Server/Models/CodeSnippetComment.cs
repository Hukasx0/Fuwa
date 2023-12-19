using Fuwa.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fuwa.Server.Models
{
    public class CodeSnippetComment
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public int CodeSnippetId { get; set; }

        [ForeignKey(nameof(CodeSnippetId))]
        public CodeSnippet? CodeSnippet { get; set; }

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
