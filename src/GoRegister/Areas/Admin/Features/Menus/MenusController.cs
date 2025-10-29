using GoRegister.ApplicationCore.Domain.Menus.Commands;
using GoRegister.ApplicationCore.Domain.Menus.Models;
using GoRegister.ApplicationCore.Domain.Menus.Queries;
using GoRegister.Areas.Admin.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GoRegister.Areas.Admin.Features.Menus
{
    public class MenusController : ApiProjectAdminControllerBase
    {
        const string EditItemRoute = ResetProjectRoute + "/[controller]/edit-item";
        const string CreateItemRoute = ResetProjectRoute + "/[controller]/create-item";

        private readonly IMediator _mediator;

        public MenusController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task<MenuViewModel> List() => _mediator.Send(new MenuListQuery());

        public async Task<IActionResult> UpdateList(MenuListUpdateCommand command)
        {
            var result = await _mediator.Send(command);
            if(result)
            {
                return Ok(await _mediator.Send(new MenuListQuery()));
            }

            return BadRequest();
        }

        [Route(CreateItemRoute), HttpGet]
        public async Task<IActionResult> CreateItem()
        {
            return Ok((await _mediator.Send(new MenuItemQuery(null))).Value);
        }

        [Route(CreateItemRoute), HttpPost]
        public async Task<IActionResult> CreateItem(MenuItemModel model)
        {
            return Ok(new { Id = (await _mediator.Send(new MenuItemSaveCommand(model))).Value });
        }

        [Route(EditItemRoute), HttpGet]
        public async Task<IActionResult> EditItem(int id)
        {
            return Ok((await _mediator.Send(new MenuItemQuery(id))).Value);
        }

        [Route(EditItemRoute), HttpPut]
        public async Task<IActionResult> EditItem(MenuItemModel model)
        {
            await _mediator.Send(new MenuItemSaveCommand(model));
            return Ok();
        }

        [Route(ResetProjectRoute + "/[controller]/delete-item"), HttpPost]
        public async Task<IActionResult> DeleteItem(int id)
        {
            await _mediator.Send(new MenuItemDeleteCommand() { Id = id });
            return Ok();
        }
    }
}
