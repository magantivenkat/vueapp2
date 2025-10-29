using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Registration.Framework;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Builder;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Registration.Fields.Header
{
    public class SubHeaderFormDriver : PresentationFieldDriver<SubHeaderField, FieldEditorModel>
    {
        public override FieldTypeEnum FieldType => FieldTypeEnum.SubHeader;
        protected override string EditorName => "Sub Header";
        protected override string EditorTemplate => "SubHeaderEdit";

        public override async Task<IFormDriverResult> Display(SubHeaderField field, FieldDisplayContext context, int projectId = 0)
        {
            var model = new SubHeaderDisplayModel();
            model.Build(field);
            model.Type = "SubHeader";
            model.Title = field.Name;
            return new FormDriverResult("_RegistrationForm_SubHeader", model);
        }
    }
}
