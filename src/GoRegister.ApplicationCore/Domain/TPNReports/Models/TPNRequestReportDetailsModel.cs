using System;
using System.Collections.Generic;
using System.Text;

namespace GoRegister.ApplicationCore.Domain.TPNReports.Models
{
    public class TPNRequestReportDetailsModel
    {
        public int TPNReportDetailsId { get; set; }
        public int RequestId { get; set; }
        public int? MRFResponseDetailsId { get; set; }
        public string MRFFormName { get; set; }
        public int? ClientId { get; set; }
        public string ClientUuid { get; set; }
        public string GBTClient { get; set; }
        public string ClientName { get; set; }
        public string TPNCountry { get; set; }
         public string TPNSharedMailbox { get; set; }
        public string ContactFirstName { get; set; }
        public string ContactLastName { get; set; }
        public string ContactEmail { get; set; }
        public DateTime DepartureDate { get; set; }
        public string Destination { get; set; }
        public string EventName { get; set; }
        public DateTime FormDateTimeCreated { get; set; }
        public int? AdminUserId { get; set; }
        public string AdminUserName { get; set; }
        public string GAManagerEmail { get; set; }
        public bool IsSendWeekly { get; set; }
        public bool IsSendFortNightly { get; set; }
        public bool IsSendMonthly { get; set; }

    }
}
