using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Framework;
using GoRegister.Models;
using Microsoft.AspNetCore.Authorization;
using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Framework.Multitenancy.Filters;
using GoRegister.Filters;
using MediatR;
using GoRegister.ApplicationCore.Domain.Delegates;
using GoRegister.ApplicationCore.Framework.Identity;

namespace GoRegister.Controllers
{
    [TenantFilter]
    [ThemedResultFilter]
    public class HomeController : Controller
    {
        private readonly ProjectTenant _tenant;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMemoryCache _memoryCache;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;
        private readonly ICurrentAttendeeAccessor _currentAttendeeAccessor;
        private readonly ICurrentUserAccessor _currentUserAccessor;

        public HomeController(ProjectTenant tenant, SignInManager<ApplicationUser> signInManager, IMemoryCache memoryCache, UserManager<ApplicationUser> userManager, ApplicationDbContext context, IMediator mediator, ICurrentUserAccessor currentUserAccessor, ICurrentAttendeeAccessor currentAttendeeAccessor)
        {
            _tenant = tenant;
            _signInManager = signInManager;
            _memoryCache = memoryCache;
            _userManager = userManager;
            _context = context;
            _mediator = mediator;
            _currentUserAccessor = currentUserAccessor;
            _currentAttendeeAccessor = currentAttendeeAccessor;
        }

        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
        public IActionResult Index()
        {

            ViewBag.Date = DateTime.Now;

            var tenant = _memoryCache.Get<string>("hello") ?? "";
            var page = _context.CustomPages.FirstOrDefault(e => e.Slug == null);

            if (page == null) return View();

            return View((object)page.Content);
        }

        public IActionResult Privacy()
        {
            _memoryCache.Set<string>("hello", _tenant.Name);

            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Preview(int id)
        {
            var user = _userManager.Users.IgnoreQueryFilters().FirstOrDefault(e => e.Id == id);

            await _signInManager.SignInAsync(user, false);

            HttpContext.Response.Cookies.Append("preview", JsonConvert.SerializeObject(new PreviewCookie()), new CookieOptions()
            {
                HttpOnly = true
            });

            return Redirect("~/");
        }

        [HttpPost]
        public IActionResult UpdatePreview(PreviewCookie preview, string returnUrl)
        {
            HttpContext.Response.Cookies.Append("preview", JsonConvert.SerializeObject(preview), new CookieOptions()
            {
                HttpOnly = true
            });

            return Redirect(returnUrl);
        }

        public async Task<IActionResult> HasAcceptedPrivacyPolicy()
        {
            var userId = _currentUserAccessor.GetUserId();
            if (userId == null)
            {
                return BadRequest();
            }

            var attendee = await _context.Delegates.FindAsync(userId);
            if (attendee.AcceptedPrivacyPolicy)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> AcceptPrivacyPolicy()
        {
            var userId = _currentUserAccessor.GetUserId();
            if (userId == null) return Ok(); // we don't care about anonymous users, cant do anything
            var attendee = await _context.Delegates.FindAsync(userId);
            if(!attendee.AcceptedPrivacyPolicy)
            {
                attendee.AcceptedPrivacyPolicy = true;
                attendee.AcceptedPrivacyPolicyDateUtc = SystemTime.UtcNow;
                await _context.SaveChangesAsync();
            }

            return Ok();
        }
    }

    public class PreviewCookie
    {
        public int RegType { get; set; }
    }
}
