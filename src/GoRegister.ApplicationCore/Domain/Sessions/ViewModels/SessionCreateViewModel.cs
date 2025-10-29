using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GoRegister.ApplicationCore.Domain.Sessions.ViewModels
{
    public class SessionCreateEditViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime DateCreatedUtc { get; set; }

        [DisplayName("Session start time")]
        public DateTime DateStartUtc { get; set; }
        [DisplayName("Session end time")]
        public DateTime DateEndUtc { get; set; }
        [DisplayName("Registration close time")]
        public DateTime DateCloseRegistrationUtc { get; set; }

        public int Capacity { get; set; }
        [DisplayName("Reserved Capacity")]
        public int CapacityReserved { get; set; }

        [DisplayName("Meeting Link")]
        public string MeetingLink { get; set; }
        [DisplayName("Meeting Password")]
        public string MeetingPassword { get; set; }

        [DisplayName("Open for registration")]
        public bool OpenForRegistration { get; set; }

        [DisplayName("Is session optional")]
        public bool IsOptional { get; set; }

        [DisplayName("Session part of a category")]
        public int? SessionCategoryId { get; set; }
        public IEnumerable<SelectListItem> SessionCategories { get; set; }

        [DisplayName("Limit which registration types can attend")]
        public List<int> RegTypeIds { get; set; }
        public IEnumerable<SelectListItem> RegTypes { get; set; }

        public int RegistrationCount { get; set; }

        public string Notes { get; set; }


        public int ProjectId { get; set; }
    }

}
