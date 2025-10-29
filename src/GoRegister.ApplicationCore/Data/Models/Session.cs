using System;
using System.Collections.Generic;

namespace GoRegister.ApplicationCore.Data.Models
{
    public class Session : MustHaveProjectEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int Capacity { get; set; }
        public int CapacityReserved { get; set; }

        public string MeetingLink { get; set; }
        public string MeetingPassword { get; set; }

        public bool OpenForRegistration { get; set; }
        public bool IsOptional { get; set; }

        public string Notes { get; set; }

        public DateTime DateCreatedUtc { get; set; }
        public DateTime DateStartUtc { get; set; }
        public DateTime DateEndUtc { get; set; }
        public DateTime DateCloseRegistrationUtc { get; set; }

        public SessionCategory SessionCategory { get; set; }
        public List<SessionRegistrationType> SessionRegistrationTypes { get; set; }

        // Link table sessions to delegate (to count number of delegates to capacity)
        public List<DelegateSessionBooking> DelegateSessionBookings { get; set; }
        public List<AnonSessionBooking> AnonSessionBookings { get; set; }
    }
}
