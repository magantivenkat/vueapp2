using System.ComponentModel.DataAnnotations;

namespace GoRegister.Areas.Admin.Models
{
    public class Email
    {
        [Required]
        [EmailAddress]
        public string From { get; set; }

        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string To { get; set; }

        [EmailAddress]
        public string Cc { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }
    }
}
