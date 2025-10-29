using GoRegister.ApplicationCore.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace GoRegister.ApplicationCore.Domain.Registration.Models
{
    public class EmailTemplateModel
    {
        public int Id { get; set; }

        public bool IsActivated { get; set; }

        public bool IsEnabled { get; set; }

        [Display(Name = "Status")]
        public string RegistrationStatus { get; set; }

        public int RegistrationTypeId { get; set; }

        [Display(Name = "Registration Type")]
        public string RegistrationType { get; set; }

        public EmailType EmailType { get; set; }

        [Display(Name = "My Email Address")]
        public string SendToMeEmailAddress { get; set; }

        [EmailAddress]
        public string PreviewEmailAddress { get; set; }

        [EmailAddress]
        public string AdditionalEmailAddress { get; set; }

        public bool PreviewSuccess { get; set; }

        public string PreviewMessage { get; set; }

        public string Subject { get; set; }

        public string Cc { get; set; }

        public string Bcc { get; set; }

        public string Body { get; set; }
    }
}
