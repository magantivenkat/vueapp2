/*  MRF Changes : Remove registration status column;
    Modified Date : 30th September 2022
    Modified By : Mandar.Khade @amexgbt.com
    Team member : Harish.Rane @amexgbt.com
    JIRA Ticket No : GoRegister/GOR-236

    MRF Changes : Create MRF Client Form details in Json format 
    Modified Date : 06th October 2022
    Modified By : Mandar.Khade@amexgbt.com
    Team member : Harish.Rane@amexgbt.com
    JIRA Ticket No : GoRegister/GOR-238-New  

    MRF Changes : Remove edit MRF Client Form details saved in Json format 
    Modified Date : 31st October 2022
    Modified By : Mandar.Khade@amexgbt.com
    Team member : Harish.Rane@amexgbt.com
    JIRA Ticket No : GoRegister/GOR-228-3 

   MRF Changes : Share Link functionality
    Modified Date : 02nd Nov 2022
    Modified By : Mandar.Khade@amexgbt.com
    Team member : Harish.Rane@amexgbt.com
    JIRA Ticket No : GoRegister/GOR-242 
 
 */

using GoRegister.Areas.Admin.Controllers;
using GoRegister.Areas.Admin.Extensions;
using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Registration.Models;
using GoRegister.ApplicationCore.Domain.Registration.Services;
using GoRegister.ApplicationCore.Domain.Settings.Services;
using GoRegister.ApplicationCore.Framework.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoRegister.ApplicationCore.Framework.DataTables;
using GoRegister.ApplicationCore.Domain.Registration;
using GoRegister.ApplicationCore.Domain.Sessions.Services;
using MediatR;
using GoRegister.ApplicationCore.Domain.BulkUpload.Commands;
using GoRegister.Framework.MVC;
using GoRegister.ApplicationCore.Domain.BulkUpload.Queries;
using GoRegister.ApplicationCore.Domain.BulkUpload.Models;
using System.Data;
using GoRegister.ApplicationCore.Domain.Delegates;
using Microsoft.EntityFrameworkCore;
using GoRegister.ApplicationCore.Domain.Delegates.Queries;
using GoRegister.ApplicationCore.Domain.Sessions;
using static GoRegister.ApplicationCore.Domain.Reports.Framework.DateReportFilter;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace GoRegister.Areas.Admin.Features.Delegates
{
    public class DelegatesController : ProjectAdminControllerBase
    {
        private readonly IDelegateService _delegateService;
        private readonly IRegistrationService _registrationService;
        private readonly IRegistrationLinkService _registrationLinkService;
        private readonly IProjectSettingsAccessor _projectSettingsAccessor;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotifier _notifier;
        private readonly IFormService _formService;
        private readonly ISessionService _sessionService;
        private readonly IMediator _mediator;
        private readonly IDelegateUserCacheService _delegateUserCacheService;
        private readonly IConfiguration _configuration;


        public DelegatesController(INotifier notifier, IDelegateService delegateService, IRegistrationService registrationService, IRegistrationLinkService registrationLinkService, IProjectSettingsAccessor projectSettingsAccessor, ApplicationDbContext context, UserManager<ApplicationUser> userManager, IFormService formService, ISessionService sessionService, IMediator mediator, IDelegateUserCacheService delegateUserCacheService, IConfiguration configuration)
        {
            _notifier = notifier;
            _delegateService = delegateService;
            _registrationService = registrationService;
            _registrationLinkService = registrationLinkService;
            _projectSettingsAccessor = projectSettingsAccessor;
            _context = context;
            _userManager = userManager;
            _formService = formService;
            _sessionService = sessionService;
            _mediator = mediator;
            _delegateUserCacheService = delegateUserCacheService;
            _configuration = configuration;
        }

        //public IActionResult Index(bool? delegateDeleted)
        //{
        //    if (delegateDeleted.HasValue && delegateDeleted.Value)
        //    {
        //        _notifier.Success("Delegate deleted successfully");
        //    }

        //    return View();
        //}

        public IActionResult Index(bool? delegateDeleted)
        {
            if (delegateDeleted.HasValue && delegateDeleted.Value)
            {
                _notifier.Success("MRF URL shared successfully.");
            }
            
            return View();
        }

        public async Task<FileStreamResult> DelegateTemplate()
        {
            var stream = await _mediator.Send(new DownloadDelegateUploadTemplate.Query());
            return new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") { FileDownloadName = "delegate-upload-template.xlsx" };
        }

        [HttpPost]
        public async Task<IActionResult> LoadData([FromBody] DataTables.DtParameters dtParameters, int sessionId)
        {
            IEnumerable<DelegateListItemModel> delegates = new List<DelegateListItemModel>();

            delegates = sessionId > 0 ?
                await _sessionService.GetDelegatesForSession(sessionId) :
                await _delegateService.GetList();

            var totalResultsCount = delegates.Count(); // get the amount of total items (without the skip and take) 

            delegates = FilterDelegates(dtParameters, delegates);

            delegates = OrderDelegates(dtParameters, delegates);

            var result = Json(new
            {
                draw = dtParameters.Draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = delegates.Count(), // get the amount of filtered items  
                data = delegates
                    .Skip(dtParameters.Start)
                    .Take(dtParameters.Length)
            });

            return result;
        }

        private static IEnumerable<DelegateListItemModel> OrderDelegates(DataTables.DtParameters dtParameters, IEnumerable<DelegateListItemModel> delegates)
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

        private static IEnumerable<DelegateListItemModel> FilterDelegates(DataTables.DtParameters dtParameters, IEnumerable<DelegateListItemModel> delegates)
        {
            var searchBy = dtParameters.Search?.Value;
            if (!string.IsNullOrEmpty(searchBy))
            {
                //delegates = delegates
                //    .Where(r => r.Name?.ToUpper().Contains(searchBy.ToUpper()) == true
                //                || r.Email?.ToUpper().Contains(searchBy.ToUpper()) == true
                //                || r.RegistrationType?.ToUpper().Contains(searchBy.ToUpper()) == true
                //                || r.RegistrationStatus?.ToUpper().Contains(searchBy.ToUpper()) == true);

                delegates = delegates
                   .Where(r => r.Name?.ToUpper().Contains(searchBy.ToUpper()) == true
                               || r.Email?.ToUpper().Contains(searchBy.ToUpper()) == true);
            }

            return delegates;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var vm = await _delegateService.GetCreateModelAsync();

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(DelegateCreateModel model)
        {
            // validate
            if (ModelState.IsValid)
            {
                var result = await _delegateService.CreateAsync(model);
                if (result)
                {
                    return RedirectToAction("Edit", new { id = result.Value });
                }

                ModelState.AddModelError("", "There was an issue creating your delegate");

            }

            var vm = await _delegateService.GetCreateModelAsync(model);
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Manage(int id)
        {
            var user = await _registrationService.GetUser(id);
            if (user == null) return NotFound();
            var attendee = _delegateUserCacheService.Get(user);
            var model = await _formService.BuildFormSummary(attendee, FormType.Registration, true);

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {

            var formModel = await _formService.GetRegistrationForm();

            int index = Convert.ToInt32(this._configuration.GetSection("FieldDisplay")["Index"]);
            int count = formModel.Fields.Count - index;

            formModel.Fields.RemoveRange(index, count);

            var responseResult = await _formService.GetUserResponseModel(formModel.Form.Id, id);
            if (responseResult.Failed)
            {
                return NotFound();
            }

            formModel.IsAdmin = true;

            var model = await _formService.GetFormDisplayModel(formModel, responseResult.Value);
            return View("Edit", model);
        }

        [HttpPost]
        public async Task<IActionResult> MRFShareLink(int id)
        { 
            var du = await _registrationService.GetUser(id);
                      
            string InvitationLink = await _registrationLinkService.GetInvitationLink(du.UniqueIdentifier);

            var accessToken = this._configuration.GetSection("APIDetails")["AccessToken"];
            var mrfUpdateFormAPI = this._configuration.GetSection("APIDetails")["MRFUpdateFormAPI"];
            var project = await _context.Projects.SingleOrDefaultAsync(p => p.Id == du.ProjectId);                
            var formDictionary = new Dictionary<string, object>();    
            formDictionary.TryAdd("ExternalURL", InvitationLink);
            var invitationLinkData = JsonConvert.SerializeObject(formDictionary);
            var data = new StringContent(invitationLinkData);

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(mrfUpdateFormAPI + "/" + project.Prefix);
            client.DefaultRequestHeaders.Add("Authorization", accessToken);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                     
            var result = await client.PutAsync(client.BaseAddress, data);

            if (result.IsSuccessStatusCode == false)
            {
                return NotFound();
                
            }
            else
            {
                return Ok();
            }

        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var responseResult = await _delegateService.DeleteDelegate(id);
           
            if (responseResult.Failed)
            {
                return NotFound();
            }

            return Json(Url.Action("Index", "Delegates"));
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] FormModel model)
        {
            var formModel = await _formService.GetRegistrationForm();
            // get current user
            var userId = model.User.Id.Value;
            var userResponseResult = await _formService.GetUserResponseModel(formModel.Form.Id, userId);
            if (userResponseResult.Failed)
            {
                return NotFound();
            }
            var response = userResponseResult.Value;

            formModel.IsAdmin = true;

            var delegateUser = response.Response.DelegateUser;
            response.Response.SetupAudit();
            await _formService.ProcessFormResponse(formModel, response, model);
            var serializedForm = _formService.SerializeForm(response.Response, formModel);
            delegateUser.RegistrationDocument = serializedForm;
            //delegateUser.MRFClientResponse = _formService.SerializeFormRevised(response.Response, formModel);
            delegateUser.HasBeenModified();
            //add audit record
            var audit = response.Response.GetAudit(ActionedFrom.AdminForm, delegateUser.ApplicationUser);
            _context.DelegateAudits.Add(audit);

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetTestDelegates()
        {
            // Make sure it is a test delegate
            var previewCookie = Request.Cookies["PreviewAs"];

            var model = await _mediator.Send(new GetTestDelegatesForPreview.Query { CookieValue = previewCookie });
            return PartialView("_DropDownList", model.Delegates);
        }

        [HttpGet]
        public async Task<IActionResult> PreviewTestDelegate(int delegateId)
        {
            // Make sure it is a test delegate
            var delegateUser = await _context.Delegates.SingleOrDefaultAsync(d => d.Id == delegateId && d.IsTest);
            if (delegateUser == null) return BadRequest();

            var previewLink = await _registrationLinkService.GetPreviewLink(delegateUser.UniqueIdentifier);
            return Redirect(previewLink);
        }

        [HttpPost]
        public async Task<IActionResult> BulkUpload(UploadFileCommand command)
        {
            var result = await _mediator.Send(command);
            return result.ToJsonResult();
        }

        [HttpPost]
        public async Task<IActionResult> ValidateBulkUpload([FromBody] BulkUploadMappingModel model)
        {
            var query = new ValidateUploadQuery(model);
            var result = await _mediator.Send(query);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> ExecuteBulkUpload([FromBody] BulkUploadMappingModel model)
        {
            var query = new BulkUploadCommand(model);
            var result = await _mediator.Send(query);
            return Json(result);
        }

        public async Task<IActionResult> Audit(int id)
        {
            var audits = await _registrationService.BuildAudit(id);


            return View(audits);
        }

        class DelegateAuditModel
        {
            public string ActionedBy { get; set; }
            public DateTime ActionedUtc { get; set; }
            public string OldRegistrationStatus { get; set; }
            public string NewRegistrationStatus { get; set; }

        }

        [HttpPost]
        public async Task<IActionResult> ChangeRegistrationStatus(ChangeRegistrationStatusModel model, string returnUrl)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var user = await _registrationService.GetUser(model.Id);
            if (user == null)
            {
                return NotFound();
            }
            await _registrationService.UpdateRegistrationStatus(user, model.RegistrationStatus, currentUser, ActionedFrom.AdminForm);
            _notifier.Success("Registration status updated successfully");
            return Redirect(returnUrl);
        }


    }
}
