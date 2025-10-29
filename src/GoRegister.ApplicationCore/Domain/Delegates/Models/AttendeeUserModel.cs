using GoRegister.ApplicationCore.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoRegister.ApplicationCore.Domain.Delegates.Models
{
    public class AttendeeUserModel : IAttendeeModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int RegistrationTypeId { get; set; }
        public RegistrationStatus RegistrationStatus { get; set; }
        public string RegistrationType { get; set; }
        public string RegistrationDocument { get; set; }
        public Guid UniqueIdentifier { get; set; }
    }
}
