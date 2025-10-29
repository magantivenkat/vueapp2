using GoRegister.ApplicationCore.Data.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace GoRegister.ApplicationCore.Domain.Registration.Models
{
    public class EmailAuditModel
    {
        public int Id { get; set; } // Id(send this as custom argument to sendgrid) Guid?

        public Guid BatchId { get; set; }

        public int DelegateUserId { get; set; }

        [Display(Name = "Status")]
        public string RegistrationStatus { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Name => $"{FirstName} {LastName}";

        public string Email { get; set; }

        public EmailAuditStatus? StatusId { get; set; } // Enum? StatusId(1 = review, 2 = sent)

        public int? EmailTemplateId { get; set; }

        public string FromEmail { get; set; }

        public string FromEmailDisplayName { get; set; }

        public string To { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public string Cc { get; set; }

        public string Bcc { get; set; }
    }
}
