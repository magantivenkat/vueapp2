using System;
using System.Collections.Generic;
using System.Text;

namespace GoRegister.ApplicationCore.Domain.TPNReports.Models
{
    public class TPNReportExcelModel
    {
       
        public string RequestDate { get; set; }
        public string TPNServicingCountry { get; set; }
        public string TPNSharedMailbox { get; set; }
        public string GBTClient { get; set; }
        public string CompanyName { get; set; }
        public string ClientContactFirstName { get; set; }
        public string ClientContactLastName { get; set; }
        public string ClientContactEmail { get; set; }
        public string DepartureDate { get; set; }
        public string Destination { get; set; }
        public string EventName { get; set; } 

    }
}
