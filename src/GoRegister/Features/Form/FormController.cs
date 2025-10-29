using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Registration;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Builder;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Models;
using GoRegister.ApplicationCore.Domain.Registration.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoRegister.Features.Form
{
    public class FormController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFormService _formService;
        private readonly ApplicationDbContext _db;
        private readonly IRegistrationService _registrationService;

        public FormController(UserManager<ApplicationUser> userManager, IFormService formService, ApplicationDbContext db, IRegistrationService registrationService)
        {
            _userManager = userManager;
            _formService = formService;
            _db = db;
            _registrationService = registrationService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Preview(string id)
        {
            var previewRegType = await _db.RegistrationTypes.FirstAsync();

            var previewUser = new FormResponseModel()
            {
                RegistrationTypeId = previewRegType.Id,
                Model = new Dictionary<string, Newtonsoft.Json.Linq.JToken>()
            };

            return View(new PreviewViewModel(id, previewUser));
        }

        [HttpPost]
        public async Task<IActionResult> PreviewModel([FromBody] FormEditorPreviewModel vm)
        {
            var previewResult = await _registrationService.SaveForm(vm.Id, vm);

            var regTypeId = (await _db.RegistrationTypes.FirstAsync()).Id;
            // build reg model
            var model = await _formService.GenerateFormSchema(regTypeId, previewResult);
            var dict = new Dictionary<int, FormSchema>();
            dict.Add(regTypeId, model);
            return Ok(dict);
        }

        public async Task<IActionResult> Submit(int id)
        {
            if(User.Identity.IsAuthenticated)
            {
                var formModel = await _formService.GetForm(id);
                var userId = int.Parse(_userManager.GetUserId(User));
                var formResponse = await _formService.GetUserResponseModel(formModel.Form.Id, userId);
                if (formResponse.Failed)
                {
                    return NotFound();
                }

                var model = await _formService.GetFormDisplayModel(formModel, formResponse.Value);
                return View("Index", model);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Submit(int id, [FromBody] FormModel model)
        {
            UserFormResponseModel response = null;
            var formModel = await _formService.GetForm(id);
            if (User.Identity.IsAuthenticated)
            {
                // get current user
                var userId = int.Parse(_userManager.GetUserId(User));
                var userResponseResult = await _formService.CreateUserResponseModel(formModel.Form.Id, userId);
                if (userResponseResult)
                {
                    response = userResponseResult.Value;
                }
                else
                {
                    // why would this fail?
                }
            }
            else
            {
                //TODO:handle anonymous reg
            }



            response.Response.SetupAudit();
            await _formService.ProcessFormResponse(formModel, response, model);
            var serializedForm = _formService.SerializeForm(response.Response, formModel);
            //add audit record
            var audit = response.Response.GetAudit(ActionedFrom.Form, response.Response.DelegateUser?.ApplicationUser);
            _db.DelegateAudits.Add(audit);

            await _db.SaveChangesAsync();
            return Ok();
        }
    }

    public class PreviewViewModel
    {
        public PreviewViewModel(string id, FormResponseModel user)
        {
            Id = id;
            User = user;
        }

        public string Id { get; set; }
        public FormResponseModel User { get; set; }
    }
}
