using GoRegister.Areas.Admin.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GoRegister.Areas.Admin.Features.Mediatr
{
    public class MediatrController : ProjectAdminControllerBase
    {
        private readonly IMediator _mediator;

        public MediatrController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _mediator.Send(new Index.Query());
            if(result)
            {
                return View(result.Value);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Index.Command command)
        {
            var result = await _mediator.Send(command);
            if(!result)
            {
                await _mediator.Publish(command);
                return View(command);
            }
            return View();
        }
    }
}
