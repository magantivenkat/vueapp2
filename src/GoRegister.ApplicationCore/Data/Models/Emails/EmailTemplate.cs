using System.Collections.Generic;

namespace GoRegister.ApplicationCore.Data.Models
{
    public class EmailTemplate : MustHaveProjectEntityForEmail
    {
        public int Id { get; set; }
        public string BodyHtml { get; set; }
        public bool HasTextBody { get; set; }
        public string BodyText { get; set; }
        public int EmailId { get; set; }
        public bool IsDefault { get; set; }

        public Email Email { get; set; }
        public ICollection<EmailTemplateRegistrationType> RegistrationTypes { get; set; }
    }
}
