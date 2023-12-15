/*
 *  For returning the user's tag and username, used when the user has posted something and we are to display only simple data to navigate to their profile
 */

namespace Fuwa.ViewModels
{
    public class ShortUserDataViewModel
    {
        public string? Tag { get; set; }
        // TODO: avatar
        public string? Username { get; set; }
    }
}
