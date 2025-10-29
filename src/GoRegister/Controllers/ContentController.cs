using Microsoft.AspNetCore.Mvc;
using GoRegister.ApplicationCore.Data;
using System.Linq;
using System.Threading.Tasks;
using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.Filters;
using GoRegister.ApplicationCore.Domain.CustomPages.Queries;
using GoRegister.Framework.MVC;
using GoRegister.ApplicationCore.Domain.CustomPages.Models;

namespace GoRegister.Controllers
{
    public class ContentController : GoRegisterControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ContentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("~/")]
        public async Task<ActionResult<CustomPageDisplayModel>> Index()
        {
            var query = new RenderCustomPageQuery()
            {
                PageType = PageType.HomePage
                //PageType = PageType.CustomPage
            };
            var result = await Mediator.Send(query);

            if(!result.Failed)
            {
                var vm = result.Value;
                ViewData["Title"] = vm.Title;
                return View("Display", vm);
            }

            return result.ToActionResult();
        }

        [Route("~/page/{slug}")]
        public async Task<ActionResult<CustomPageDisplayModel>> Page(string slug)
        {
            var query = new RenderCustomPageQuery()
            {
                PageType = PageType.CustomPage,
                Slug = slug
            };
            var result = await Mediator.Send(query);

            if (!result.Failed)
            {
                var vm = result.Value;
                ViewData["Title"] = vm.Title;
                return View("Display", vm);
            }

            return result.ToActionResult();
        }
    }
}
