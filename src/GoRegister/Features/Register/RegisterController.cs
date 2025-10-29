/* MRF Changes: Clear first 3 entities of on User MRF Registration form
Modified Date: 18th October 2022
Modified By: Mandar.Khade@amexgbt.com
Team member: Harish.Rane@amexgbt.com
JIRA Ticket No: GoRegister / GOR - 238 - New */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Domain.Registration.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using GoRegister.ApplicationCore.Domain.Registration;
using GoRegister.ApplicationCore.Domain.Registration.Framework;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Builder;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using GoRegister.ApplicationCore.Framework.Domain;
using GoRegister.ApplicationCore.Domain.Registration.Commands;
using GoRegister.ApplicationCore.Domain.Registration.Queries;
using GoRegister.Framework.MVC;
using GoRegister.ApplicationCore.Data.Enums;
using System.Linq;
using GoRegister.ApplicationCore.Data.Models;

namespace GoRegister.Features.Register
{
    public class RegisterController : Controller
    {
        private readonly IFormService _formService;
        private readonly ApplicationDbContext _context;
        private readonly IRegistrationService _registrationService;

        private readonly IMediator _mediator;

        public RegisterController(
            ApplicationDbContext context,
            IFormService formService, IRegistrationService registrationService, IMediator mediator)
        {
            _context = context;
            _formService = formService;
            _registrationService = registrationService;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("~/MRF")]
        public async Task<IActionResult> Index(int? typeId)
        {
            var query = new IndexHandler.Query()
            {
                RegistrationTypeId = typeId
            };

            var result = await _mediator.Send(query);

            if (result.Status == IndexHandler.ResponseStatus.DisplayRegistration)
            {

                string keyId = string.Empty;

                var formModel = await _formService.GetRegistrationForm();
                       
                foreach (var item in formModel.Form.RegistrationPages)
                {                    
                     keyId = item.Fields.FirstOrDefault(m => m.FieldTypeId == FieldTypeEnum.Textbox && m.DataTag.ToLower() == "companyname").Id.ToString();
                     break;
                }            

                result.RegisterModel.User.Model.Clear();


                if (keyId != string.Empty)
                {
                    var project = await _context.Projects.FirstOrDefaultAsync(e => e.Id == formModel.Form.ProjectId);
                    var client = await _context.Clients.FirstOrDefaultAsync(e => e.ClientUuid == project.Prefix);
                    result.RegisterModel.User.Model[keyId] = client.Name;
                }

                foreach (var item in formModel.Form.RegistrationPages)
                {
                    foreach (var itemField in item.Fields)
                    {
                        if (itemField.IsHidden == true && itemField.IsStandardField == true)
                        {
                            var keyIdField = itemField.Id.ToString();
                            //result.RegisterModel.User.Model[keyIdField] = "dummy";
                            result.RegisterModel.User.Model[keyIdField] = itemField.DefaultValue ?? "dummy";
                        }
                    }
             
                }

                return View(result.RegisterModel);
            }

            switch (result.Status)
            {
                case IndexHandler.ResponseStatus.DisplayRegistration:
                    return View("Index", result.RegisterModel);
                case IndexHandler.ResponseStatus.DisplayRegistrationStatusPage:
                    return StatusPageResult(result.RegistrationStatusModel);
                case IndexHandler.ResponseStatus.NotFound:
                    return NotFound();
                case IndexHandler.ResponseStatus.RegistrationClosed:
                    return NotFound();
                default:
                    return NotFound();
            }
        }

        private IActionResult StatusPageResult(IndexHandler.RegistrationStatusModel registrationStatusModel)
        {
            return View("StatusPage", registrationStatusModel);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] FormModel model)
        {
            var query = new RegisterHandler.Query { Model = model };
            var result = await _mediator.Send(query);
            return Json(result);       
        }

        [Authorize]
        public async Task<IActionResult> Decline()
        {
            var result = await _mediator.Send(new DeclineHandler.Query());
            if (result.Failed)
            {
                return Content(((Error.Invalid)result.Error).Message);
            }

            return View(result.Value.Model);
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> Decline([FromBody] FormModel model)
        {
            var query = new DeclineRegistration.Command { Model = model };
            var result = await _mediator.Send(query);
            return result.ToJsonResult();
        }
        
        [Authorize]
        public async Task<IActionResult> Cancel()
        {
            var result = await _mediator.Send(new CancelHandler.Query());
            if (result.Failed)
            {
                return Content(((Error.Invalid)result.Error).Message);
            }

            return View(result.Value.Model);
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> Cancel([FromBody] FormModel model)
        {
            var query = new CancelRegistration.Command { Model = model };
            var result = await _mediator.Send(query);
            return result.ToJsonResult();
        }

        public IActionResult Preview()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PreviewModel(int id, [FromBody] FormEditorUpdateModel vm)
        {
            var previewResult = await _registrationService.SaveForm(id, vm);

            var regTypeId = (await _context.RegistrationTypes.FirstAsync()).Id;
            // build reg model
            var model = await _formService.GenerateFormSchema(regTypeId, previewResult);
            var dict = new Dictionary<int, FormSchema>();
            dict.Add(regTypeId, model);
            return Ok(dict);
        }
    }

    public class ControllerModelUpdater : IUpdateModel
    {
        private readonly Controller _controller;
        private readonly IFormCollection _formCollection;

        public ControllerModelUpdater(Controller controller)
        {
            _controller = controller;
        }

        public ControllerModelUpdater(Controller controller, IFormCollection formCollection) : this(controller)
        {
            _formCollection = formCollection;
        }

        public ModelStateDictionary ModelState => _controller.ModelState;
        public IFormCollection Form => _formCollection ?? _controller.HttpContext.Request.Form;

        public Task<bool> TryUpdateModelAsync<TModel>(TModel model) where TModel : class
        {
            return _controller.TryUpdateModelAsync<TModel>(model);
        }

        public Task<bool> TryUpdateModelAsync<TModel>(TModel model, string prefix) where TModel : class
        {
            return _controller.TryUpdateModelAsync<TModel>(model, prefix);
        }

        public Task<bool> TryUpdateModelAsync<TModel>(TModel model, string prefix, params Expression<Func<TModel, object>>[] includeExpressions) where TModel : class
        {
            return _controller.TryUpdateModelAsync<TModel>(model, prefix, includeExpressions);
        }

        public bool TryValidateModel(object model)
        {
            return _controller.TryValidateModel(model);
        }

        public bool TryValidateModel(object model, string prefix)
        {
            return _controller.TryValidateModel(model, prefix);
        }
    }


    public class UpdateModel
    {
        public IFormCollection FormCollection { get; set; }

    }
}