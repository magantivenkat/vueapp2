using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Framework.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.UserActions
{
    public interface IUserActionService
    {
        Task<bool> ReachedActionThreshold(UserActionType actionType, int number, TimeSpan time, bool registerEvent = true);
        Task<bool> ReachedActionThresholdForAnonymous(UserActionType actionType, int number, TimeSpan time, string data, bool registerEventAndSave = true);
        Task RegisterAction(UserActionType actionType, bool saveChanges = true);
        Task RegisterAction(UserActionType actionType, string data, bool saveChanges = true);
    }

    public class UserActionService : IUserActionService
    {
        private readonly ApplicationDbContext _db;
        private readonly ICurrentUserAccessor _currentUser;

        public UserActionService(ApplicationDbContext db, ICurrentUserAccessor currentUser)
        {
            _db = db;
            _currentUser = currentUser;
        }

        public async Task RegisterAction(UserActionType actionType, bool saveChanges = true)
        {
            await RegisterAction(actionType, null, saveChanges);
        }

        public async Task RegisterAction(UserActionType actionType, string data, bool saveChanges = true)
        {
            _db.UserActions.Add(new UserAction
            {
                ActionType = actionType,
                DateCreatedUtc = SystemTime.UtcNow,
                Data = data,
                UserId = _currentUser.GetUserId()
            });

            if (saveChanges) await _db.SaveChangesAsync();
        }

        public async Task<bool> ReachedActionThreshold(UserActionType actionType, int number, TimeSpan time, bool registerEventAndSave = true)
        {
            var greaterThanThreshold = await _db.UserActions.CountAsync(e =>
                e.ActionType == actionType &&
                e.UserId == _currentUser.GetUserId().Value &&
                e.DateCreatedUtc > SystemTime.UtcNow.Subtract(time)) >= number;

            if(!greaterThanThreshold && registerEventAndSave)
            {
                await RegisterAction(actionType);
            }

            return greaterThanThreshold;
        }

        public async Task<bool> ReachedActionThresholdForAnonymous(UserActionType actionType, int number, TimeSpan time, string data, bool registerEventAndSave = true)
        {
            var greaterThanThreshold = await _db.UserActions.CountAsync(e =>
                e.ActionType == actionType &&
                e.Data == data &&
                e.DateCreatedUtc > SystemTime.UtcNow.Subtract(time)) >= number;

            if (!greaterThanThreshold && registerEventAndSave)
            {
                await RegisterAction(actionType, data);
            }

            return greaterThanThreshold;
        }
    }
}
