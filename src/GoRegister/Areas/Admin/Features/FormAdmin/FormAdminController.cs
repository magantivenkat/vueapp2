using Microsoft.AspNetCore.Mvc;
using GoRegister.ApplicationCore.Data;
using System.Threading.Tasks;
using GoRegister.ApplicationCore.Domain.Registration.Services;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Builder;
using MediatR;
using GoRegister.Areas.Admin.Controllers;
using GoRegister.ApplicationCore.Data.Enums;

namespace GoRegister.Areas.Admin.Features.FormAdmin
{
    public class FormAdminController : ProjectAdminControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IRegistrationService _registrationService;
        private readonly IMediator _mediator;

        public FormAdminController(ApplicationDbContext context, IRegistrationService registrationService, IMediator mediator)
        {
            _context = context;
            _registrationService = registrationService;
            _mediator = mediator;
        }

        [Route("")]
        public async Task<IActionResult> Index()
        {
            var result = await _mediator.Send(new GetForms.Query());

            return View(result);
        }

        public IActionResult Edit(int id)
        {
            return View(id);
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            var model = await _registrationService.BuildEditorModel(id);
            return Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> Post(int id, [FromBody] FormEditorUpdateModel vm)
        {
            var result = await _registrationService.SaveForm(id, vm);

            foreach(var t in result.PreSaveExecuteActions)
            {
                await t();
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AddForm(string displayName, int formTypeId)
        {
            var result = await _mediator.Send(new AddForm.Command { DisplayName = displayName, FormType = (FormType)formTypeId });
            return RedirectToAction("Edit", new { id = result.Id } );
        }
    }
}