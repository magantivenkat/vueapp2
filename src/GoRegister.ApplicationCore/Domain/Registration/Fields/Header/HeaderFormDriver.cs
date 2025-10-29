using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Registration.Framework;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Builder;
using Microsoft.CodeAnalysis;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Registration.Fields.Header
{
    public class HeaderFormDriver : PresentationFieldDriver<HeaderField, FieldEditorModel>
    {
        public override FieldTypeEnum FieldType => FieldTypeEnum.Header;
        protected override string EditorName => "Header";
        protected override string EditorTemplate => "HeaderEdit";

        public override async Task<IFormDriverResult> Display(HeaderField field, FieldDisplayContext context, int projectId= 0)
        {
            var model = new HeaderDisplayModel();
            model.Build(field);
            model.Type = "Header";
            model.Title = field.Name;
            return new FormDriverResult("_RegistrationForm_Header", model);
        }
    }
}
