using AutoMapper;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Framework.Domain;
using System;

namespace GoRegister.ApplicationCore.Domain.Registration.Framework
{
    public class FormDisplayContext
    {
        public readonly IMapper Mapper;
        public readonly UserFormResponse Response;
        public bool IsAdmin { get; }

        public FormDisplayContext(IMapper mapper, UserFormResponse response, bool isAdmin)
        {
            Mapper = mapper;
            Response = response;
            IsAdmin = isAdmin;
        }
    }

    public class FieldDisplayContext
    {
        private readonly int _fieldId;

        public FieldDisplayContext(int fieldId, FormDisplayContext formContext)
        {
            _fieldId = fieldId;
            FormContext = formContext;
        }

        public FormDisplayContext FormContext { get; }

        public Result<string> GetStringValue()
        {
            return FormContext.Response != null
                ? FormContext.Response.GetStringValue(_fieldId)
                : Result.Fail<string>();
        }

        public Result<int> GetFieldOptionId()
        {
            return FormContext.Response != null
                ? FormContext.Response.GetFieldOptionId(_fieldId)
                : Result.Fail<int>();
        }

        public Result<DateTime> GetDateTimeValue()
        {
            return FormContext.Response != null
                ? FormContext.Response.GetDateTimeValue(_fieldId)
                : Result.Fail<DateTime>();
        }
    }
}
