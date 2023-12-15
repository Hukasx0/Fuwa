using Fuwa.Models;

/*
 *  For returning code snippets via API / displaying on a website
 */

namespace Fuwa.ViewModels
{
    public class CodeSnippetViewModel
    {
        public int Id { get; set; }
        public ShortUserDataViewModel? PostedBy { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Code { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public CodeLanguage CodeLanguage { get; set; }
        public ICollection<ShortUserDataViewModel>? LikedBy { get; set; }
    }
}
