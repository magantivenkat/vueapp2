using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace GoRegister.ApplicationCore.Domain.Registration.Models
{
    public class DelegateCreateModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public bool HasMultipleRegistrationTypes { get; set; }

        [Required]
        [Display(Name = "Registration Type")]
        public int RegistrationTypeId { get; set; }

        public SelectList RegistrationTypeSelectList { get; set; }

        [Display(Name = "For Testing")]
        public bool IsTest { get; set; }

    }
}
