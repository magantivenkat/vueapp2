using GoRegister.ApplicationCore.Domain.Registration.Framework;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace GoRegister.ApplicationCore.Domain.Registration
{
    public class FormModel
    {
        public Dictionary<int, FormSchema> FormSchemas { get; set; }
        public FormResponseModel User { get; set; }
        public int FormId { get; set; }
        public bool IsAdmin { get; set; }
        //public LookupDataModel LookupData { get; set; }

    }


        public class FormRazorModel
    {
        public List<IFormDriverResult> DriverResults { get; set; } = new List<IFormDriverResult>();
        public Dictionary<object, object> User { get; set; }
    }

    public class FormSchema
    {
        public bool IsAdmin { get; set; }
        public bool EnableReview { get; set; }
        public bool HideWizard { get; set; }
        public string SubmitButton { get; set; }
        public string ReviewButton { get; set; }
        public List<FormPageDisplayModel> Pages { get; set; }
        public List<IFieldDisplayModel> Fields { get; set; }
    }

    public class FormPageDisplayModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
    }

    public class FormResponseModel
    {
        public int? Id { get; set; }
        public int RegistrationTypeId { get; set; }
        public int? ParentDelegateId { get; set; }
        public bool IsGuest { get; set; }
        public Dictionary<string, JToken> Model { get; set; }

        public Dictionary<string,FormResponseModel> Guests { get; set; } = new Dictionary<string, FormResponseModel>();
    }

    public class FieldResult<T>
    {
        public IFieldDisplayModel Model { get; set; }
        public T Value { get; set; }
    }
}
