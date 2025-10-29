using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Registration.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GoRegister.Controllers
{
    public class UserController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UserController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IRegistrationLinkService _serviceProvider;

        public UserController(ILogger<UserController> logger, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context, IRegistrationLinkService serviceProvider)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _serviceProvider = serviceProvider;
        }

        [AllowAnonymous]
        public async Task<IActionResult> DirectLogin(Guid id, string returnUrl, bool isPersistent = false)
        {
            var user = await _context.Delegates.Include(e => e.ApplicationUser).FirstOrDefaultAsync(e => e.UniqueIdentifier == id);
            await _signInManager.SignInAsync(user.ApplicationUser, isPersistent);
            returnUrl = !string.IsNullOrWhiteSpace(returnUrl) ? $"~{returnUrl}" : "~/";

            var host = await _serviceProvider.GetHostForRedirect();
            returnUrl = returnUrl + "?longUrl=" + user.MRFClientUserInvitationlink.Replace(host, string.Empty);
            return Redirect(returnUrl);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Preview(Guid id, string returnUrl, bool isPersistent = false)
        {
            var user = await _context.Delegates.Include(e => e.ApplicationUser).FirstOrDefaultAsync(e => e.UniqueIdentifier == id && e.IsTest);
            await _signInManager.SignInAsync(user.ApplicationUser, isPersistent);
            returnUrl = !string.IsNullOrWhiteSpace(returnUrl) ? $"~{returnUrl}" : "~/MRF";

            var host = await _serviceProvider.GetHostForRedirect();
            returnUrl = returnUrl + "?longUrl=" + user.MRFClientUserInvitationlink.Replace(host, string.Empty);
            return Redirect(returnUrl);
        }
    }
}
