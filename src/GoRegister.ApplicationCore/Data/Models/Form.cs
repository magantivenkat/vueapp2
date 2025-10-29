using GoRegister.ApplicationCore.Data.Enums;
using System.Collections.Generic;

namespace GoRegister.ApplicationCore.Data.Models
{
    public class Form : MustHaveProjectEntity
    {
        public int Id { get; set; }
        public FormType FormTypeId { get; set; }

        public string AdminDisplayName { get; set; }

        public virtual ICollection<RegistrationPage> RegistrationPages { get; set; } = new HashSet<RegistrationPage>();
        public bool IsReviewPageHidden { get; set; }
        public string SubmitButtonText { get; set; }
    }
}
