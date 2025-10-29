/*  MRF Changes : Get prefix on client dropdown change
    Modified Date : 01st November 2022
    Modified By : Mandar.Khade @amexgbt.com
    Team member : Harish.Rame @amexgbt.com
    JIRA Ticket No : GoRegister/GOR-228   */


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using GoRegister.ApplicationCore.Domain.Clients;
using GoRegister.ApplicationCore.Framework.DataTables;
using GoRegister.ApplicationCore.Data;
using Microsoft.EntityFrameworkCore;
using GoRegister.Framework.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using GoRegister.ApplicationCore.Domain.Countries;
using GoRegister.ApplicationCore.Framework.Identity;
using System.Security.Claims;
using GoRegister.ApplicationCore.Framework.Notifications;
using GoRegister.ApplicationCore.Domain.AdminUsers;
using System.Collections.Generic;
using GoRegister.ApplicationCore.Data.Enums;
using System;

namespace GoRegister.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Client/[action]")]
    [Route("Client/[action]/id")]
    [Authorize(Policies.ManageClients)]
    public class ClientController : AdminControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly IClientService _clientService;
        private readonly ICountryCacheService _countryCacheService;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IAdminUserService _adminUserService;

        public ClientController(ApplicationDbContext context, IClientService clientService, ICountryCacheService countryCacheService, ICurrentUserAccessor currentUserAccessor, IAdminUserService adminUserService)
        {
            _context = context;
            _clientService = clientService;
            _countryCacheService = countryCacheService;
            _currentUserAccessor = currentUserAccessor;
            _adminUserService = adminUserService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //var clients = await _clientService.GetList();


            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View("CreateEditClient", new ClientModel());
        }

        [HttpGet]
        public async Task<IActionResult> TPNIndex()
        {
            return View("TPNIndex");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ClientModel model = await _clientService.GetById(id);
            model.FormAction = "Edit";

            return View("CreateEditClient", model);
        }



        [HttpPost]
        public async Task<IActionResult> Save(ClientModel model)
        {

            if (model.FormAction != null && model.FormAction.ToLower() == "edit")
            {
                this.ModelState.Remove("Email");
            }

            if (ModelState.IsValid)
            {
                ClientModel client = await _clientService.Save(model);

                if (model.FormAction.ToLower() != "edit")
                {
                    if (client == null)
                    {
                        ViewBag.ClientExist = "Client already exists. Please try creating different client.";
                        return View("CreateEditClient", model);
                    }
                    else
                    {
                        return RedirectToAction("Edit", new { id = client.Id });
                    }
                }
                else if (model.FormAction.ToLower() == "edit")
                {
                    if (client == null)
                    {
                        ViewBag.ClientExist = "Client already exists. Please try updating to different client.";
                        return View("CreateEditClient", model);
                    }
                    else
                    {
                        return RedirectToAction("Edit", new { id = model.Id });
                    }
                }
            }

            return View("CreateEditClient", model);
        }

        [HttpPost]
        public async Task<IActionResult> Email(CreateEditClientEmailModel model)
        {
            if (ModelState.IsValid)
            {
                var email = await _clientService.SaveEmailAsync(model);
                return new JsonResult(email);
            }
            return View("CreateEditClient", model);
        }

        [HttpDelete]
        public async Task<IActionResult> Email(int emailId)
        {
            bool deleted = await _clientService.DeleteEmailAsync(emailId);
            return new JsonResult(deleted);
        }

        public async Task<IActionResult> LoadData([FromBody] DataTables.DtParameters dtParameters)
        {
            var clients = await _clientService.GetList();
            var totalResultsCount = clients.Count(); // get the amount of unfiltered items  

            clients = _clientService.FilterClientTable(dtParameters, clients);
            clients = _clientService.OrderClientTable(dtParameters, clients);

            var result = Json(new
            {
                draw = dtParameters.Draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = clients.Count(), // get the amount of filtered items  
                data = clients
                    .Skip(dtParameters.Start)
                    .Take(dtParameters.Length)
            });

            return result;
        }

        [HttpGet]
        public async Task<IActionResult> ClientEmails(int clientId)
        {
            var client = await _context.Clients.Include(c => c.ClientEmails).SingleOrDefaultAsync(c => c.Id == clientId);
            return new JsonResult(client.ClientEmails);
        }

        [HttpGet]
        public async Task<IActionResult> ClientDetails(int clientId)
        {
            var client = await _context.Clients.SingleOrDefaultAsync(c => c.Id == clientId);
            return new JsonResult(client.ClientUuid);
        }

        [HttpGet]
        public async Task<IActionResult> TPNView(int id)
        {
            ClientModel model = await _clientService.GetById(id);
            model.TPNCountryClientEmails = await _clientService.GetTPNEmailsById(id);
            model.TPNClientGAMappings = await _clientService.GetTPNClientGAMappingsById(id);

            model.FormAction = "Edit";


            return View("CreateTPNClient", model);
        }

        public async Task<IActionResult> TPNEmailCreate(int clientId)
        {
            var client = await _context.Clients.SingleOrDefaultAsync(c => c.Id == clientId);

            TPNCountryClientEmailModel model = new TPNCountryClientEmailModel();
            model.Id = 0;
            model.ClientId = clientId;
            model.ClientUuid = client.ClientUuid;
            model.TPNCountryList = await _countryCacheService.GetCountryDropdownListMRF();
            model.FormAction = "Add";

            return View("CreateEditTPNClient", model);
        }

        public async Task<IActionResult> TPNEmailEdit(int emailId)
        {
            TPNCountryClientEmailModel model = await _clientService.GetTPNEmailDetails(emailId);
            model.TPNCountryList = await _countryCacheService.GetCountryDropdownListMRF();
            model.FormAction = "Edit";

            return View("CreateEditTPNClient", model);
        }

        [HttpPost]
        public async Task<IActionResult> TPNCountryEmail(TPNCountryClientEmailModel model)
        {
            if (model.FormAction != null && model.FormAction.ToLower() == "edit")
            {
                this.ModelState.Remove("Email");
            }

            if (ModelState.IsValid)
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                model.ModifiedBy = userId;
                var email = await _clientService.SaveTPNEmailAsync(model);
                ModelState.Clear();
                return new JsonResult(email);
            }
            return View("CreateEditTPNClient", model);
        }

        [HttpDelete]
        public async Task<IActionResult> TPNCountryEmailDel(int emailId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            bool deleted = await _clientService.DeleteTPNEmailAsync(emailId, userId);
            return new JsonResult(deleted);
        }

        [HttpPost]
        public async Task<IActionResult> TPNCountryEmailUpdt(TPNCountryClientEmailModel model)
        {
            ModelState.Clear();
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            model.ModifiedBy = userId;
            var email = await _clientService.SaveTPNEmailAsync(model);
            return new JsonResult(email);

        }

        public async Task<IActionResult> TPNCountryMapGA(int clientId)
        {
            var client = await _context.Clients.SingleOrDefaultAsync(c => c.Id == clientId);

            TPNClientGAMappingModel model = new TPNClientGAMappingModel();
            model.Id = 0;
            model.ClientId = clientId;
            model.ClientUuid = client.ClientUuid;
            model.TPNCountryList = await _countryCacheService.GetCountryDropdownListMRF();
            model.FormAction = "Add";

            var userList = await _adminUserService.GetList();

            var adminUserList = new List<SelectListItem>();

            adminUserList = (from u in userList
                             join ur in _context.UserRoles on u.Id equals ur.UserId
                             join r in _context.Roles on ur.RoleId equals r.Id
                             where r.Name.ToLower() == "administrator" && u.FirstName != null
                             orderby u.Name ascending
                             select new SelectListItem()
                             {
                                 Value = u.Id.ToString(),
                                 Text = u.FirstName + " " + u.LastName
                             }).ToList();

            model.AdminUserList = adminUserList;

            var reportFrequenciesList = Enum.GetValues(typeof(ReportFrequency))
               .Cast<ReportFrequency>()
               .Select(t => new SelectListItem
               {
                   Value = ((int)t).ToString(),
                   Text = t.ToString()
               }).ToList();

            model.ReportFrequencies = reportFrequenciesList;

            return View("MapGATPNCountry", model);
        }


        public async Task<IActionResult> SaveTPNClientGAMapping(TPNClientGAMappingModel model)
        {

            if (model.FormAction != null && model.FormAction.ToLower() == "edit")
            {
                this.ModelState.Remove("Email");
            }

            if (ModelState.IsValid)
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                model.ModifiedBy = userId;
                var tpnClientMapping = await _clientService.SaveTPNClientGAMappingAsync(model);
                ModelState.Clear();
                return new JsonResult(tpnClientMapping);
            }

            return View("MapGATPNCountry", model);
        }

        public async Task<IActionResult> TPNClientGAMappingEdit(int mappingId)
        {
            TPNClientGAMappingModel model = await _clientService.GetTPNClientGAMappingDetails(mappingId);
            model.TPNCountryList = await _countryCacheService.GetCountryDropdownListMRF();
            model.FormAction = "Edit";

            var userList = await _adminUserService.GetList();
            var adminUserList = new List<SelectListItem>();

            adminUserList = (from u in userList
                             join ur in _context.UserRoles on u.Id equals ur.UserId
                             join r in _context.Roles on ur.RoleId equals r.Id
                             where r.Name.ToLower() == "administrator" && u.FirstName != null
                             orderby u.Name ascending
                             select new SelectListItem()
                             {
                                 Value = u.Id.ToString(),
                                 Text = u.FirstName + " " + u.LastName
                             }).ToList();

            model.AdminUserList = adminUserList;

            var reportFrequenciesList = Enum.GetValues(typeof(ReportFrequency))
               .Cast<ReportFrequency>()
               .Select(t => new SelectListItem
               {
                   Value = ((int)t).ToString(),
                   Text = t.ToString()
               }).ToList();

            model.ReportFrequencies = reportFrequenciesList;

            return View("MapGATPNCountry", model);
        }

        public async Task<IActionResult> TPNClientGAMappingDelete(int mappingId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            bool deleted = await _clientService.DeleteTPNClientGAMappingAsync(mappingId, userId);
            return new JsonResult(deleted);
        }

    }
}