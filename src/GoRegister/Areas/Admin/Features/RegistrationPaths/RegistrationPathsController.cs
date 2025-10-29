using GoRegister.ApplicationCore.Domain.Liquid.Queries;
using GoRegister.Areas.Admin.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.Areas.Admin.Features.RegistrationPaths
{
    public class RegistrationPathsController : ProjectAdminControllerBase
    {
        private readonly IMediator _mediator;

        public RegistrationPathsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route(ResetProjectRoute + "/RegistrationPaths")]
        public async Task<IActionResult> Index()
        {
            var result = await _mediator.Send(new Index.Query());
            if (!result.IsPathsEnabled)
                return RedirectToAction(nameof(Edit), new { id = result.OnlyPathId });
            return View(result);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var result = await _mediator.Send(new Edit.Query(id));
            if (result == null) return NotFound();
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Edit.Command command)
        {
            if(!ModelState.IsValid)
            {
                await _mediator.Send(new Edit.Query(command));
                return View(command);
            }

            var id = await _mediator.Send(command);
            return RedirectToAction(nameof(Edit), new { id });
        }      
    }
}
