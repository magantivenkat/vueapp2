using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GoRegister.ApplicationCore.Domain.Registration.Models
{
    public class EmailTemplateListModel
    {
        public bool HasMultipleRegistrationTypes { get; set; }

        [Display(Name = "Registration Type")]
        public int RegistrationTypeId { get; set; }

        public SelectList RegistrationTypeSelectList { get; set; }

        public Dictionary<string, bool> EmailTypes { get; set; } = new Dictionary<string, bool>();

        public IEnumerable<EmailTemplateModel> EmailTemplateList { get; set; } = new List<EmailTemplateModel>();


    }
}
