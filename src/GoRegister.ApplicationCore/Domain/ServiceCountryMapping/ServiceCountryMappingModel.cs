using GoRegister.ApplicationCore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace GoRegister.ApplicationCore.Domain.ServiceCountryMapping
{
    public class ServiceCountryMappingModel
    {
        public string? ClientUuid { get; set; }
        public string? ServiceCountry { get; set; }
        [Required]
        public string? ServiceCountryUuid { get; set; }
        public string? RequestCountry { get; set; }
        [Required]
        public string? RequestCountryId { get; set; }
        public DateTime? DateCreated { get; set; }
        public int? CreatedBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsDefault { get; set; }
        public int ProjectId { get; set; }
        public string Smessage { get; set; }
        public string Rmessage { get; set; }
        public string AddEdit { get; set; }
       
    }
}
