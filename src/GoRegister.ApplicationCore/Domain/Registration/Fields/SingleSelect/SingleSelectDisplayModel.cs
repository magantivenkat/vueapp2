using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Registration.Fields.SingleSelect
{
    public class SingleSelectDisplayModel : BaseSingleFieldDisplayModel
    {
        public int Value { get; set; }
        public string DefaultOption { get; set; }
        public IEnumerable<FieldOptionDisplayModel> Options { get; set; }
        public override string SummaryTemplate { get; set; } = "OptionValueSummary";
    }
}
