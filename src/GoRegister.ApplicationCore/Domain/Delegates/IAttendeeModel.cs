using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Domain.Registration.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoRegister.ApplicationCore.Domain.Delegates
{
    public interface IAttendeeModel : IDelegateUserCache
    {
        string FirstName { get; set; }
        string LastName { get; set; }
        int RegistrationTypeId { get; set; }
        string RegistrationType { get; set; }
        RegistrationStatus RegistrationStatus { get; set; }
        string Email { get; set; }
    }
}
