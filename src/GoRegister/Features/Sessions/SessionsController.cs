using Microsoft.AspNetCore.Mvc;
using GoRegister.ApplicationCore.Domain.Sessions.Services;
using GoRegister.ApplicationCore.Domain.Sessions.ViewModels;
using GoRegister.ApplicationCore.Domain.Settings.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using GoRegister.ApplicationCore.Framework.Identity;

namespace GoRegister.Features.Register
{
    public class SessionsController : Controller
    {
        private readonly ISessionBookingService _sessionBookingService;
        private readonly IProjectSettingsAccessor _projectSettingsAccessor;
        private readonly ISessionService _sessionService;
        private readonly ICurrentUserAccessor _currentUserAccessor;

        public SessionsController(ISessionBookingService sessionBookingService, IProjectSettingsAccessor projectSettingsAccessor, ISessionService sessionService,ICurrentUserAccessor currentUserAccessor)
        {
            _sessionBookingService = sessionBookingService;
            _projectSettingsAccessor = projectSettingsAccessor;
            _sessionService = sessionService;
            _currentUserAccessor = currentUserAccessor;
        }

        [HttpPost]
        public IActionResult Reserve([FromBody] DelegateSessionBookingModel model)
        {
            var reservedSession = _sessionBookingService.ReserveSessionForDelegate(model.SessionId, model.DelegateId, false);
            return new JsonResult(reservedSession);
        }

        public async Task<IActionResult> Index()
        {
            var result = await GetUserSessions();

            return View("UserSessions", result);
        }
        private async Task<List<SessionModel>> GetUserSessions()
        {
            var project = await _projectSettingsAccessor.GetAsync();

            var result = _sessionService.GetUserSessions(_currentUserAccessor.GetUserId(), project.Id).ToList();

            return result;
        }
    }
}