using System;

namespace GoRegister.ApplicationCore.Domain.Sessions.ViewModels
{
    public class SessionModel
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

        public DateTime DateCreatedUtc { get; set; }
        public DateTime DateStartUtc { get; set; }
        public DateTime DateEndUtc { get; set; }
        public DateTime DateCloseRegistrationUtc { get; set; }

        //public Project Project { get; set; }
        public SessionCategoryModel SessionCategory { get; set; }

        public int RegistrationCount { get; set; }
        public int AnonRegistrationCount { get; internal set; }
    }
}
