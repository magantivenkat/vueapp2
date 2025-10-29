using GoRegister.ApplicationCore.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoRegister.ApplicationCore.Data.Models
{
    public class TPNCountryAdminMapping
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string ClientUuid { get; set; }
        public string TPNCountry { get; set; }
        public int AdminUserId { get; set; }
        public string GAMEmail { get; set; }
        public ReportFrequency ReportFrequency { get; set; }
        public DateTime DateModified { get; set; }
        public int ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public Client Client { get; set; }
        public ApplicationUser ModifiedByUser { get; set; }
    }
}
