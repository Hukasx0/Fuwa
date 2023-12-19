using Fuwa.ViewModels;

namespace Fuwa.Server.ViewModels
{
    public class MixViewModel
    {
        public string? Title { get; set; }
        public ShortUserDataViewModel? Author { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
