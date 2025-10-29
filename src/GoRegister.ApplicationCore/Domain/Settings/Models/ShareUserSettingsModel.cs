using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GoRegister.ApplicationCore.Domain.Settings.Models
{
    public class ShareUserSettingsModel
    {
        [Display(Name = "Share User")]
        public string[] ShareUser { get; set; }
        public IEnumerable<SelectListItem> SelectShareUser { get; set; }

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
    }
}
