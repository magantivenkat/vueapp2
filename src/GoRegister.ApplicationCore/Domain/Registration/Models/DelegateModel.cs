using System;

namespace GoRegister.ApplicationCore.Domain.Registration.Models
{
    public class DelegateListItemModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name => $"{FirstName} {LastName}";
        public string Email { get; set; }
        public string RegistrationType { get; set; }
        public string RegistrationStatus { get; set; }
        public string Action { get; set; }

        public DateTime InvitedDate { get; set; }

        public DateTime ConfirmedDate { get; set; }

        public DateTime DeclinedDate { get; set; }

        public DateTime CancelledDate { get; set; }

        public DateTime? InvitedUtc { get; set; }
        public DateTime? ConfirmedUtc { get; set; }
        public DateTime? CancelledUtc { get; set; }
        public DateTime? DeclinedUtc { get; set; }
        public DateTime? ModifiedUtc { get; set; }
        public bool IsTest { get; set; }


        public int TotalRegistrations { get; set; }

    }
}
