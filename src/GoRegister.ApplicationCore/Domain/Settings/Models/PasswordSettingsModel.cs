using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Settings.Models
{
    public class PasswordSettingsModel
    {
        [Display(Name="Set Sitewide Password")]
        public string SitewidePasswordPlainText { get; set; }
        public string SitewidePasswordHashed { get; set; }

        [Display(Name = "Allow anonymous access")]
        public bool AllowAnonymousAccess { get; set; }

    }
}
