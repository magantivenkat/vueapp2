using GoRegister.ApplicationCore.Domain.Liquid.Queries;
using GoRegister.Areas.Admin.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GoRegister.Areas.Admin.Features.DataTags
{
    public class DataTagsController : ProjectAdminControllerBase
    {
        private readonly IMediator _mediator;

        public DataTagsController(IMediator mediator)
        {
            _mediator = mediator;
        }
  
        public async Task<IActionResult> List()
        {
            return Json(await _mediator.Send(new ListDataTagsQuery.Query()));
        }
    }
}
