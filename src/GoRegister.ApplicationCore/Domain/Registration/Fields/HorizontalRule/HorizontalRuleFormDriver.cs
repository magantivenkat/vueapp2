using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Registration.Framework;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Registration.Fields.HorizontalRule
{
    public class HorizontalRuleFormDriver : PresentationFieldDriver<HorizontalRuleField, FieldEditorModel>
    {
        public override FieldTypeEnum FieldType => FieldTypeEnum.HorizontalRule;
        protected override string EditorName => "Divider";
        protected override string OverrideName => EditorName;

        public override async Task<IFormDriverResult> Display(HorizontalRuleField field, FieldDisplayContext context, int projectId = 0)
        {
            var model = new PresentationFieldDisplayModel();
            model.Build(field);
            model.Type = "divider";
            return new FormDriverResult("_RegistrationForm_Divider", model);
        }
    }
}
