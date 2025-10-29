using System;
using System.Collections.Generic;
using System.Text;

namespace GoRegister.ApplicationCore.Domain.Registration.Fields.Session
{
    public class SessionFieldInputModel
    {
        public List<SessionModel> Sessions { get; set; }
    }

    public class SessionModel
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
    }
}
