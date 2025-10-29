namespace GoRegister.Areas.Admin.Models.ChartViewModels
{
    public class ChartPieViewModel
    {
        public string Header { get; set; }

        public int CutOutPercentage { get; set; }

        public int TotalAccepted { get; set; }

        public int TotalDeclined { get; set; }

        public int TotalCancelled { get; set; }

    }
}
