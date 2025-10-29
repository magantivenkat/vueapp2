using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GoRegister.ApplicationCore.Domain.Registration.Models
{
    public class EmailTemplateDelegateModel
    {
        public int Id { get; set; }

        //public int ProjectId { get; set; }

        [Display(Name = "Status")]
        public string RegistrationStatus { get; set; }

        [Display(Name = "Registration Type")]
        public string RegistrationType { get; set; }

        public int RegistrationTypeId { get; set; }

        public int RegistrationStatusId { get; set; }

        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Display(Name = "Email Display From")]
        public string EmailDisplayFrom { get; set; }

        [Display(Name = "Email Reply To")]
        public string EmailReplyTo { get; set; }

        public IEnumerable<DelegateListItemModel> DelegateUsers { get; set; }

        [Display(Name = "Number Of Delegates")]
        public int NumberOfDelegates { get; set; }
    }
}
