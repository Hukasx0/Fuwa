using Fuwa.Models;

/*
 *  For returning user data via API / displaying on a website
 */

namespace Fuwa.ViewModels
{
    public class UserViewModel
    {
        public string? Tag { get; set; }
        // TODO: avatar
        public string? Username { get; set; }
        public Rank Rank { get; set; }
        public string? Bio { get; set; }
        public string? GithubProfile { get; set; }
        public string? Company { get; set; }
        public string? Location { get; set; }
        public string? PersonalWebsite { get; set; }
    }
}
