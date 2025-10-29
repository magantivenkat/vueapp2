using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GoRegister.ApplicationCore.Domain.CustomPages.Models
{
    public class CustomPageCreateEditModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [RegularExpression(@"^[a-z](-?[a-z])*$",
         ErrorMessage = "Only lowercase characters and dashes are allowed.")]
        public string Slug { get; set; }
        
        public string Content { get; set; }

        public bool HasMultipleRegistrationTypes { get; set; }

        [Required]
        [Display(Name = "Registration Type")]
        public int[] RegistrationTypeId { get; set; }

        public SelectList RegistrationTypeSelectList { get; set; }

        [Display(Name = "Registration Status")]
        public int[] RegistrationStatusId { get; set; }

        public SelectList RegistrationStatusSelectList { get; set; }

        public List<CustomPageRegistrationType> CustomPageRegistrationTypes { get; set; } = new List<CustomPageRegistrationType>();

        public List<CustomPageRegistrationStatus> CustomPageRegistrationStatuses { get; set; } = new List<CustomPageRegistrationStatus>();

        [Display(Name = "Page Is Visible")]
        public bool IsVisible { get; set; }

        public int Position { get; set; }

        public int ProjectPageId { get; set; }
        public PageType PageType { get; set; }

    }
}
