using System;
using System.Collections.Generic;
using System.Text;

namespace GoRegister.ApplicationCore.Domain.TPNReports.Models
{
    public class TPNReportRequestModel
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string TPNCountry { get; set; }
        public string DateRequested { get; set; }
        public string RequestBy { get; set; }
        public string DownloadPath { get; set; }
        public string ReportType { get; set; }
        public string ReportStatus { get; set; }


        public string ClientUuid { get; set; }
    }
}
