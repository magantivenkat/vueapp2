using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Builder;
using System.Collections.Generic;

namespace GoRegister.ApplicationCore.Domain.Registration.Models
{
    public class FormBuilderModel : IFormBuilderModel
    {
        public Form Form { get; set; }
        public List<Field> Fields { get; set; }
        public List<FormRuleModel> Rules { get; set; }
        public List<RegistrationPage> Pages { get; set; }
        public bool IsAdmin { get; set; }
    }

    public class FormRuleModel
    {
        public int Id { get; set; }
        public string FieldOptionId { get; set; }
        public string FieldId { get; set; }
        public string NextFieldId { get; set; }
        public string NextFieldOptionId { get; set; }
        public bool IsHidden { get; set; }
    }
}
