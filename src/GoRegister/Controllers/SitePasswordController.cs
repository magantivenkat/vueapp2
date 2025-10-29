using GoRegister.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Settings.Models;
using GoRegister.ApplicationCore.Domain.Settings.Services;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace GoRegister.Controllers
{
    [AllowAnonymous]
    public class SitePasswordController : Controller
    {
        private readonly IProjectSettingsAccessor _settings;

        public SitePasswordController(IProjectSettingsAccessor settings)
        {
            _settings = settings;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string password)
        {
            var settings = await _settings.GetAsync();
            //compare entered pass to hashed pass
            var result = new PasswordHasher<ApplicationUser>().VerifyHashedPassword(null, settings.SitewidePasswordHashed, password);

            if (result != PasswordVerificationResult.Success) return View();

            var prefixPath = HttpContext.Request.PathBase;
            HttpContext.Response.Cookies.Append(Constants.SitePasswordCookie, settings.SitewidePasswordHashed);
            return Redirect("~/");
        }
    }
}
