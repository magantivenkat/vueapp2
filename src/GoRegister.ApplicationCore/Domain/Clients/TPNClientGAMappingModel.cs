using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GoRegister.ApplicationCore.Domain.Clients
{
    public class TPNClientGAMappingModel
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string ClientUuid { get; set; }
        [Required(ErrorMessage = "Please select country")]
        public string TPNCountry { get; set; }
        [Required(ErrorMessage = "Please select admin")]
        public int AdminUserId { get; set; }
        [Required(ErrorMessage = "Please Enter Global Account Manager Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please Enter Valid Email")]
        public string GAMEmail { get; set; }
        [Required(ErrorMessage = "Please select report frequency")]
        public ReportFrequency ReportFrequency { get; set; }
        public DateTime DateModified { get; set; }
        public int ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public ClientModel Client { get; set; }
        public ApplicationUser ModifiedByUser { get; set; }
        public string FormAction { get; set; }
        public List<SelectListItem> TPNCountryList { get; set; }
        public List<SelectListItem> AdminUserList { get; set; }
        public List<SelectListItem> ReportFrequencies { get; set; }
        public string ClientName { get; set; }
        public string AdminName { get; set; }

    }
}
