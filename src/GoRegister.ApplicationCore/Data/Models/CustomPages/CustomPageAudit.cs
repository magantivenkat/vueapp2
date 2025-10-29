using System;
using System.Collections.Generic;

namespace GoRegister.ApplicationCore.Data.Models
{
    public class CustomPageAudit
    {
        public int Id { get; set; }

        public int CustomPageId { get; set; }

        public CustomPage CustomPage { get; set; }

        public bool PageStatus { get; set; }

        public DateTime TimeStamp { get; set; }

        public ICollection<CustomPageAuditRegistrationType> CustomPageAuditRegistrationTypes { get; set; } = new List<CustomPageAuditRegistrationType>();
    }
}
