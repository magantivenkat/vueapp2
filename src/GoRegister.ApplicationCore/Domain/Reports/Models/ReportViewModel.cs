using GoRegister.ApplicationCore.Domain.Reports.Framework;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GoRegister.ApplicationCore.Domain.Reports.Models
{
    public class ReportViewModel : ReportDataViewModel
    {
        //public List<string> SelectedGroupBys { get; set; } = new List<string>();

        public List<SelectListItem> ReportFields { get; set; }
        public List<IReportFilterViewModel> ReportFilters { get; set; }
        public List<GroupByViewModel> GroupBys { get; set; }
    }
}
