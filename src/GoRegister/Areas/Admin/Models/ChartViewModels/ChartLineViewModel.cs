using System.Collections.Generic;

namespace GoRegister.Areas.Admin.Models.ChartViewModels
{
    public class ChartLineViewModel
    {
        public string Header { get; set; }

        public List<string> Labels { get; set; } = new List<string>();

        public string DataSetsLabel { get; set; }

        public List<int> ChartData { get; set; } = new List<int>();

    }
}
