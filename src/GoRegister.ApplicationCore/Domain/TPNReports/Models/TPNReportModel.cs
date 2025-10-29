using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GoRegister.ApplicationCore.Domain.TPNReports.Models
{
    public class TPNReportModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please select client")]
        public int ClientId { get; set; }
        public string ClientUuid { get; set; }
        public string ClientName { get; set; }
        [Required(ErrorMessage = "Please select country")]
        public string TPNCountry { get; set; }
        public DateTime RequestedDate { get; set; }
        public int RequestedBy { get; set; }
        public int ReportStatus { get; set; }
        public string ReportType { get; set; }
        public string DownloadPath { get; set; }
        public ReportStatus ReportStatusId { get; set; }
        public List<SelectListItem> TPNCountryList { get; set; }
        public List<SelectListItem> ClientList { get; set; }
        public List<TPNReportRequestModel> TPNReportRequests { get; set; }


    }
}
