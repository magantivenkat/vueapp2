using GoRegister.ApplicationCore.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoRegister.ApplicationCore.Data.Models
{
    public class TPNReportRequest
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string ClientUuid { get; set; }
        public string TPNCountry { get; set; }
        public DateTime RequestedDate { get; set; }
        public int RequestedBy { get; set; }
        public string DownloadPath { get; set; }
        public string ReportType { get; set; }
        public ReportStatus ReportStatusId { get; set; }
        public Client Client { get; set; }      
        public string GAMEmail { get; set; }
        public ReportFrequency ReportFrequency { get; set; }
        public string FileName { get; set; }

    }
}
