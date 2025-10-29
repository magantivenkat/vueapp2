using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Builder;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Registration.Models
{
    public class PreviewFormBuilderModel : IFormBuilderModel
    {
        public PreviewFormBuilderModel(Form form)
        {
            Form = form;
        }

        public Form Form { get; }
        public List<Field> Fields { get; } = new List<Field>();
        public List<FormRuleModel> Rules { get; set; } = new List<FormRuleModel>();
        public List<RegistrationPage> Pages { get; } = new List<RegistrationPage>();
        public bool IsAdmin { get; set; }
        public List<Func<Task>> PreSaveExecuteActions { get; set; }

        public void AddPage(RegistrationPage page, string localId)
        {
            page.SetTempId(localId);
            Pages.Add(page);
        }

        public void AddField(Field field, string localId)
        {
            field.SetTempId(localId);
            Fields.Add(field);
        }

        public void AddRule(FieldOption option, Field nextField)
        {
            Rules.Add(new FormRuleModel
            {
                FieldId = option.Field.GetRenderId(),
                FieldOptionId = option.GetRenderId(),
                NextFieldId = nextField.GetRenderId(),
                IsHidden = option.Field.IsHidden
            });
        }
    }
}
