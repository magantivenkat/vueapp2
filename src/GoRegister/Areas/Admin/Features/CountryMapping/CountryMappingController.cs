using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Clients;
using GoRegister.ApplicationCore.Domain.Countries;
using GoRegister.ApplicationCore.Domain.Projects.Services;
using GoRegister.ApplicationCore.Domain.ServiceCountryMapping;
using GoRegister.ApplicationCore.Domain.Settings.Models;
using GoRegister.ApplicationCore.Domain.Settings.Services;
using GoRegister.ApplicationCore.Framework;
using GoRegister.Areas.Admin.Controllers;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
//using GoRegister.ApplicationCore.Framework.ProjectTenant;
namespace GoRegister.Areas.Admin.Features.CountryMapping
{
    public class CountryMappingController : ProjectAdminControllerBase
    {
        private readonly IServiceCountryMappingService _countryMappingService;
        private readonly IProjectService _projectService;
        private readonly ISettingsService _settingsService;
        private readonly IClientService _clientservice;
        public static int ProjectId { get; set; }
        public static string ClientUuid { get; set; }
        public static int UserId { get; set; }
        public static string AddEdit { get; set; }
        public CountryMappingController(IServiceCountryMappingService countryMappingService, IProjectService projectService, ISettingsService settingsService, IClientService clientService)
        {
            _countryMappingService = countryMappingService;
            _projectService = projectService;
            _settingsService = settingsService;
            _clientservice = clientService;
        }
        [HttpGet]
        public async Task<IActionResult> AddServiceCountry()
        {
            var project = await _settingsService.GetSettingsAsync<GeneralSettingsModel>();
            var client = await _clientservice.GetClientById(project.ClientId);
            ProjectId = project.Id;
            ClientUuid = client.ClientUuid;
            UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await LoadData();
            AddEdit = "Add";
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddServiceCountry([FromForm] ServiceCountryMappingModel model)
        {
            AddEdit = model.AddEdit;
            if (model.ServiceCountryUuid == "0"|| model.ServiceCountryUuid==null|| model.ServiceCountryUuid == "0")
            {
            
                await LoadData();
                ViewBag.Smessage = "Please select Service Country";
                return View(model);
            }else if(model.RequestCountryId=="" || model.RequestCountryId==null)
            {
               
                await LoadData();
                ViewBag.Rmessage = "Please select Requestor Country";
                return View(model);
            }
            if (AddEdit == "Add")
            {
                var dt = await _countryMappingService.GetList(ProjectId, ClientUuid);
                var checksc = dt.Where(i => i.ServiceCountryUuid == model.ServiceCountryUuid).ToList();
                var checkrc = dt.Select(e => e.RequestCountry).Distinct().ToList();
                var rc = model.RequestCountry.Split(",", StringSplitOptions.RemoveEmptyEntries);
                if (checksc.Count > 0)
                {
                    await LoadData();
                    ViewBag.Smessage = "Service Country Mapping Already exists. Please choose another Country";
                    return View(model);
                }
                string r = "";
                foreach (string c in rc)
                {
                    if (checkrc.Contains(c))
                    {
                        r = r + " " + c;
                    }
                }
                if (r != "")
                {
                    await LoadData();
                    ViewBag.Rmessage = "Requestor Country  '" + r + "' already Mapped to a different Country. Please choose another Country";
                    return View(model);
                }
            }
            //else
            {
                if (AddEdit == "Edit")
                {
                    var dt = await _countryMappingService.GetList(ProjectId, ClientUuid);
                    var checksc = dt.Where(i => i.ServiceCountryUuid == model.ServiceCountryUuid).ToList();
                    var checkrc = dt.Where(i=>i.ServiceCountryUuid!= model.ServiceCountryUuid).Select(e => e.RequestCountry).Distinct().ToList();
                    var rc = model.RequestCountry.Split(",", StringSplitOptions.RemoveEmptyEntries);
                    string r = "";
                    foreach (string c in rc)
                    {
                        if (checkrc.Contains(c))
                        {
                            r = r + " " + c;
                        }
                    }
                    if (r != "")
                    {
                        await LoadData();
                        ViewBag.Rmessage = "Requestor Country '" + r + "' already mapped to a different Country. Please choose another Country";
                        return View(model);
                    }
                    await _countryMappingService.Delete(ProjectId, ClientUuid, model.ServiceCountryUuid);

                }
                List<MRFServiceCountryMapping> dtos = new List<MRFServiceCountryMapping>();
                var rqct = model.RequestCountry.Split(",", StringSplitOptions.RemoveEmptyEntries);
                if (rqct.Length == 1)
                {
                    MRFServiceCountryMapping dto = new MRFServiceCountryMapping();
                    dto.IsDefault = true;
                    dto.ClientUuid = ClientUuid;
                    dto.ProjectId = ProjectId;
                    dto.DateCreated = DateTime.UtcNow;
                    dto.CreatedBy = UserId;
                    dto.RequestCountry = rqct[0].ToString();
                    dto.RequestCountryId =await _countryMappingService.GetCountryISO(rqct[0].ToString());
                    dto.ServiceCountry = model.ServiceCountry;
                    dto.ServiceCountryUuid = model.ServiceCountryUuid;
                    dto.IsActive = true;
                    dtos.Add(dto);
                }
                else
                {
                    foreach (string ct in rqct)
                    {
                        MRFServiceCountryMapping dto = new MRFServiceCountryMapping();
                        dto.IsDefault = false;
                        dto.ClientUuid = ClientUuid;
                        dto.ProjectId = ProjectId;
                        dto.DateCreated = DateTime.UtcNow;
                        dto.CreatedBy = UserId;
                        dto.RequestCountry = ct.ToString();
                        dto.RequestCountryId = await _countryMappingService.GetCountryISO(ct.ToString());
                        dto.ServiceCountry = model.ServiceCountry;
                        dto.ServiceCountryUuid = model.ServiceCountryUuid;
                        dto.IsActive = true;
                        dtos.Add(dto);
                    }
                }
                var result = await _countryMappingService.Create(dtos);
                AddEdit = "Add";
                await LoadData();
                return View();
            }
        }

        public async Task<JsonResult> Edit(string ServiceCountryUuid)
        {
            ServiceCountryMappingModel m = new ServiceCountryMappingModel();
            var id = ServiceCountryUuid;
            var dtos = await _countryMappingService.GetMappingbyServicecountry(ProjectId, ClientUuid, ServiceCountryUuid);
            m = ArrangeRequestorCountry(dtos)[0];
            AddEdit = "Edit";
            return Json(m);
        }
        public async Task LoadData()
        {
            ViewBag.ServiceCountryList = await _countryMappingService.LoadServiceCountry(ProjectId, ClientUuid);
                   
            //ViewBag.ServiceCountryList = await _countryMappingService.LoadRequestCountry();
            ViewBag.RequestCountryList = await _countryMappingService.LoadRequestCountry();
            ViewBag.ProjectId = ProjectId;
            var servicecountrylist = await _countryMappingService.LoadServiceCountry(ProjectId, ClientUuid);
            var dtos = await _countryMappingService.GetList(ProjectId, ClientUuid);
            var carr = dtos.Select(i => i.ServiceCountry).Distinct().ToList();int count = 0;
            if (servicecountrylist != null)
            {               
                var scty = servicecountrylist.Select(e => e.Text).ToList();
                if (carr.Count > 0)
                {
                    foreach (string country in scty)
                    {
                        if (!carr.Contains(country))
                        {
                            count++;
                        }
                    }
                }
                else
                {
                    count = servicecountrylist.Count();
                }
            }
           
            ViewBag.ServiceCountryMappingList = ArrangeRequestorCountry(dtos);
            ViewBag.Smessage = "";
            ViewBag.Rmessage = "";
            ViewBag.Countmap = count;
        }

        public ServiceCountryMappingModel LoadViewModel(string reqCountry, string reqCountryId, MRFServiceCountryMapping scm)
        {
            ServiceCountryMappingModel vm = new ServiceCountryMappingModel();
            vm.ClientUuid = scm.ClientUuid;
            vm.ProjectId = scm.ProjectId;
            vm.ServiceCountryUuid = scm.ServiceCountryUuid;
            vm.ServiceCountry = scm.ServiceCountry;
            vm.RequestCountry = reqCountry;
            vm.RequestCountryId = reqCountryId;
            vm.IsActive = scm.IsActive;
            vm.IsDefault = scm.IsDefault;
            vm.CreatedBy = scm.CreatedBy;
            vm.DateCreated = scm.DateCreated;
            return vm;
        }

        public List<ServiceCountryMappingModel> ArrangeRequestorCountry(List<MRFServiceCountryMapping> dtos)
        {
            List<ServiceCountryMappingModel> Vms = new List<ServiceCountryMappingModel>();
            var serviceCountries = dtos.Select(e => e.ServiceCountryUuid).Distinct().ToList();
            foreach (var servCountry in serviceCountries)
            {
                var servdtos = dtos.Where(e => e.ServiceCountryUuid == servCountry).ToList();
                var requestorcountryIdslist = servdtos.Select(e => e.RequestCountryId).Distinct().ToList();
                var requestorcountrieslist = servdtos.Select(e => e.RequestCountry).Distinct().ToList();
                string reqcountry = string.Join(",", requestorcountrieslist);
                string reqcountryId = string.Join(",", requestorcountryIdslist);
                ServiceCountryMappingModel vm = LoadViewModel(reqcountry, reqcountryId, servdtos[0]);
                Vms.Add(vm);
            }
            return Vms;
        }
        public async Task<int> Delete(string ServiceCountryUuid)
        {
            await _countryMappingService.Delete(ProjectId, ClientUuid, ServiceCountryUuid);
            var servicecountrylist = await _countryMappingService.LoadServiceCountry(ProjectId, ClientUuid);
            var dtos = await _countryMappingService.GetList(ProjectId, ClientUuid);
            var carr = dtos.Select(i => i.ServiceCountry).Distinct().ToList(); int count = 0;
            if (servicecountrylist != null)
            {
                var scty = servicecountrylist.Select(e => e.Text).ToList();
                if (carr.Count > 0)
                {
                    foreach (string country in scty)
                    {
                        if (!carr.Contains(country))
                        {
                            count++;
                        }
                    }
                }
                else
                {
                    count = servicecountrylist.Count();
                }
            }
            ViewBag.Countmap = count;
            return count;
        }

    }
}
