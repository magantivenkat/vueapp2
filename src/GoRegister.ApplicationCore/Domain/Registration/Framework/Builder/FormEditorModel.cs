using GoRegister.ApplicationCore.Data.Enums;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace GoRegister.ApplicationCore.Domain.Registration.Framework.Builder
{
    public abstract class FormEditorModelBase
    {
        public IEnumerable<FormPageModel> Pages { get; set; }
    }

    public class FormEditorModel : FormEditorModelBase
    {
        public int? Id { get; set; }
        public string FormName { get; set; }
        public IEnumerable<IFieldEditorModel> Fields { get; set; } = Enumerable.Empty<IFieldEditorModel>();
        public Dictionary<int, FieldEditorTypeModel> FieldTypes { get; set; }
        public Dictionary<int, RegistrationTypeModel> RegistrationTypes { get; set; }
        public FormType FormType { get; set; }
        public bool IsReviewPageHidden { get; set; }
        public string SubmitButtonText { get; set; }
        public bool IsProjectLive { get; set; }

        public class RegistrationTypeModel
        {
            public string Name { get; set; }
        }

    }

    public class FormPageModel
    {
        public string Id { get; set; }
        public IEnumerable<string> Fields { get; set; } = new List<string>();
        public bool IsDeleted { get; set; }
        public string Name { get; set; }

        public bool TryGetId(out int id)
        {
            return int.TryParse(Id, out id);
        }
    }

    public class FormEditorUpdateModel : FormEditorModelBase
    {
        public FormUpdateModel Form { get; set; }
        public List<JToken> Fields { get; set; } = new List<JToken>();
    }

    public class FormUpdateModel
    {
        public string FormName { get; set; }
        public bool IsReviewPageHidden { get; set; }
        public string SubmitButtonText { get; set; }
    }

    public class FormEditorPreviewModel : FormEditorUpdateModel
    {
        public int Id { get; set; }
    }
}
