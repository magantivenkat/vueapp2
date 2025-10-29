using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MediatR;
using System.Threading;
using GoRegister.ApplicationCore.Domain.Templates.Queries;

namespace GoRegister.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[controller]/[action]")]

    public class TemplatesController : AdminControllerBase
    {
        private readonly IMediator _mediator;

        public TemplatesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            var projects = await _mediator.Send(new GetTemplates.Query(), new CancellationToken());
            return View(projects);
        }

    }
}
