using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.CustomPages.Services;
using GoRegister.ApplicationCore.Domain.Delegates;
using GoRegister.ApplicationCore.Domain.Menus.Queries;
using GoRegister.ApplicationCore.Domain.Settings.Services;
using GoRegister.ApplicationCore.Framework;
using GoRegister.Areas.Admin.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly IProjectSettingsAccessor _projectSettingsAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ICustomPageService _customPageService;
        private readonly IMediator _mediator;
        private readonly ICurrentAttendeeAccessor _currentAttendee;

        public MenuViewComponent(ApplicationCore.Domain.Settings.Services.IProjectSettingsAccessor projectSettingsAccessor,
                                            UserManager<ApplicationUser> userManager, ApplicationDbContext context, ICustomPageService customPageService, IMediator mediator, ICurrentAttendeeAccessor currentAttendee)
        {
            _projectSettingsAccessor = projectSettingsAccessor;
            _userManager = userManager;
            _context = context;
            _customPageService = customPageService;
            _mediator = mediator;
            _currentAttendee = currentAttendee;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var attendeeWrapper = await _currentAttendee.Get();

            var vm = await _mediator.Send(new BuildMenuQuery() { AttendeeAuthorizationModel = attendeeWrapper.GetAuthModel() });

            return View(vm);
        }
    }
}
