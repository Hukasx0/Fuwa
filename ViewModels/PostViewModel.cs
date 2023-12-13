﻿/*
 *  For returning posts via API / displaying on a website
 */

namespace Fuwa.ViewModels
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public ShortUserDataViewModel? PostedBy { get; set; }
        public string? Title { get; set; }
        public string? Text { get; set; }
        public string? CreatedDate { get; set; }
        public string? LastModifiedDate { get; set; }
    }
}
