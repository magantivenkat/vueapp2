using System;
using System.Collections.Generic;
using System.Text;

namespace GoRegister.ApplicationCore.Data.Models
{
    public class TPNReportDataStatus
    {
        public int Id { get; set; }
        public int TPNReportDetailsId { get; set; }
        public int MRFClientResponseDetailsId { get; set; }
        public bool IsSendWeekly { get; set; }
        public bool IsSendFortNightly { get; set; }
        public bool IsSendMonthly { get; set; }
    }
}
