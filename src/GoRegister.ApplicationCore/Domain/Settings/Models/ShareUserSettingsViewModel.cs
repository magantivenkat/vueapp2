using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GoRegister.ApplicationCore.Domain.Settings.Models
{
    public class ShareUserSettingsViewModel
    {
        [Required(ErrorMessage = "Please Select User")]
        [Display(Name = "Share User")]
        public int UserId { get; set; }

        public int ProjectId { get; set; }

        public List<ShareUserSettingsModel> Users { set; get; }

        public List<ShareUserSettingsModel> MappedUsers { set; get; }
    }
}
