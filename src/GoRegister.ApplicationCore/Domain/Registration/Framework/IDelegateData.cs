using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Models;
using GoRegister.ApplicationCore.Domain.Delegates.Models;
using GoRegister.ApplicationCore.Data;
using Microsoft.EntityFrameworkCore;
using GoRegister.ApplicationCore.Data.Enums;

namespace GoRegister.ApplicationCore.Domain.Registration.Framework
{
    public class DelegateData
    {
        public Dictionary<int, object> Get { get; } = new Dictionary<int, object>();

        public DelegateData(IDelegateUserCache user)
        {
            if (!string.IsNullOrWhiteSpace(user.RegistrationDocument))
            {
                Get = JsonConvert.DeserializeObject<Dictionary<int, object>>(user.RegistrationDocument);
            }
        }
    }

    public class DelegateDataTagAccessor
    {
        public IDelegateUserCache User;
        private readonly Dictionary<string, DataTagMapping> _datatagMappings;
        private readonly IFieldDriverAccessor _fieldDriverAccessor;
        public readonly DelegateData Data;
        private readonly ApplicationDbContext _db;

        private readonly Lazy<Task<Dictionary<string, AttendeeCategorySessionModel>>> _sessionFactory;

        public DelegateDataTagAccessor(IDelegateUserCache user, Dictionary<string, DataTagMapping> datatagMappings, IFieldDriverAccessor fieldDriverAccessor, ApplicationDbContext db)
        {
            User = user;
            _datatagMappings = datatagMappings;
            _fieldDriverAccessor = fieldDriverAccessor;
            Data = new DelegateData(user);
            _db = db;

            _sessionFactory = new Lazy<Task<Dictionary<string, AttendeeCategorySessionModel>>>(async () =>
            {
                var sessionsBookedQuery = _db.DelegateSessionBookings
                    .Where(dsb => dsb.DelegateUserId == Id && dsb.DateReleasedUtc == null)
                    .Select(dsb => new AttendeeSessionModel
                    {
                        SessionId = dsb.Session.Id,
                        DateStartUtc = dsb.Session.DateStartUtc,
                        CategoryId = dsb.Session.SessionCategory.Id,
                        CategoryName = dsb.Session.SessionCategory.Name,
                        Description = dsb.Session.Description,
                        MeetingLink = dsb.Session.MeetingLink,
                        MeetingPassword = dsb.Session.MeetingPassword,
                        IsOptional = dsb.Session.IsOptional,
                        Name = dsb.Session.Name
                    });

                var sessionsQuery = _db.Sessions
                    .Where(s => (!s.SessionRegistrationTypes.Any() || s.SessionRegistrationTypes.Select(srt => srt.RegistrationTypeId).Contains(RegistrationTypeId)))
                    .Where(s => !s.IsOptional)
                    .Select(s => new AttendeeSessionModel
                    {
                        SessionId = s.Id,
                        DateStartUtc = s.DateStartUtc,
                        CategoryId = s.SessionCategory.Id,
                        CategoryName = s.SessionCategory.Name,
                        Description = s.Description,
                        MeetingLink = s.MeetingLink,
                        MeetingPassword = s.MeetingPassword,
                        IsOptional = s.IsOptional,
                        Name = s.Name
                    });

                var list = await sessionsQuery.Union(sessionsBookedQuery).ToListAsync();
                return list.GroupBy(e => e.CategoryName).ToDictionary(e => e.Key, e => new AttendeeCategorySessionModel(e.ToList()), StringComparer.OrdinalIgnoreCase);
            });
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int RegistrationTypeId { get; set; }
        public string RegistrationType { get; set; }
        public Data.Enums.RegistrationStatus RegistrationStatus { get; set; }
        public Guid UniqueIdentifier => User.UniqueIdentifier;
        public async Task<Dictionary<string, AttendeeCategorySessionModel>> GetSessions() => await _sessionFactory.Value;
        public Task<Dictionary<string, AttendeeCategorySessionModel>> Sessions => _sessionFactory.Value;

        public async Task<object> GetValue(string key)
        {
            var keyToCompare = key.ToUpperInvariant();
            if (!_datatagMappings.ContainsKey(keyToCompare)) return null;
            var fi = _datatagMappings[keyToCompare];
            if (!Data.Get.ContainsKey(fi.FieldId)) return null;
            // should just the value be satisfactory here?
            var context = new DelegateUserCacheGetContext() { DelegateData = this };
            var driver = _fieldDriverAccessor.GetFormDriver(fi.FieldType);
            return await driver.GetCachedValueAsync(fi.FieldId, context);
        }

        public object GetResponseValue(int fieldId)
        {
            if (!Data.Get.ContainsKey(fieldId)) return null;
            return Data.Get[fieldId];
        }

        //public Task<object> this[string key] => Task.Run(async () =>
        //{
        //    // check the corresponding id for the provided data tag
        //    var keyToCompare = key.ToUpperInvariant();
        //    if (!_datatagMappings.ContainsKey(keyToCompare)) return null;
        //    var fi = _datatagMappings[keyToCompare];
        //    if (!_data.Get.ContainsKey(fi.FieldId)) return null;
        //    // should just the value be satisfactory here?
        //    var context = new DelegateUserCacheGetContext() { DelegateData = this, Value = _data.Get[fi.FieldId] };
        //    var driver = _fieldDriverAccessor.GetFormDriver(fi.FieldType);
        //    return await driver.GetCachedValueAsync(context);
        //});
    }
}
