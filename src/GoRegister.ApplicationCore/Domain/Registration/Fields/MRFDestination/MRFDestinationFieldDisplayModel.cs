using GoRegister.ApplicationCore.Domain.Registration.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoRegister.ApplicationCore.Domain.Registration.Fields.MRFDestination
{
    public class MRFDestinationFieldDisplayModel : BaseSingleFieldDisplayModel, IFieldDisplayModel<string>
    {
        public string Value { get; set; }
    }
}
