using GoRegister.ApplicationCore.Domain.Registration.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Registration.Fields.TelephoneNumber
{
    public class TelephoneFieldDisplayModel : BaseSingleFieldDisplayModel, IFieldDisplayModel<string>
    {
        public string Value { get; set; }
    }
}
