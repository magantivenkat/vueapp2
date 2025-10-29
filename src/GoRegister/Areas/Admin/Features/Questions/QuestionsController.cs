using GoRegister.Areas.Admin.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GoRegister.Areas.Admin.Features.Questions
{
    public class QuestionsController : ProjectAdminControllerBase
    {
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
