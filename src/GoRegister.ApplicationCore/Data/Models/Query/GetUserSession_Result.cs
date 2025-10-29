using System;
using System.Collections.Generic;
using System.Text;

namespace GoRegister.ApplicationCore.Data.Models.Query
{
    public class GetUserSession_Result
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string MeetingLink { get; set; }
        public DateTime DateStartUtc { get; set; }
        public DateTime DateEndUtc { get; set; }
        public DateTime DateCloseRegistrationUtc { get; set; }
    }
}
