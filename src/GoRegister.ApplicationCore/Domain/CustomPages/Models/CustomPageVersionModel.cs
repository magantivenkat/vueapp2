using System;

namespace GoRegister.ApplicationCore.Domain.CustomPages.Models
{
    public class CustomPageVersionModel
    {
        public int Id { get; set; }

        public int CustomPageId { get; set; }

        public string Title { get; set; }
        public string Content { get; set; }
        public string Slug { get; set; }

        public bool IsVisible { get; set; }

        public DateTime TimeStamp { get; set; }

        public string TimeStampFormat { get; set; }

        public string HumanizedDateSent { get; set; }

        public string ToolTipAuditInfo { get; set; }
    }
}
