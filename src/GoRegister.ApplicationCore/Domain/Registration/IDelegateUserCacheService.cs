using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Delegates;
using GoRegister.ApplicationCore.Domain.Registration.Framework;
using GoRegister.ApplicationCore.Domain.Registration.Models;

namespace GoRegister.ApplicationCore.Domain.Registration
{
    public interface IDelegateUserCacheService
    {
        DelegateDataTagAccessor Get(DelegateUser delegateUser);
        DelegateDataTagAccessor Get(IAttendeeModel delegateUser);
    }

    public class DelegateUserCacheService : IDelegateUserCacheService
    {
        private readonly ApplicationDbContext _context;
        private readonly IFieldDriverAccessor _fieldDriverAccessor;
        private readonly IDataTagMappingCache _dataTagCache;

        public DelegateUserCacheService(ApplicationDbContext context, IFieldDriverAccessor fieldDriverAccessor, IDataTagMappingCache dataTagCache)
        {
            _context = context;
            _fieldDriverAccessor = fieldDriverAccessor;
            _dataTagCache = dataTagCache;
        }

        public DelegateDataTagAccessor Get(DelegateUser delegateUser)
        {
            var data = new DelegateDataTagAccessor(delegateUser, _dataTagCache.Get(), _fieldDriverAccessor, _context);
            data.FirstName = delegateUser.ApplicationUser.FirstName;
            data.LastName = delegateUser.ApplicationUser.LastName;
            data.Email = delegateUser.ApplicationUser.Email;
            data.Id = delegateUser.Id;
            data.RegistrationType = delegateUser.RegistrationType.Name;
            data.RegistrationTypeId = delegateUser.RegistrationTypeId;
            data.RegistrationStatus = (Data.Enums.RegistrationStatus)delegateUser.RegistrationStatusId;

            return data;
        }

        public DelegateDataTagAccessor Get(IAttendeeModel delegateUser)
        {
            var data = new DelegateDataTagAccessor(delegateUser, _dataTagCache.Get(), _fieldDriverAccessor, _context);
            data.FirstName = delegateUser.FirstName;
            data.LastName = delegateUser.LastName;
            data.Email = delegateUser.Email;
            data.Id = delegateUser.Id;
            data.RegistrationType = delegateUser.RegistrationType;
            data.RegistrationTypeId = delegateUser.RegistrationTypeId;
            data.RegistrationStatus = delegateUser.RegistrationStatus;

            return data;
        }
    }
}
