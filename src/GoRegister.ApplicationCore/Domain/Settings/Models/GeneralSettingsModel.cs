using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Domain.Projects.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GoRegister.ApplicationCore.Domain.Settings.Models
{
    public class GeneralSettingsModel
    {
        public int Id { get; set; }

        [Required()]
        [DisplayName("Project name")]
        public string Name { get; set; }

        public string Code { get; set; }

        public List<SelectListItem> TimeZones { get; set; } = new List<SelectListItem>();
        public string Timezone { get; set; }
        [Required()]
        [DisplayName("Project start date")]
        public DateTime StartDate { get; set; }

        [Required()]
        [DisplayName("Project end date")]
        public DateTime EndDate { get; set; }

        [Required()]
        [DisplayName("Project archive date")]
        public DateTime ArchiveDate { get; set; }

        [DisplayName("Delete data date")]
        public DateTime DeleteDataDate { get; set; }

        [DisplayName("Project email address")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please Enter Valid Email")]
        public string EmailAddress { get; set; }

        [Required()]
        [DisplayName("Project email from name")]
        public string EmailDisplayFrom { get; set; }

        [DisplayName("Email reply to")]
        public string EmailReplyTo { get; set; }

        [DisplayName("Project telephone number")]
        public string TelephoneNumber { get; set; }

        [DisplayName("Venue name")]
        public string VenueName { get; set; }

        [DisplayName("Venue address")]
        public string VenueAddress { get; set; }

        [DisplayName("Venue website")]
        public string VenueWebsite { get; set; }

        [DisplayName("Venue telephone number")]
        public string VenueTelephoneNumber { get; set; }

        [DisplayName("Project Status")]
        public ProjectStatus StatusId { get; set; }

        public ProjectTypeEnum ProjectType { get; set; }

        public int DaysToEvent => DateTime.UtcNow > StartDate ? 0 : (int)(StartDate - DateTime.Now).TotalDays;

        public string LongDateFormat { get; set; }
        public string ShortDateFormat { get; set; }
        public string TimeFormat { get; set; }
        
        [DisplayName("Allow duplicate emails")] 
        public bool AllowDuplicateEmails { get; set; }

        public string EmailType { get; set; }

        [DisplayName("Custom email address")]
        [RegularExpression(@"^[a-zA-Z0-9,-.]+$", ErrorMessage = "Only alphanumeric characters, dash and dot are allowed.")]
        public string CustomEmailAddress { get; set; }
        public int ClientId { get; set; }

        public string SelectedEmail { get; set; }

        [DisplayName("Show in search engines")]
        public bool BlockSearchEngineIndexing { get; set; }

        [Display(Name = "Share User")]
        public string[] ShareUser { get; set; }

        public IEnumerable<SelectListItem> SelectShareUser { get; set; }

    }
}
