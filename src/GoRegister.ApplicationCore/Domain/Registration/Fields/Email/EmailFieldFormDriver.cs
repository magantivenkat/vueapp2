using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Registration.Framework;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Translators;
using System.Threading.Tasks;
using GoRegister.ApplicationCore.Data.Enums;
using OfficeOpenXml;
using GoRegister.ApplicationCore.Domain.Registration.Fields.Text;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Models;
using System.ComponentModel.DataAnnotations;
using GoRegister.ApplicationCore.Data;
using Microsoft.EntityFrameworkCore;
using GoRegister.ApplicationCore.Domain.Settings.Services;
using Microsoft.CodeAnalysis;

namespace GoRegister.ApplicationCore.Domain.Registration.Fields.Email
{
    public class EmailFieldFormDriver : FormDriverBase<EmailField, string>
    {
        private readonly ApplicationDbContext _db;
        private readonly IProjectSettingsAccessor _projectSettingsAccessor;

        public EmailFieldFormDriver(ApplicationDbContext db, IProjectSettingsAccessor projectSettingsAccessor)
        {
            _db = db;
            _projectSettingsAccessor = projectSettingsAccessor;
        }

        protected override string OverrideName => "Email";
        protected override string DataTagFixed => "Email";
        public override FieldTypeEnum FieldType => FieldTypeEnum.Email;
        protected override bool IsUnique => true;

        public override IFormResponseTranslator<string> FormResponseTranslator => new StringFormResponseTranslator();

        public override DataStorageStrategyEnum StorageStrategy => DataStorageStrategyEnum.String;

        //public override async Task<IFormDriverResult> Display(EmailField field, FieldDisplayContext context)
        //{
        //    var model = new TextFieldDisplayModel();
        //    model.Build(field);
        //    model.Type = "text-input";
        //    model.InputType = "email";
        //    model.AddValidation("email", true);
        //    model.AddValidation("required", true);

        //    return new FormDriverResult("_RegistrationFormTextField_New", model);
        //}

        public override async Task<IFormDriverResult> Display(EmailField field, FieldDisplayContext fieldDisplayContextMRF, int projectId=0)
        {
            var model = new TextFieldDisplayModel();
            model.Build(field);
            model.Type = "text-input";
            model.InputType = "email";
            model.AddValidation("email", true);
            model.AddValidation("required", true);

            return new FormDriverResult("_RegistrationFormTextField_New", model);
        }

        //public async override Task ValidateAsync(EmailField field, ResponseResult<string> response, FormValidationContext context, FieldResponseContext responseContext)
        //{
        //    // generally this should be redundant as we'll always make the email field mandatory but that property could be manually changed in the db
        //    if (!response || string.IsNullOrWhiteSpace(response.Value))
        //    {
        //        context.AddError("Please provide a valid email address");
        //        return;
        //    }

        //    var normalizedEmail = response.Value.ToUpperInvariant();
        //    //if the email has not changed let's carry on
        //    if (normalizedEmail == responseContext.FormContext.Response.DelegateUser.ApplicationUser.NormalizedEmail) return;

        //    if (!new EmailAddressAttribute().IsValid(response.Value))
        //    {
        //        context.AddError("Please provide a valid email address");
        //        return;
        //    }

        //    // for bulk upload we will run this check beforehand
        //    if (responseContext.FormContext.FormExecutionFrom != FormExecutionFrom.BulkUpload)
        //    {
        //        var settings = await _projectSettingsAccessor.GetAsync();
        //        // only validate email is unique if site doesn't allow duplicate email addresses and this is not a test user
        //        if (!responseContext.FormContext.Response.DelegateUser.IsTest &&
        //            !settings.AllowDuplicateEmails &&
        //            (await _db.Delegates.AnyAsync(e => e.ApplicationUser.NormalizedEmail == normalizedEmail && !e.IsTest))) // ignore test users, duplicates allowed
        //        {
        //            context.AddError("A user with this email already exists");
        //            return;
        //        }
        //    }
        //}


        public async override Task ValidateAsync(EmailField field, ResponseResult<string> responseMRF, FormValidationContext formValidationContextMRF, FieldResponseContext responseContext)
        {
            // generally this should be redundant as we'll always make the email field mandatory but that property could be manually changed in the db
            if (!responseMRF || string.IsNullOrWhiteSpace(responseMRF.Value))
            {
                formValidationContextMRF.AddError("Please provide a valid email address");
                return;
            }

            var normalizedEmail = responseMRF.Value.ToUpperInvariant();
            //if the email has not changed let's carry on
            if (normalizedEmail == responseContext.FormContext.UserFormResponseMRF.DelegateUser.ApplicationUser.NormalizedEmail) return;

            if (!new EmailAddressAttribute().IsValid(responseMRF.Value))
            {
                formValidationContextMRF.AddError("Please provide a valid email address");
                return;
            }

            // for bulk upload we will run this check beforehand
            if (responseContext.FormContext.FormExecutionFrom != FormExecutionFrom.BulkUpload)
            {
                var settings = await _projectSettingsAccessor.GetAsync();
                // only validate email is unique if site doesn't allow duplicate email addresses and this is not a test user
                if (!responseContext.FormContext.UserFormResponseMRF.DelegateUser.IsTest &&
                    !settings.AllowDuplicateEmails &&
                    (await _db.Delegates.AnyAsync(e => e.ApplicationUser.NormalizedEmail == normalizedEmail && !e.IsTest))) // ignore test users, duplicates allowed
                {
                    formValidationContextMRF.AddError("A user with this email already exists");
                    return;
                }
            }
        }

        //protected override void Process(EmailField field, ResponseResult<string> response, FieldResponseContext context)
        //{
        //    if (response.HasValue)
        //    {
        //        context.FormContext.Responses.DelegateUser.UpdateEmail(response.Value);
        //    }
        //}

        protected override void Process(EmailField field, ResponseResult<string> responseMRF, FieldResponseContext fieldResponseContextMRF)
        {
            if (responseMRF.HasValue)
            {
                fieldResponseContextMRF.FormContext.UserFormResponseMRF.DelegateUser.UpdateEmail(responseMRF.Value);
            }
        }

        public override ResponseResult<string> GetExcelResponse(EmailField field, ExcelRange excelRange)
        {
            // we return a fail here because an email is always required for an upload
            if (string.IsNullOrWhiteSpace(excelRange.Text))
            {
                return ResponseResult.Fail<string>();
            }

            return ResponseResult.Ok(excelRange.Text?.Trim());
        }

        //public override object BuildUserResponses(UserResponseContext context)
        //{
        //    return context.Response.DelegateUser.ApplicationUser.Email;
        //}

        public override object BuildUserResponses(UserResponseContext userResponseContextMRF)
        {
            return userResponseContextMRF.UserFormResponseMRF.DelegateUser.ApplicationUser.Email;
        }

        //protected override Task<string> GetSummaryValueAsync(EmailField field, DelegateUserCacheGetContext context)
        //{
        //    return Task.FromResult(context.DelegateData.Email);
        //}

        protected override Task<string> GetSummaryValueAsync(EmailField field, DelegateUserCacheGetContext delegateUserCacheGetContextMRF)
        {
            //return Task.FromResult(context.DelegateData.Email);
            return Task.FromResult(delegateUserCacheGetContextMRF.DelegateData.Email);
        }
    }
}
