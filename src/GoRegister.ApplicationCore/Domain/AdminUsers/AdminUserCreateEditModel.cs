using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GoRegister.ApplicationCore.Domain.AdminUsers
{
    public class AdminUserCreateEditModel : AccountSettingsModel
    {        
        [Required(ErrorMessage ="Please Enter First Name")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please Enter Last Name")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        
        [Required(ErrorMessage = "Please Enter Email")]
        [DataType(DataType.EmailAddress,ErrorMessage = "Please Enter Valid Email")]
        public string Email { get; set; }        
        public AdminUserRoleModel[] Roles { get; set; }

        [Display(Name ="Roles")]
        public string[] Roles1 { get; set; }
        public bool IsInvited { get; set; }

        public IEnumerable<SelectListItem> SelectRoles { get; set; }

    }
}
