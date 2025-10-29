using GoRegister.ApplicationCore.Data.Enums;
using System;
using System.Collections.Generic;

namespace GoRegister.ApplicationCore.Domain.Registration.Models
{
    public class EmailAuditListModel
    {
        public Guid BatchId { get; set; }

        public int EmailTemplateId { get; set; }

        public string From { get; set; }

        public string FromName { get; set; }

        public string PlainTextContent { get; set; }

        public string HtmlTextContent { get; set; }

        public EmailType EmailType { get; set; }

        public IEnumerable<EmailAuditModel> EmailAuditList { get; set; }
    }
}
