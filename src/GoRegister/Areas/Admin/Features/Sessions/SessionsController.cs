using GoRegister.Areas.Admin.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GoRegister.ApplicationCore.Domain.Sessions.ViewModels;
using GoRegister.ApplicationCore.Domain.Sessions.Services;
using System;
using GoRegister.ApplicationCore.Domain.Registration.Services;
using System.Linq;
using GoRegister.ApplicationCore.Framework.DataTables;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using GoRegister.Areas.Admin.Extensions;
using GoRegister.ApplicationCore.Domain.Settings.Services;

namespace GoRegister.Areas.Admin.Features.Sessions
{
    public class SessionsController : ProjectAdminControllerBase
    {
        private readonly ISessionService _sessionService;
        private readonly ISessionBookingService _sessionBookingService;
        private readonly IRegistrationTypeService _registrationTypeService;
        private readonly IProjectSettingsAccessor _projectSettingsAccessor;

        public SessionsController(ISessionService sessionService, ISessionBookingService sessionBookingService, IRegistrationTypeService registrationTypeService, IProjectSettingsAccessor projectSettingsAccessor)
        {
            _sessionService = sessionService;
            _sessionBookingService = sessionBookingService;
            _registrationTypeService = registrationTypeService;
            _projectSettingsAccessor = projectSettingsAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Delegates(int id)
        {
            SessionCreateEditViewModel model = await _sessionService.GetAsync(id);
            return View("Delegates", model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var project = await _projectSettingsAccessor.GetAsync();

            var model = new SessionCreateEditViewModel 
            {
                DateStartUtc = project.StartDate,
                DateEndUtc = project.StartDate,
                DateCloseRegistrationUtc = project.StartDate.AddDays(7),
                SessionCategories = _sessionService.GetCategorySelectList(),
                RegTypes = _registrationTypeService.GetRegTypeSelectList(),
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SessionCreateEditViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            SessionModel result = await _sessionService.CreateAsync(model);

            return RedirectToAction("Index");

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            SessionCreateEditViewModel model = await _sessionService.GetAsync(id);
            model.SessionCategories = _sessionService.GetCategorySelectList();
            model.RegTypes = _registrationTypeService.GetRegTypeSelectList();

            return View("Create", model);
        }

        [HttpPost]
        public async Task<IActionResult> Categories(SessionCategoryModel model)
        {
            SessionCategoryModel result = await _sessionService.CreateCategoryAsync(model);

            return new JsonResult(result);
        }

        [HttpPost]
        public IActionResult Reserve([FromBody] DelegateSessionBookingModel model)
        {
            var reservedSession = _sessionBookingService.ReserveSessionForDelegate(model.SessionId, model.DelegateId, true);
            return new JsonResult(reservedSession);
        }

        [HttpPost]
        public IActionResult LoadData([FromBody] DataTables.DtParameters dtParameters)
        {
            var sessions = _sessionService.GetSessions();
            var totalResultsCount = sessions.Count(); // get the amount of unfiltered items  

            sessions = FilterSessionTable(dtParameters, sessions);
            sessions = OrderSessionTable(dtParameters, sessions);

            var result = Json(new
            {
                draw = dtParameters.Draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = sessions.Count(), // get the amount of filtered items  
                data = sessions
                    .Skip(dtParameters.Start)
                    .Take(dtParameters.Length)
            });

            return result;

        }

        private List<SessionModel> OrderSessionTable(DataTables.DtParameters dtParameters, List<SessionModel> sessions)
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

            sessions = orderAscendingDirection
                ? sessions.AsQueryable().OrderByDynamic(orderCriteria, DataTableOrderExtensions.Order.Asc).ToList()
                : sessions.AsQueryable().OrderByDynamic(orderCriteria, DataTableOrderExtensions.Order.Desc).ToList();
            return sessions;
        }

        private List<SessionModel> FilterSessionTable(DataTables.DtParameters dtParameters, List<SessionModel> sessions)
        {
            var searchBy = dtParameters.Search?.Value;
            if (!string.IsNullOrEmpty(searchBy))
            {
                sessions = sessions
                    .Where(s => s.Name?.ToUpper().Contains(searchBy.ToUpper()) == true
                                || s.Description?.ToUpper().Contains(searchBy.ToUpper()) == true
                                //|| s..RegistrationType?.ToUpper().Contains(searchBy.ToUpper()) == true
                                //|| s.RegistrationStatus?.ToUpper().Contains(searchBy.ToUpper()) == true
                                ).ToList();
            }

            return sessions;
        }
    }
}
