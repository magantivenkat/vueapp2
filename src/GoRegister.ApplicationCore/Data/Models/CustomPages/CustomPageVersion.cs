using System;

namespace GoRegister.ApplicationCore.Data.Models
{
    public class CustomPageVersion
    {
        public int Id { get; set; }

        public int CustomPageId { get; set; }

        public CustomPage CustomPage { get; set; }

        public string Title { get; set; }
        public string Content { get; set; }
        public string Slug { get; set; }

        public bool IsVisible { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}