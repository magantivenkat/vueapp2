using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Registration.Framework;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Builder;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Registration.Fields.Header
{
    public class HtmlFormDriver : PresentationFieldDriver<HtmlField, FieldEditorModel>
    {
        public override FieldTypeEnum FieldType => FieldTypeEnum.Html;
        protected override string EditorName => "Html";
        protected override string EditorTemplate => "HtmlEdit";

        public override async Task<IFormDriverResult> Display(HtmlField field, FieldDisplayContext context, int projectId = 0)
        {
            var model = new HtmlDisplayModel();
            model.Build(field);
            model.Type = "HtmlView";
            model.Title = field.Name;
            return new FormDriverResult("_RegistrationForm_Html", model);
        }

        protected override void Edit(HtmlField field, FieldEditorModel model)
        {
            base.Edit(field, model);
            model.IsForPresentation = true;
        }
    }
}
