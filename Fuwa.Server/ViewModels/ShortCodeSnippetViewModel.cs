using Fuwa.Models;

namespace Fuwa.Server.ViewModels
{
    public class ShortCodeSnippetViewModel
    {
        public int Id { get; set; }
        public string? AuthorTag { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public MixedFromViewModel? MixedFrom { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public CodeLanguage CodeLanguage { get; set; }
    }
}
