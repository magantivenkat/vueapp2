using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using GoRegister.ApplicationCore.Domain.Clients;
using GoRegister.ApplicationCore.Framework.DataTables;
using GoRegister.ApplicationCore.Domain.Domains;
using System.Collections.Generic;
using GoRegister.Areas.Admin.Extensions;
using GoRegister.Framework.Authorization;

namespace GoRegister.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Domain/[action]")]
    [Authorize(Policies.ManageDomains)]
    public class DomainController : AdminControllerBase
    {
        private readonly IClientService _clientService;
        private readonly IDomainService _domainService;

        public DomainController(IClientService clientService, IDomainService domainService)
        {
            _clientService = clientService;
            _domainService = domainService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new DomainModel();
            model.ClientList = await _clientService.GetClientDropdownList();
            return View("CreateEdit", model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            DomainModel model = await _domainService.GetById(id);
            model.ClientList = await _clientService.GetClientDropdownList();

            return View("CreateEdit", model);
        }

        [HttpPost]
        public async Task<IActionResult> Save(DomainModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ClientList = await _clientService.GetClientDropdownList();
                return View("CreateEdit", model);
            }

            var result = await _domainService.SaveAsync(model);
            return RedirectToAction("Edit", new { id = result.Value.Id });
        }

        [HttpGet]
        public async Task<IActionResult> ListDomains(int clientId)
        {
            var domains = await _domainService.GetDomainsAsync(clientId);
            return new JsonResult(domains);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int domainId)
        {
            await _domainService.DeleteDomainsAsync(domainId);
            return new JsonResult("deleted");
        }

        [HttpPost]
        public async Task<IActionResult> LoadData([FromBody] DataTables.DtParameters dtParameters, int sessionId)
        {
            IEnumerable<DomainModel> domainList = await _domainService.GetDomainsAsync();

            var totalResultsCount = domainList.Count(); // get the amount of total items (without the skip and take) 

            domainList = FilterDelegates(dtParameters, domainList);

            domainList = OrderDelegates(dtParameters, domainList);

            var result = Json(new
            {
                draw = dtParameters.Draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = domainList.Count(), // get the amount of filtered items  
                data = domainList
                    .Skip(dtParameters.Start)
                    .Take(dtParameters.Length)
            });

            return result;
        }

        private static IEnumerable<DomainModel> OrderDelegates(DataTables.DtParameters dtParameters, IEnumerable<DomainModel> delegates)
        {
            string orderCriteria;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "Id";
            }

            delegates = orderAscendingDirection
                ? delegates.AsQueryable().OrderByDynamic(orderCriteria, DataTableOrderExtensions.Order.Asc)
                : delegates.AsQueryable().OrderByDynamic(orderCriteria, DataTableOrderExtensions.Order.Desc);
            return delegates;
        }

        private static IEnumerable<DomainModel> FilterDelegates(DataTables.DtParameters dtParameters, IEnumerable<DomainModel> domainList)
        {
            var searchBy = dtParameters.Search?.Value;
            if (!string.IsNullOrEmpty(searchBy))
            {
                domainList = domainList
                    .Where(d => d.Host?.ToUpper().Contains(searchBy.ToUpper()) == true
                                //|| d.Email?.ToUpper().Contains(searchBy.ToUpper()) == true
                                //|| d.RegistrationType?.ToUpper().Contains(searchBy.ToUpper()) == true
                                //|| d.RegistrationStatus?.ToUpper().Contains(searchBy.ToUpper()) == true
                                );
            }

            return domainList;
        }

    }
}
