using GoRegister.ApplicationCore.Domain.Emails.Queries;
using GoRegister.ApplicationCore.Services.Email;
using GoRegister.Areas.Admin.Controllers;
using GoRegister.Framework.MVC;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace GoRegister.Areas.Admin.Features.Emails
{
    [Route(ProjectRoute + "/api/[controller]/[action]/{id?}")]
    public class EmailsController : ProjectAdminControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;
        private readonly IEmailService _emailService;

        public EmailsController(IConfiguration configuration, IMediator mediator, IEmailService emailService)
        {
            _configuration = configuration;
            _mediator = mediator;
            _emailService = emailService;
        }

        [Route(IndexRoute)]
        [Route(ResetProjectRoute + "/emails/{*path}")]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [Route(ResetProjectRoute + "/emails/PreviewEmail/{id}")]
        public async Task<IActionResult> PreviewEmail(Preview.Query query)
        {
            var result = await _mediator.Send(query);
            if (!result)
            {
                return NotFound();
            }

            return View("PreviewEmail", result.Value);
        }

        [HttpPost]
        [Route(ResetProjectRoute + "/emails/PreviewEmailTemplate")]
        public async Task<IActionResult> PreviewEmailTemplate([FromBody]PreviewEmailTemplate.Query query)
        {
            var result = await _mediator.Send(query);
            return new JsonResult(result);
        }

        public async Task<IActionResult> ListEmails()
        {
            return Json(await _mediator.Send(new ListEmails.Query()));
        }

        public async Task<IActionResult> Create(CreateEmail.Query query)
        {
            return Json(await _mediator.Send(query));
        }

        public async Task<ActionResult<CreateEditEmail.Response>> Edit(EditEmail.Query query)
        {
            return (await _mediator.Send(query)).ToActionResult();
        }

        [HttpPost]
        public async Task<ActionResult<int>> PostEmail([FromBody]CreateEditEmail.Command command)
        {
            return (await _mediator.Send(command)).ToActionResult();
        }

        public async Task<ActionResult<CreateEditEmailLayout.Response>> EditLayout(EditEmailLayout.Query query)
        {
            return (await _mediator.Send(query)).ToActionResult();
        }

        [HttpPost]
        public async Task<ActionResult<int>> PostEmailLayout([FromBody]CreateEditEmailLayout.Command command)
        {
            return (await _mediator.Send(command)).ToActionResult();
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Preview([FromBody] GenerateEmailPreview.Command command)
        {
            return (await _mediator.Send(command)).ToActionResult();
        }

        public async Task<ActionResult<GetEmailPreviews.Response>> GetPreviews(GetEmailPreviews.Query query)
        {
            return (await _mediator.Send(query)).ToActionResult();
        }

        public async Task<ActionResult<Guid>> SendBatch(Guid id) =>
            (await _emailService.SendEmails(id)).ToActionResult();

        public async Task<IActionResult> SearchUsers(SearchUsersQuery.Query query) =>
            Json(await _mediator.Send(query));

        public async Task<ActionResult<EmailLookup.Response>> EmailLookup(EmailLookup.Query query) =>
            (await _mediator.Send(query)).ToActionResult();

        public async Task<ActionResult<Send.Response>> Send(Send.Query query) =>
            (await _mediator.Send(query)).ToActionResult();

        [HttpPost]
        public async Task<ActionResult<CustomSearchUsers.Response>> CustomSearchUsers([FromBody]CustomSearchUsers.Query query) =>
            (await _mediator.Send(query)).ToActionResult();

        public async Task<IActionResult> CheckBatchStatus(CheckBatchStatus.Query query) =>
           Json(await _mediator.Send(query));

    }
}
