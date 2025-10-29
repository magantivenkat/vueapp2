using System;

namespace GoRegister.ApplicationCore.Domain.Registration.Models
{
    public class EmailAuditNotificationModel
    {
        public int Id { get; set; }

        public int EmailAuditId { get; set; }


        #region SendGrid Properties

        public string SgMessageId { get; set; }

        public string SgEventId { get; set; }

        public string EventType { get; set; }

        public string Email { get; set; }

        public string Category { get; set; }

        public DateTime Timestamp { get; set; }

        public string SmtpId { get; set; }

        public string Response { get; set; }

        public string Attempt { get; set; }

        public string UserAgent { get; set; }

        public string Ip { get; set; }

        public bool Tls { get; set; }

        public string Url { get; set; }

        public string Reason { get; set; }

        public string Status { get; set; }

        public string Type { get; set; }

        public int AsmGroupId { get; set; }

        #endregion
    }
}
