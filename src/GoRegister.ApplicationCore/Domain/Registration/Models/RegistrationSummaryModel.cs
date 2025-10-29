using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Registration.Models
{
    public class RegistrationSummaryModel
    {
        public int UserId { get; set; }
        public List<RegistrationFieldSummaryModel> Fields { get; set; }
    }

    public class RegistrationFieldSummaryModel
    {
        public string DataTag { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
