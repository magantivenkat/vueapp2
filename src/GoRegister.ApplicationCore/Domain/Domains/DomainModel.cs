using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GoRegister.ApplicationCore.Domain.Domains
{
    public class DomainModel
    {
        public int Id { get; set; }

        [Required]
        [RegularExpression("^(?!-)[A-Za-z0-9-]+([\\-\\.]{1}[a-z0-9]+)*\\.[A-Za-z]{2,6}$",
            ErrorMessage = "Please enter a valid domain name")]
        public string Host { get; set; }

        [DisplayName("How will the project URL be displayed?")]
        public bool IsSubdomainHost { get; set; }

        public int? ClientId { get; set; }
        public List<SelectListItem> ClientList { get; set; }
    }
}
