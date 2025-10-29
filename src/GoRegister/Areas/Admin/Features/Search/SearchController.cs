using GoRegister.Areas.Admin.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GoRegister.Areas.Admin.Features.Search
{
    public class SearchController : ProjectAdminControllerBase
    {
        private readonly IMediator _mediator;

        public SearchController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Query(SearchQuery.Query query) =>
            Json(await _mediator.Send(query));
    }
}
