using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Domain.Delegates;
using GoRegister.ApplicationCore.Domain.Registration.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoRegister.ApplicationCore.Domain.Registration.Models
{
    public class DelegateLiquidQueryModel : IDelegateUserCache, IAttendeeModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int RegistrationTypeId { get; set; }
        public string RegistrationType { get; set; }
        public string Email { get; set; }
        public string RegistrationDocument { get; set; }
        public RegistrationStatus RegistrationStatus { get; set; }
        public Guid UniqueIdentifier { get; set; }
    }
}
