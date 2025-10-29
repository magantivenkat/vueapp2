using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Registration.Fields;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Builder;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Registration.Framework
{
    public abstract class InputFormDriverBase<TField, TFieldEditor, TResponse, TFieldDisplay> : FormDriverBase<TField, TFieldEditor, TResponse> 
        where TField : Field, new()
        where TFieldEditor : IFieldEditorModel, new()
        where TFieldDisplay : BaseSingleFieldDisplayModel, new()
    {
        public async override Task<IFormDriverResult> Display(TField field, FieldDisplayContext context, int projectId = 0)
        {
            var model = new TFieldDisplay();
            model.Build(field);
            if(field.IsMandatory)
            {
                model.AddValidation("required", true);
            }

            // remove readonly if admin
            if (context.FormContext.IsAdmin)
            {
                model.Readonly = false;
            }

            model = await Display(model, field, context,projectId);

            if (model == null) return null;

            return new FormDriverResult("", model);
        }

        public abstract Task<TFieldDisplay> Display(TFieldDisplay model, TField field, FieldDisplayContext context, int projectId = 0);
    }

    public abstract class InputFormDriverBase<TField, TResponse, TFieldDisplay> : InputFormDriverBase<TField, FieldEditorModel, TResponse, TFieldDisplay> 
        where TField : Field, new()
        where TFieldDisplay : BaseSingleFieldDisplayModel, new()
    {

    }
}
