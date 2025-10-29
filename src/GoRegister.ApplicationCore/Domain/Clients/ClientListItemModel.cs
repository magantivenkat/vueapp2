using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Clients
{
    public class ClientListItemModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name => $"{FirstName} {LastName}";
        public string Email { get; set; }
        public string RegistrationType { get; set; }
        public string InvitationList { get; set; }
        public string RegistrationStatus { get; set; }

        public string Action { get; set; }
    }
}
