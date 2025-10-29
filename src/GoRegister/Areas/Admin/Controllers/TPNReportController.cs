using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Clients;
using GoRegister.ApplicationCore.Domain.Countries;
using GoRegister.ApplicationCore.Domain.TPNReports;
using GoRegister.ApplicationCore.Domain.TPNReports.Models;
using GoRegister.ApplicationCore.Framework;
using GoRegister.ApplicationCore.Framework.DataTables;
using GoRegister.ApplicationCore.Framework.Identity;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GoRegister.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("TPNReport/[action]")]
    [Route("TPNReport/[action]/id")]
    public class TPNReportController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly ITPNReportService _tpnReportServices;
        private readonly IClientService _clientService;
        private readonly ICountryCacheService _countryCacheService;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly ProjectTenant _projectTenant;


        public TPNReportController(ITPNReportService tpnReportServices, IClientService clientService, ICountryCacheService countryCacheService, IBackgroundJobClient backgroundJobClient, ProjectTenant projectTenant, ICurrentUserAccessor currentUserAccessor, ApplicationDbContext context)
        {
            _clientService = clientService;
            _tpnReportServices = tpnReportServices;
            _countryCacheService = countryCacheService;
            _context = context;
            _currentUserAccessor = currentUserAccessor;
            _backgroundJobClient = backgroundJobClient;
            _projectTenant = projectTenant;
        }

        public IActionResult Index()
        {

            return View("Index");
        }

        public async Task<IActionResult> TPNReportCreate()
        {
            var model = new TPNReportModel();

            model.TPNCountryList = await _countryCacheService.GetCountryDropdownListMRF();
            model.ClientList = await _clientService.GetClientDropdownList();

            return View("TPNReportCreate", model);
        }

       
        public async Task<IActionResult> LoadData([FromBody] DataTables.DtParameters dtParameters)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var tpnReportRequests = await _tpnReportServices.GetTPNReportRequestsList();
            var totalResultsCount = tpnReportRequests.Count(); // get the amount of unfiltered items  

            tpnReportRequests = _tpnReportServices.FilterTPNReportRequestTable(dtParameters, tpnReportRequests);
            tpnReportRequests = _tpnReportServices.OrderTPNReportRequestTable(dtParameters, tpnReportRequests);

            var result = Json(new
            {
                draw = dtParameters.Draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = tpnReportRequests.Count(), // get the amount of filtered items  
                data = tpnReportRequests
                    .Skip(dtParameters.Start)
                    .Take(dtParameters.Length)
            });

            return result;
        }

        [HttpPost]
        public async Task<IActionResult> Save(TPNReportModel model)
        {
            if (ModelState.IsValid)
            {
                var client = await _context.Clients.SingleOrDefaultAsync(c => c.Id == model.ClientId);
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                model.ClientUuid = client.ClientUuid;
                model.ClientName = client.Name;
                model.ReportType = "Manual";
                model.ReportStatusId = ReportStatus.New;
                model.RequestedDate = DateTime.Now;
                model.RequestedBy = userId;

                TPNReportRequest tpnReportRequest = await _tpnReportServices.Save(model);

                model = new TPNReportModel();
                model.TPNCountryList = await _countryCacheService.GetCountryDropdownListMRF();
                model.ClientList = await _clientService.GetClientDropdownList();

               _backgroundJobClient.Enqueue<ITPNReportService>(i => i.GenerateManualTPNReport(_projectTenant,tpnReportRequest.Id));

            }

            return View("Index", model);
        }
    }
}