using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoRegister.ApplicationCore.Domain.Delegates.Models
{
    public class AttendeeSessionModel
    {
        public int SessionId { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string MeetingLink { get; set; }
        public string MeetingPassword { get; set; }
        public DateTime DateStartUtc { get; set; }
        public DateTime DateEndUtc { get; set; }
        public DateTime DateCloseRegistrationUtc { get; set; }
        public bool IsOptional { get; set; }
    }

    public class AttendeeCategorySessionModel // : List<AttendeeSessionModel>
    {
        private readonly AttendeeSessionModel _first;
        

        public AttendeeCategorySessionModel(IEnumerable<AttendeeSessionModel> attendeeSessionModels)// : base(attendeeSessionModels)
        {
            _first = attendeeSessionModels.FirstOrDefault();
            List = attendeeSessionModels;
        }

        public string Name => _first?.Name;
        public int? SessionId => _first?.SessionId;
        public int? CategoryId => _first?.CategoryId;
        public string CategoryName => _first?.CategoryName;
        public string Description => _first?.Description;
        public string MeetingLink => _first?.MeetingLink;
        public string MeetingPassword => _first?.MeetingPassword;
        public DateTime? DateStartUtc => _first?.DateStartUtc;
        public DateTime? DateEndUtc => _first?.DateEndUtc;
        public DateTime? DateCloseRegistrationUtc => _first?.DateCloseRegistrationUtc;

        public IEnumerable<AttendeeSessionModel> List { get; private set; }
    }
}
