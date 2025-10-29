using GoRegister.ApplicationCore.Data.Enums;

namespace GoRegister.ApplicationCore.Domain.Registration.Models
{
    public class EmailTemplateCreateModel
    {
        public int Id { get; set; }

        public bool IsEnabled { get; set; }

        public int RegistrationTypeId { get; set; }

        public int RegistrationStatusId { get; set; }

        public EmailType EmailType { get; set; }

        public string Subject { get; set; }

        public string Cc { get; set; }

        public string Bcc { get; set; }

        public string Body { get; set; }
    }
}
