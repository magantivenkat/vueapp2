using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GoRegister.ApplicationCore.Domain.AdminUsers
{
    public class AccountSettingsModel
    {

        public int Id { get; set; }

        public List<SelectListItem> TimeZones { get; set; } = new List<SelectListItem>();

        [Display(Name = "Time Zone")]
        public string TimeZone { get; set; }

        public List<SelectListItem> DateFormats { get; set; } = new List<SelectListItem>();

        [Display(Name = "Date Format")]
        public string DateFormat { get; set; }
    }
}
