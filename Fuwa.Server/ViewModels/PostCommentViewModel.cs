/*
 *  For returning post comments via API / displaying on a website
 */

namespace Fuwa.ViewModels
{
    public class PostCommentViewModel
    {
        public int Id { get; set; }
        public ShortUserDataViewModel? PostedBy { get; set; }
        public string? Text { get; set; }
        public string? CreatedDate { get; set; }
        public string? LastModifiedDate { get; set; }
    }
}
