using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Data.Models.Emails;
using System.Collections.Generic;

namespace GoRegister.ApplicationCore.Data.Models
{
    public class Email : MustHaveProjectEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public EmailType EmailType { get; set; }
        public bool IsEnabled { get; set; }
        public string Subject { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }

        public ICollection<EmailTemplate> EmailTemplates { get; set; } = new HashSet<EmailTemplate>();

        public int? EmailLayoutId { get; set; }
        public EmailLayout EmailLayout { get; set; }
    }
}
