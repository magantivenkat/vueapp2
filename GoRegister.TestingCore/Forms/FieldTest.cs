using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Registration.Framework;
using GoRegister.ApplicationCore.Domain.Registration.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoRegister.TestingCore.Forms
{
    public abstract class FieldTest<TField> : DatabaseContextTest where TField : Field, new()
    {
        protected DelegateUser DelegateUser { get; set; }
        //protected UserFormResponse FormResponse { get; set; }
        protected UserFormResponse UserFormResponseMRF { get; set; }
        protected FormResponseContext ResponseContext { get; set; }
        protected FormValidationContext ValidationContext { get; set; }
        protected Dictionary<string, JToken> FormData { get; set; }

        protected TField Field { get; set; }

        //public FieldTest() : base()
        //{
        //    Field = new TField();
        //    Field.Id = 1;

        //    DelegateUser = new DelegateUser
        //    {
        //        Id = 1,
        //        ApplicationUser = new ApplicationUser
        //        {
        //            Id = 1,
        //        }
        //    };

        //    UserFormResponse = new UserFormResponse();
        //    UserFormResponse.DelegateUser = DelegateUser;

        //    //FormResponse = new UserFormResponse
        //    //{
        //    //    DelegateUser = DelegateUser
        //    //};

        //    ResponseContext = new FormResponseContext();
        //    //ResponseContext.Response = UserFormResponse;
        //    ResponseContext.Response = UserFormResponseMRF;
        //    ResponseContext.FieldRuleExecutor = new FieldRuleExecutor(new Dictionary<FieldTypeEnum, IFormDriver>(), new List<FormRuleModel>());

        //    //ResponseContext = new FormResponseContext
        //    //{
        //    //    Response = UserFormResponse,
        //    //    FieldRuleExecutor = new FieldRuleExecutor(new Dictionary<FieldTypeEnum, IFormDriver>(), new List<FormRuleModel>())
        //    //};

        //    FormData = new Dictionary<string, JToken>();
        //    ValidationContext = new FormValidationContext();
        //}

        public FieldTest() : base()
        {
            Field = new TField();
            Field.Id = 1;

            DelegateUser = new DelegateUser
            {
                Id = 1,
                ApplicationUser = new ApplicationUser
                {
                    Id = 1,
                }
            };

            UserFormResponseMRF = new UserFormResponse();
            UserFormResponseMRF.DelegateUser = DelegateUser;

            //FormResponse = new UserFormResponse
            //{
            //    DelegateUser = DelegateUser
            //};

            ResponseContext = new FormResponseContext();
            //ResponseContext.Response = UserFormResponse;
            ResponseContext.UserFormResponseMRF = UserFormResponseMRF;
            ResponseContext.FieldRuleExecutor = new FieldRuleExecutor(new Dictionary<FieldTypeEnum, IFormDriver>(), new List<FormRuleModel>());

            //ResponseContext = new FormResponseContext
            //{
            //    Response = UserFormResponse,
            //    FieldRuleExecutor = new FieldRuleExecutor(new Dictionary<FieldTypeEnum, IFormDriver>(), new List<FormRuleModel>())
            //};

            FormData = new Dictionary<string, JToken>();
            ValidationContext = new FormValidationContext();
        }

        protected void AddFormResponse(object value)
        {
            FormData.Add(Field.GetRenderId(), JToken.FromObject(value));
        }
    }
}
