using GoRegister.ApplicationCore.Data.Enums;
using System.Collections.Generic;

namespace GoRegister.ApplicationCore.Domain.Delegates.Models
{
    public class ContentAuthorizationModel
    {
        public bool AllowAnonymous { get; set; }
        public IEnumerable<RegistrationStatus> RegistrationStatuses { get; set; } = new List<RegistrationStatus>();
        public IEnumerable<int> RegistrationTypes { get; set; } = new List<int>();

    }
}
