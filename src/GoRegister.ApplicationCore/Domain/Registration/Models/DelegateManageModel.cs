using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Registration.Models
{
    public class DelegateManageModel
    {
        public List<DelegateRegistrationAuditModel> Audits { get; set; }
        //public List<DelegateEmailAudit> EmailAudits { get; set; }
    }
}
