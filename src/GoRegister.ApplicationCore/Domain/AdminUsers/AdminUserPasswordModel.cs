using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GoRegister.ApplicationCore.Domain.AdminUsers
{
    public class AdminUserPasswordModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        [Required(ErrorMessage = "Please Enter Password")]
        [DataType(DataType.Password)]
        [StringLength(int.MaxValue, ErrorMessage = "Password must be at least 8 characters, with 1 lowercase, 1 uppercase, 1 digit and 1 special character", MinimumLength = 8)]
        public string Password { get; set; }    

        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and Confirm password should match")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
