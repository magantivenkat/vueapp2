using GoRegister.ApplicationCore.Domain.Settings.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GoRegister.Features.User
{
    public class UserController : Controller
    {
        private readonly IProjectSettingsAccessor _settingsAccessor;

        public UserController(IProjectSettingsAccessor settingsAccessor)
        {          
            _settingsAccessor = settingsAccessor;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(string returnUrl)
        {
            var settings = await _settingsAccessor.GetAsync();

            ViewBag.emailFrom = settings.EmailReplyTo !=null ? settings.EmailReplyTo.ToString() : null;
            ViewBag.returnUrl = returnUrl;

            return View();
        }
    }
}
