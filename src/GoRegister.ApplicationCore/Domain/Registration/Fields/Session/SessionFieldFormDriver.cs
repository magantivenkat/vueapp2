using GoRegister.ApplicationCore.Domain.Registration.Framework;
using System;
using System.Threading.Tasks;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Translators;
using OfficeOpenXml;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Models;
using GoRegister.ApplicationCore.Domain.Sessions.Services;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using GoRegister.ApplicationCore.Extensions;
using System.Collections.Generic;
using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Builder;

namespace GoRegister.ApplicationCore.Domain.Registration.Fields.Session
{
    public class SessionFieldFormDriver : InputFormDriverBase<SessionField, FieldEditorModel, List<SessionModel>, SessionFieldDisplayModel>
    {
        private readonly IUrlHelper _urlHelper;
        private readonly ISessionAccessor _sessionAccessor;
        private readonly ISessionBookingService _sessionBookingService;

        public SessionFieldFormDriver(IUrlHelper urlHelper, ISessionAccessor sessionAccessor, ISessionBookingService sessionBookingService)
        {
            _urlHelper = urlHelper;
            _sessionAccessor = sessionAccessor;
            _sessionBookingService = sessionBookingService;
        }

        public override Data.Enums.FieldTypeEnum FieldType => Data.Enums.FieldTypeEnum.Session;
        public override IFormResponseTranslator<List<SessionModel>> FormResponseTranslator => new ObjectFormResponseTranslator<List<SessionModel>>();

        public override DataStorageStrategyEnum StorageStrategy => throw new NotImplementedException();

        protected override List<FormType> FormTypes => new List<FormType> { FormType.Registration };

        public override async Task<SessionFieldDisplayModel> Display(SessionFieldDisplayModel model, SessionField field, FieldDisplayContext context, int projectId = 0)
        {
            model.Build(field);
            model.Type = "Sessions";
            model.ReserveSessionUrl = context.FormContext.IsAdmin ?
                $"/admin/project/{field.ProjectId}/sessions/reserve" :
                _urlHelper.Action("Reserve", "Sessions");
            model.IsAdmin = context.FormContext.IsAdmin;

            var sessions = await _sessionAccessor.GetSessionsForCategories(field.GetCategoryIds());

            model.Sessions = sessions.Select(s => new SessionItem
            {
                Id = s.Id,
                Name = s.Name,
                DateStart = s.DateStartUtc.ConvertToUserProfileTimeZone(s.Project.Timezone).PrettyDate(), //
                DateEnd = s.DateEndUtc.ConvertToUserProfileTimeZone(s.Project.Timezone).PrettyDate(), //
                Description = s.Description,
                SessionCategoryId = s.SessionCategory?.Id,
                SessionCategoryName = s.SessionCategory?.Name,
                RegTypeIds = s.SessionRegistrationTypes.Select(rt => rt.RegistrationTypeId).ToList(),
                IsSingleSession = s.SessionCategory?.IsSingleSession ?? false,
                IsFull = IsSessionFull(s)
            }).OrderBy(s => s.DateStart);

            return await Task.FromResult(model);
        }

        private bool IsSessionFull(Data.Models.Session s)
        {
            if (!s.IsOptional) return true;
            if (s.DelegateSessionBookings.Count == s.Capacity + s.CapacityReserved) return true;

            return false;
        }

        //protected override void Process(SessionField field, ResponseResult<List<SessionModel>> response, FieldResponseContext context)
        //{
        //    if (response.HasValue)
        //    {
        //        var user = context.FormContext.Response.DelegateUser;
        //        foreach (var session in response.Value)
        //        {
        //            if (session.IsDeleted) return;
        //            _sessionBookingService.AddDelegate(session, user);
        //        }
        //    }
        //}

        protected override void Process(SessionField field, ResponseResult<List<SessionModel>> responseMRF, FieldResponseContext context)
        {
            if (responseMRF.HasValue)
            {
                var user = context.FormContext.UserFormResponseMRF.DelegateUser;
                foreach (var session in responseMRF.Value)
                {
                    if (session.IsDeleted) return;
                    _sessionBookingService.AddDelegate(session, user);
                }
            }
        }

        public override ResponseResult<List<SessionModel>> GetExcelResponse(SessionField field, ExcelRange excelRange)
        {
            throw new NotImplementedException();
        }

        //public override async Task<object> BuildUserResponsesAsync(UserResponseContext context)
        //{
        //    return await _sessionAccessor.GetDelegateSessions(context.Response.DelegateUser.Id);
        //}

        public override async Task<object> BuildUserResponsesAsync(UserResponseContext contextMRF)
        {
            return await _sessionAccessor.GetDelegateSessions(contextMRF.UserFormResponseMRF.DelegateUser.Id);
        }

        //public override Task ValidateAsync(SessionField field, ResponseResult<List<SessionModel>> response, FormValidationContext context, FieldResponseContext responseContext)
        //{
        //    if (field.IsMandatory && (!response.HasValue || response.Value.Count == 0))
        //    {
        //        context.AddError("Please join a session");
        //        return Task.CompletedTask;
        //    }

        //    return Task.CompletedTask;
        //}

        public override Task ValidateAsync(SessionField field, ResponseResult<List<SessionModel>> responseMRF, FormValidationContext contextMRF, FieldResponseContext responseContext)
        {
            if (field.IsMandatory && (!responseMRF.HasValue || responseMRF.Value.Count == 0))
            {
                contextMRF.AddError("Please join a session");
                return Task.CompletedTask;
            }

            return Task.CompletedTask;
        }

        //protected override async Task<string> GetSummaryValueAsync(SessionField field, DelegateUserCacheGetContext context)
        //{
        //    var sessions = await context.DelegateData.GetSessions();
        //    var allSessions = sessions.SelectMany(e => e.Value.List).Where(e => e.IsOptional);
        //    return string.Join(", ", allSessions.Select(s => s.Name));
        //}

        protected override async Task<string> GetSummaryValueAsync(SessionField field, DelegateUserCacheGetContext contextMRF)
        {
            var sessions = await contextMRF.DelegateData.GetSessions();
            var allSessions = sessions.SelectMany(e => e.Value.List).Where(e => e.IsOptional);
            return string.Join(", ", allSessions.Select(s => s.Name));
        }
    }
}
