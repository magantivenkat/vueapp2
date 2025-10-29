using GoRegister.ApplicationCore.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GoRegister.ApplicationCore.Domain.Registration.Models
{
    public class RegistrationTypeModel
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }

        public int RegistrationPathId { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int? Capacity { get; set; }

        [Required]
        public int RegPathId { get; set; }
        public IEnumerable<SelectListItem> RegistrationPaths { get; set; }

        [Display(Name = "Delegates")]
        public ICollection<DelegateUser> DelegateUsers { get; set; } = new List<DelegateUser>();

    }
}
