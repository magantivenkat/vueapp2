using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace GoRegister.ApplicationCore.Domain.Reports.Models
{
    public class ReportDataViewModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public ReportType Type { get; set; } = ReportType.Delegates;
        public List<string> SelectedFields { get; set; } = new List<string>();
        public string OrderByKey { get; set; }
        public ReportOrder OrderByDirection { get; set; }
        public string GroupByStartFrom { get; set; }
        public List<JToken> SelectedFilters { get; set; } = new List<JToken>();
        public List<JToken> SelectedGroupBys { get; set; } = new List<JToken>();
        public bool GroupBySkipRowsWithNoData { get; set; }
        public enum ReportOrder { asc, desc };
        public enum ReportType { Delegates, Summary };
    }
}
