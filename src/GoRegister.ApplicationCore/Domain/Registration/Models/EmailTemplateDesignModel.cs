using System.ComponentModel.DataAnnotations;

namespace GoRegister.ApplicationCore.Domain.Registration.Models
{
    public class EmailTemplateDesignModel
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }

        public int RegistrationStatusId { get; set; }

        public int RegistrationTypeId { get; set; }

        [Display(Name = "Registration Type")]
        public string RegistrationType { get; set; }

        public string EmailType { get; set; }

        public bool IsEnabled { get; set; }

        [Required]
        public string Subject { get; set; }

        [EmailAddress]
        public string Cc { get; set; }

        [EmailAddress]
        public string Bcc { get; set; }

        public string Body { get; set; }

    }
}
