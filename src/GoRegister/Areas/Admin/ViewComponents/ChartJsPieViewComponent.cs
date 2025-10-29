using GoRegister.Areas.Admin.Models.ChartViewModels;
using GoRegister.ApplicationCore.Domain.Registration.Models;
using GoRegister.ApplicationCore.Domain.Registration.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.Areas.Admin.ViewComponents
{
    public class ChartJsPieViewComponent : ViewComponent
    {
        private readonly IDelegateService _delegateService;

        public ChartJsPieViewComponent(IDelegateService delegateService)
        {
            _delegateService = delegateService;
        }

        public int Columns { get; set; }

        public async Task<IViewComponentResult> InvokeAsync(string header, int cutOutPercentage)
        {
            var delegates = await _delegateService.GetList();

            var delegateListItemModels = delegates.Where(d => !d.IsTest).ToList();

            var chartViewModel = PopulateViewModel(header, cutOutPercentage, delegateListItemModels);

            return View(chartViewModel);
        }

        private static ChartPieViewModel PopulateViewModel(string header, int cutOutPercentage, IReadOnlyCollection<DelegateListItemModel> delegateListItemModels)
        {
            return new ChartPieViewModel
            {
                Header = header,
                CutOutPercentage = cutOutPercentage,
                TotalAccepted = delegateListItemModels.Count(d => d.ConfirmedUtc > d.InvitedUtc),
                TotalDeclined = delegateListItemModels.Count(d => d.DeclinedUtc > d.InvitedUtc),
                TotalCancelled = delegateListItemModels.Count(d => d.CancelledUtc > d.InvitedUtc)
            };
        }
    }
}
