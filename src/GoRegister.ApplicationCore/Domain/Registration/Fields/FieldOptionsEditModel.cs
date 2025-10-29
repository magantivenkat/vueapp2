using GoRegister.ApplicationCore.Domain.Registration.Framework.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Registration.Fields
{
    public class FieldOptionsEditModel : FieldEditorModel
    {
        public List<OptionEditModel> Options { get; set; } = new List<OptionEditModel>();
        public bool MultiSelect { get; set; } = false;
        public Data.Models.SingleSelectField.SingleSelectTypeEnum SingleSelectType { get; set; } = Data.Models.SingleSelectField.SingleSelectTypeEnum.Radio;

        public class OptionEditModel
        {
            public string Id { get; set; }
            public string Description { get; set; }
            public string InternalInformation { get; set; }
            public string AdditionalInformation { get; set; }
            public int? Capacity { get; set; }
            public bool IsDeleted { get; set; }

            public bool TryGetDatabaseId(out int id) => int.TryParse(Id, out id);
        }
        
    }
}
