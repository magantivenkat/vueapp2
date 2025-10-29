using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Delegates.Models;
using GoRegister.ApplicationCore.Domain.Liquid;
using GoRegister.ApplicationCore.Domain.Registration;
using GoRegister.ApplicationCore.Framework.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Delegates
{
    public interface ICurrentAttendeeAccessor
    {
        Task<AttendeeUserWrapper> Get();
    }

    public class CurrentAttendeeAccessor : ICurrentAttendeeAccessor
    {
        private readonly ApplicationDbContext _db;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IDelegateUserCacheService _delegateUserCacheService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CurrentAttendeeAccessor(ApplicationDbContext db, ICurrentUserAccessor currentUserAccessor, UserManager<ApplicationUser> userManager, IDelegateUserCacheService delegateUserCacheService)
        {
            _db = db;
            _currentUserAccessor = currentUserAccessor;
            _userManager = userManager;
            _delegateUserCacheService = delegateUserCacheService;
        }

        public async Task<AttendeeUserWrapper> Get()
        {
            var user = _currentUserAccessor.Get;
            if(!user.Identity.IsAuthenticated)
            {
                return new AttendeeUserWrapper(null);
            }

            var userId = int.Parse(_userManager.GetUserId(user));
            var attendee = await _db.Delegates
                .Select(d => new AttendeeUserModel
                {
                    Id = d.Id,
                    Email = d.ApplicationUser.Email,
                    RegistrationStatus = (Data.Enums.RegistrationStatus)d.RegistrationStatusId,
                    FirstName = d.ApplicationUser.FirstName,
                    LastName = d.ApplicationUser.LastName,
                    RegistrationTypeId = d.RegistrationTypeId,
                    UniqueIdentifier = d.UniqueIdentifier,
                    RegistrationDocument = d.RegistrationDocument,
                    RegistrationType = d.RegistrationType.Name,
                    
                })
                .FirstOrDefaultAsync(a => a.Id == userId);

            if(user == null)
            {
                return new AttendeeUserWrapper(null);
            }

            var d = _delegateUserCacheService.Get(attendee);

            return new AttendeeUserWrapper(d);
        }
    }
}
