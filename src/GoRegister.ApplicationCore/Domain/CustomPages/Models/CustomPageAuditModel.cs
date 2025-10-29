using System;
using System.Collections.Generic;

namespace GoRegister.ApplicationCore.Domain.CustomPages.Models
{
    public class CustomPageAuditModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public bool PageStatus { get; set; }

        public DateTime TimeStamp { get; set; }

        public ICollection<CustomPageAuditRegistrationTypeModel> CustomPageAuditRegistrationTypes { get; set; } = new List<CustomPageAuditRegistrationTypeModel>();

        public string TimeStampFormat { get; set; }

        public string HumanizedDateSent { get; set; }

        public string ToolTipAuditInfo { get; set; }
    }
}
