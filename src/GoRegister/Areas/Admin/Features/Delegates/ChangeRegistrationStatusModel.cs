using GoRegister.ApplicationCore.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.Areas.Admin.Features.Delegates
{
    public class ChangeRegistrationStatusModel
    {
        public int Id { get; set; }
        public RegistrationStatus RegistrationStatus { get; set; }
    }
}
