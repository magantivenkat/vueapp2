/*  MRF Changes : Share Link functionality - add function to get publish status
    Modified Date : 04nd Nov 2022
    Modified By : Mandar.Khade@amexgbt.com
    Team member : Harish.Rane@amexgbt.com
    JIRA Ticket No : GoRegister/GOR-242  
 */


using AutoMapper;
using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Domain.Registration.Framework;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Builder;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoRegister.ApplicationCore.Domain.Registration.Models;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Models;
using GoRegister.ApplicationCore.Framework.Domain;
using static GoRegister.ApplicationCore.Domain.Registration.Services.RegistrationService;
using GoRegister.ApplicationCore.Domain.Settings.Services;

namespace GoRegister.ApplicationCore.Domain.Registration.Services
{
    public interface IRegistrationService
    {
        Task<DelegateUser> GetUser(int userId);
        Task<PreviewFormBuilderModel> SaveForm(int id, FormEditorUpdateModel model);
        Task<FormEditorModel> BuildEditorModel(int id);
        Task<Result<DelegateUserContainer>> GetDelegateUserForEdit(int userId);
        Task<FormProcessModel> GetRegistrationFormProcessModel();
        Task UpdateRegistrationStatus(DelegateUser user, Data.Enums.RegistrationStatus status, ApplicationUser actionedBy, ActionedFrom actionedFrom);
        Task<DelegateAuditViewModel> BuildAudit(int userId);
        bool CanRegister(RegistrationType registrationType);
        bool CanDecline(RegistrationType registrationType);
        bool CanCancel(RegistrationType registrationType);

        Task<MRFClientRequest> GetMRFClientPublishStatus(int projectId);
    }

    public class RegistrationService : IRegistrationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEnumerable<IFormDriver> _formDrivers;
        private readonly IMapper _mapper;
        private readonly IProjectSettingsAccessor _projectSettingsAccessor;

        public RegistrationService(IEnumerable<IFormDriver> formDrivers, ApplicationDbContext context, IMapper mapper, IProjectSettingsAccessor projectSettingsAccessor)
        {
            _formDrivers = formDrivers;
            _context = context;
            _mapper = mapper;
            _projectSettingsAccessor = projectSettingsAccessor;
        }

        public bool CanRegister(RegistrationType registrationType)
        {
            //TODO: check if we need to add capacity check

            var regPath = registrationType.RegistrationPath;

            // check path is open for registrations
            if (!regPath.IsActive) return false;

            // check reg is open for the current date if applicable
            if (regPath.DateRegistrationFrom.HasValue && regPath.DateRegistrationFrom.Value > SystemTime.UtcNow) return false;

            if (regPath.DateRegistrationTo.HasValue && regPath.DateRegistrationTo.Value < SystemTime.UtcNow) return false;

            return true;
        }

        public bool CanDecline(RegistrationType registrationType)
        {
            var regPath = registrationType.RegistrationPath;
            // check path allows cancelling
            if (!regPath.CanDecline) return false;

            // check reg can be cancelled for the current date if applicable
            if (regPath.DateDeclineTo.HasValue && SystemTime.UtcNow > regPath.DateDeclineTo.Value) return false;

            return true;

        }

        public bool CanCancel(RegistrationType registrationType)
        {
            var regPath = registrationType.RegistrationPath;
            // check path allows cancelling
            if (!regPath.CanCancel) return false;

            // check reg can be cancelled for the current date if applicable
            if (regPath.DateCancelTo.HasValue && SystemTime.UtcNow > regPath.DateCancelTo.Value) return false;

            return true;
        }

        public async Task<DelegateUser> GetUser(int userId)
        {
            return await _context.Delegates
                .Include(e => e.ApplicationUser)
                .Include(e => e.RegistrationType)
                    .ThenInclude(e => e.RegistrationPath)
                .FirstOrDefaultAsync(e => e.Id == userId);
        }
        
        public async Task UpdateRegistrationStatus(DelegateUser user, Data.Enums.RegistrationStatus status, ApplicationUser actionedBy, ActionedFrom actionedFrom)
        {
            if ((int)status == user.RegistrationStatusId) return;

            user.ChangeRegistrationStatus(status);
            var audit = user.GetAudit(actionedFrom, actionedBy);
            await _context.DelegateAudits.AddAsync(audit);
            await _context.SaveChangesAsync();
        }

        public class DelegateUserContainer
        {
            public DelegateUser User { get; set; }
            public DelegateUser ParentUser { get; set; }
            public List<DelegateUser> Guests { get; set; } = new List<DelegateUser>();

            public bool HasGuests => Guests.Any();
            public List<DelegateUser> AllUsers => new List<DelegateUser>(Guests) { User };

            public HashSet<int> GetRegistrationTypeIds()
            {
                var ids = new HashSet<int>()
                {
                    User.RegistrationTypeId
                };

                if (ParentUser != null) ids.Add(ParentUser.RegistrationTypeId);
                foreach (var guest in Guests) ids.Add(guest.RegistrationTypeId);

                return ids;
            }
        }

        public async Task<Result<DelegateUserContainer>> GetDelegateUserForEdit(int userId)
        {
            var users = await _context.Delegates
                .Include(e => e.ApplicationUser)
                .Include(e => e.RegistrationType)
                    .ThenInclude(e => e.RegistrationPath)
                .Where(e => e.Id == userId || e.ParentDelegateUserId == userId)
                .ToListAsync();

            if (!users.Any()) return Result.Fail<DelegateUserContainer>();

            var container = new DelegateUserContainer();
            container.User = users.SingleOrDefault(e => e.Id == userId);

            if (container.User.ParentDelegateUserId != null)
            {
                container.ParentUser = await _context.Delegates
                    .Include(e => e.ApplicationUser)
                    .Include(e => e.RegistrationType)
                        .ThenInclude(e => e.RegistrationPath)
                    .FirstOrDefaultAsync(e => e.Id == container.User.ParentDelegateUserId);
            }

            return Result.Ok(container);
        }

        public async Task<FormProcessModel> GetRegistrationFormProcessModel()
        {
            var model = new FormProcessModel();
            model.Fields = await _context.Fields
                .Include(e => e.FieldOptions)//.ThenInclude("NextFieldOptionRules")
                .Include(e => e.RegistrationTypeFields)
                .Where(e => !e.IsDeleted
                    && e.RegistrationPage.Form.FormTypeId == FormType.Registration)
                .OrderBy(e => e.SortOrder)
                .ToListAsync();

            return model;
        }

        public async Task<FormEditorModel> BuildEditorModel(int id)
        {
            var project = await _projectSettingsAccessor.GetAsync();

            var regTypes = await _context.RegistrationTypes.ToListAsync();
            var regIds = regTypes.Select(regType => regType.Id);
            var pages = await _context.RegistrationPages
                .Where(e => e.FormId == id)
                .OrderBy(e => e.SortOrder)
                .ToListAsync();

            var fields = await _context.Fields
                .Include(e => e.FieldOptions)
                    .ThenInclude(e => e.NextFieldOptionRules)
                .Include(e => e.FieldOptionRules)
                .Include(e => e.RegistrationTypeFields)
                .Include(f => f.RegistrationPage)
                .Where(e => !e.IsDeleted && e.RegistrationPage.FormId == id)
                .OrderBy(e => e.SortOrder)
                .ToListAsync();

            var options = fields.SelectMany(e => e.FieldOptions).ToDictionary(e => e.Id);

            var form = await _context.Forms.FindAsync(id);

            var vm = new FormEditorModel();
            vm.IsProjectLive = project.StatusId == Projects.Enums.ProjectStatus.Live;
            vm.FormName = form.AdminDisplayName;
            vm.IsReviewPageHidden = form.IsReviewPageHidden;
            vm.SubmitButtonText = form.SubmitButtonText;
            vm.FormType = form.FormTypeId;
            vm.RegistrationTypes = regTypes.ToDictionary(r => r.Id, r => new FormEditorModel.RegistrationTypeModel
            {
                Name = r.Name
            });
            vm.Fields = fields.Select(e =>
            {
                var editor = GetFormDriver(e.FieldTypeId).BuildEditor(e, _mapper);
                editor.Rules = e.FieldOptionRules.Select(x => new
                {
                    options[x.FieldOptionId].FieldId,
                    x.FieldOptionId
                }).GroupBy(x => x.FieldId, x => x.FieldOptionId,
                    (key, optionIds) => new FieldRuleEditorModel
                    {
                        FieldId = key.ToString(),
                        OptionIds = optionIds.Select(x => x.ToString())
                    });

                return editor;
            });
            vm.FieldTypes = GetFormDrivers().ToDictionary(e => (int)e.Key, e => e.Value.GetEditorTypeModel());
            vm.Pages = pages.Select(e => new FormPageModel
            {
                Id = e.Id.ToString(),
                Fields = fields.Where(f => f.RegistrationPageId == e.Id).Select(e => e.Id.ToString()),
                Name = e.Title
            });

            vm.Id = id;

            return vm;
        }

        public async Task<PreviewFormBuilderModel> SaveForm(int id, FormEditorUpdateModel vm)
        {
            var regTypes = await _context.RegistrationTypes.ToListAsync();
            var regIds = regTypes.Select(regType => regType.Id);
            var fields = await _context.Fields
                .Include(e => e.FieldOptions)
                .Include(e => e.FieldOptionRules)
                .Include(e => e.RegistrationPage)
                .Include(e => e.RegistrationTypeFields)
                .Where(e => !e.IsDeleted && e.RegistrationPage.FormId == id)
                .ToListAsync();

            var form = await _context.Forms.
                Include(e => e.RegistrationPages)
                .Where(e => e.Id == id)
                .FirstOrDefaultAsync();

            var pages = form.RegistrationPages;

            var previewResult = new PreviewFormBuilderModel(form);
            var processedFields = new List<IFieldEditorModel>();

            // update form values
            if (!string.IsNullOrEmpty(vm.Form.FormName))
            {
                form.AdminDisplayName = vm.Form.FormName;
            }
            form.SubmitButtonText = vm.Form.SubmitButtonText;
            form.IsReviewPageHidden = vm.Form.IsReviewPageHidden;
            _context.Update(form);

            // We require here that pages which have been deleted have marked their sub fields as deleted
            foreach (var field in vm.Fields)
            {
                var driver = GetFormDriver((FieldTypeEnum)field.Value<int>("fieldTypeId"));
                // convert the json into the respective editor model
                var fieldModel = driver.BindEditorModel(field);

                if (fieldModel.IsDeleted)
                {
                    if (fieldModel.TryGetId(out var dbFieldId))
                    {
                        var dbField = fields.Single(e => e.Id == dbFieldId);
                        // remove 
                        _context.UserFieldResponses.RemoveRange(
                            (await _context.UserFieldResponses.Where(e => e.FieldId == dbField.Id).ToListAsync())
                        );
                        _context.UserFieldResponseAudits.RemoveRange(
                            (await _context.UserFieldResponseAudits.Where(e => e.FieldId == dbField.Id).ToListAsync())
                        );
                        _context.Remove(dbField);
                        _context.RemoveRange(dbField.FieldOptionRules);
                    }
                }

                processedFields.Add(fieldModel);
            }

            var updateContext = new UpdateFormContext(_mapper);

            int pageSortOrder = 0;
            int fieldSortOrder = 0;
            foreach (var pageVm in vm.Pages)
            {
                RegistrationPage dbPage;
                if (pageVm.TryGetId(out int dbPageId))
                {
                    dbPage = pages.Single(e => e.Id == dbPageId);
                    if (pageVm.IsDeleted)
                    {
                        _context.Remove(dbPage);
                        continue;
                    }
                }
                else
                {
                    if (pageVm.IsDeleted) continue;

                    dbPage = new RegistrationPage
                    {
                        FormId = id,
                        UniqueIdentifier = Guid.NewGuid()
                    };
                    _context.RegistrationPages.Add(dbPage);
                }

                // map page properties
                dbPage.Title = pageVm.Name;
                dbPage.SortOrder = pageSortOrder;

                previewResult.AddPage(dbPage, pageVm.Id);

                // loop through fields in that page and process them
                foreach (var fieldId in pageVm.Fields)
                {
                    UpdateField(fieldId, fieldSortOrder, dbPage);
                    fieldSortOrder++;
                }
            }

            // update field rules
            foreach (var field in processedFields)
            {
                if (field.IsDeleted) continue;
                var ruleOptions = field.Rules.SelectMany(e => e.OptionIds);
                var mappedField = updateContext.FieldMaps[field.Id].Field;
                var mappedRuleOptions = mappedField.FieldOptionRules.Select(e => e.FieldOptionId);
                var fieldOptionRulesToRemove = mappedField.FieldOptionRules.Where(rule => !ruleOptions.Contains(rule.FieldOptionId.ToString()));

                _context.FieldOptionRules.RemoveRange(fieldOptionRulesToRemove);
                foreach (var rule in ruleOptions)
                {
                    if (int.TryParse(rule, out int ruleId))
                    {
                        if (mappedRuleOptions.Contains(ruleId)) continue;
                    }

                    var fo = updateContext.OptionMaps[rule];
                    mappedField.FieldOptionRules.Add(new FieldOptionRule
                    {
                        FieldOption = fo,
                        NextField = mappedField
                    });


                    previewResult.AddRule(fo, mappedField);
                }
            }

            void UpdateField(string fieldId, int sortOrder, RegistrationPage regPage)
            {
                var field = processedFields.FirstOrDefault(e => e.Id == fieldId);
                if (field.IsDeleted) return;
                var driver = GetFormDriver(field.FieldTypeId);
                Field dbField;
                if (field.TryGetId(out int dbFieldId))
                {
                    dbField = fields.Single(e => e.Id == dbFieldId);
                }
                else
                {
                    dbField = driver.CreateInstance();

                  
                    dbField.RegistrationPage = regPage;
                    dbField.UniqueIdentifier = Guid.NewGuid();
                    dbField.RegistrationPage.FormId = id;
                    _context.Fields.Add(dbField);
                }

                previewResult.AddField(dbField, fieldId);


                // map reg types
                if (regIds.Count() == 1)
                {
                    // if one reg type and it has no links already add reg type link
                    //if (!dbField.RegistrationTypeFields.Any())
                    //{
                    //    dbField.RegistrationTypeFields.Add(new RegistrationTypeField
                    //    {
                    //        Field = dbField,
                    //        RegistrationTypeId = regIds.First()
                    //    });
                    //}
                }
                else
                {
                    var deletedRegTypeFields = dbField.RegistrationTypeFields.Where(rtf => !field.RegistrationTypes.Contains(rtf.RegistrationTypeId));
                    _context.RegistrationTypeFields.RemoveRange(deletedRegTypeFields);

                    if (field.RegistrationTypes.Any())
                    {
                        var newRegTypeFields = field.RegistrationTypes
                            .Where(rt => !dbField.RegistrationTypeFields.Select(e => e.RegistrationTypeId).Contains(rt));

                        foreach (var rt in newRegTypeFields)
                        {
                            dbField.RegistrationTypeFields.Add(new RegistrationTypeField { Field = dbField, RegistrationTypeId = rt });
                        }
                    }
                }



                driver.UpdateEditor(dbField, field, updateContext);

                dbField.IsStandardField = (dbField.DataTag == "AdditionalInformation" || dbField.DataTag == "Areyourdatesordestinationflexible?" || dbField.DataTag == "CompanyName" || dbField.DataTag == "Contactphonenumber" || dbField.DataTag == "DestinationExternalId" || dbField.DataTag == "Email" || dbField.DataTag == "EventEndDate" || dbField.DataTag == "EventName" || dbField.DataTag == "EventStartDate" || dbField.DataTag == "FirstName" || dbField.DataTag == "LastName" || dbField.DataTag == "MeetingFormat" || dbField.DataTag == "MeetingType" || dbField.DataTag == "NumberofAttendees" || dbField.DataTag == "ServicingCountry" || dbField.DataTag == "RequestorCountry") ? true : false;

                dbField.SortOrder = sortOrder;

                updateContext.AddFieldMap(field.Id, dbField, field);
            }

            previewResult.PreSaveExecuteActions = updateContext.PreSaveExecuteActions;

            return previewResult;
        }

        public async Task<DelegateAuditViewModel> BuildAudit(int userId)
        {
            var audits = await _context.DelegateAudits
                .Include(e => e.ActionedBy)
                .Include(e => e.UserFieldResponseAudits)
                    .ThenInclude(e => e.Field)
                .Include(e => e.UserFieldResponseAudits)
                    .ThenInclude(e => e.FieldOption)
                .Where(e => e.UserId == userId)
                .ToListAsync();

            var du = await _context.Delegates.Include(e => e.ApplicationUser).FirstOrDefaultAsync(e => e.Id == userId);

            var model = new DelegateAuditViewModel();
            model.Id = userId;
            model.CurrentRegistrationStatus = ((Data.Enums.RegistrationStatus)du.RegistrationStatusId).ToString();
            model.Name = du.ApplicationUser.FirstName + " " + du.ApplicationUser.LastName;

            if (du.InvitedUtc.HasValue)
                model.AddRegistrationStatusAudit("Invited", du.InvitedUtc.Value);

            if (du.ConfirmedUtc.HasValue)
                model.AddRegistrationStatusAudit("Confirmed", du.ConfirmedUtc.Value);

            if (du.DeclinedUtc.HasValue)
                model.AddRegistrationStatusAudit("Declined", du.DeclinedUtc.Value);

            if (du.CancelledUtc.HasValue)
                model.AddRegistrationStatusAudit("Cancelled", du.CancelledUtc.Value);

            var registrationTypes = await _context.RegistrationTypes.ToListAsync();
            var previousValues = new Dictionary<string, string>();

            void UpdatePreviousValue(string key, string value, out string previous)
            {
                if (previousValues.TryGetValue(key, out previous))
                {
                    // if a previous field value exists replace it with new value
                    previousValues[key] = value;
                }
                else
                {
                    previousValues.Add(key, value);
                }
            }

            void AddAudit(string key, string friendlyName, string value, DelegateAuditModel auditModel)
            {
                // last name
                if (!string.IsNullOrWhiteSpace(value))
                {
                    UpdatePreviousValue(key, value, out string previousValue);
                    var fieldAudit = new FieldAuditValue
                    {
                        FieldName = friendlyName,
                        NewValue = value,
                        OldValue = previousValue
                    };
                    auditModel.AddAudit(fieldAudit);
                }
            }

            foreach (var audit in audits.OrderBy(e => e.ActionedUtc))
            {
                var grouped = audit.UserFieldResponseAudits.GroupBy(e => e.Field);
                var auditModel = new DelegateAuditModel();

                auditModel.ActionedBy = audit.ActionedBy != null ?
                    audit.ActionedBy.FirstName + " " + audit.ActionedBy.LastName :
                    "(deleted)";

                auditModel.ActionedOn = audit.ActionedUtc;
                auditModel.ActionedOnFormatted = audit.ActionedUtc.ToString("f");

                // registration type
                if (audit.RegistrationTypeId != null)
                {
                    var newValue = registrationTypes.FirstOrDefault(e => e.Id == audit.RegistrationTypeId)?.Name;
                    UpdatePreviousValue("FirstName", newValue, out string previousValue);
                    var fieldAudit = new FieldAuditValue
                    {
                        FieldName = "Registration Type",
                        NewValue = newValue,
                        OldValue = previousValue
                    };
                    auditModel.FieldAudits.Add(fieldAudit);
                }

                // registration status
                if (audit.RegistrationStatusId != null)
                {
                    var newValue = ((Data.Enums.RegistrationStatus)audit.RegistrationStatusId.Value).ToString();
                    AddAudit("RegistrationStatus", "Registration Status", newValue, auditModel);
                }

                // first name
                AddAudit("FirstName", "First Name", audit.FirstName, auditModel);

                // last name
                AddAudit("LastName", "Last Name", audit.LastName, auditModel);

                // email
                AddAudit("Email", "Email", audit.Email, auditModel);


                foreach (var group in grouped)
                {
                    var driver = GetFormDriver(group.Key.FieldTypeId);

                    foreach (var response in group)
                    {
                        string value = "";
                        switch (driver.StorageStrategy)
                        {
                            case DataStorageStrategyEnum.String:
                                value = response.StringValue;
                                break;
                            case DataStorageStrategyEnum.FieldOption:
                                value = response.FieldOption?.Description ?? "";
                                break;
                            case DataStorageStrategyEnum.DateTime:
                                value = response.DateTimeValue.HasValue ?
                                    response.DateTimeValue.Value.ToShortDateString() :
                                    "";
                                break;
                            default:
                                break;
                        }

                        // field key to store previous values
                        var key = group.Key.Id.ToString();
                        AddAudit(key, group.Key.Name, value, auditModel);
                    }
                }

                model.Audits.Add(auditModel);
            }

            return model;
        }

        public async Task BuildFieldAudit(int userId, int fieldId)
        {
            var field = await _context.Fields.FirstOrDefaultAsync(e => e.Id == fieldId);
            var driver = GetFormDriver(field.FieldTypeId);
            //TODO: get audit per field
        }

        public class DelegateAuditViewModel
        {
            public int Id { get; set; }
            public List<DelegateAuditModel> Audits { get; set; } = new List<DelegateAuditModel>();
            public string CurrentRegistrationStatus { get; set; }
            public List<RegistrationStatusAudit> RegistrationAudits { get; set; } = new List<RegistrationStatusAudit>();
            public string Name { get; internal set; }

            public void AddRegistrationStatusAudit(string name, DateTime dateTime)
            {
                RegistrationAudits.Add(new RegistrationStatusAudit
                {
                    Status = name,
                    ActionedUtc = dateTime,
                    ActionedUtcFormatted = dateTime.ToString("f")
                });
            }

            public class RegistrationStatusAudit
            {
                public string Status { get; set; }
                public DateTime ActionedUtc { get; set; }
                public string ActionedUtcFormatted { get; set; }
            }
        }

        public class DelegateAuditModel
        {
            public List<FieldAuditValue> FieldAudits { get; set; } = new List<FieldAuditValue>();
            public string ActionedBy { get; internal set; }
            public DateTime ActionedOn { get; internal set; }
            public string ActionedOnFormatted { get; internal set; }

            public void AddAudit(FieldAuditValue fieldAudit)
            {
                if (fieldAudit != null)
                {
                    FieldAudits.Add(fieldAudit);
                }
            }
        }

        public class FieldAuditValue
        {
            public string FieldName { get; set; }
            public string OldValue { get; set; }
            public string NewValue { get; set; }
        }

        private IFormDriver GetFormDriver(FieldTypeEnum fieldType)
        {
            return GetFormDrivers()[fieldType];
        }

        private Dictionary<FieldTypeEnum, IFormDriver> GetFormDrivers()
        {
            return _formDrivers.ToDictionary(e => e.FieldType, e => e);
        }

        public async Task<MRFClientRequest> GetMRFClientPublishStatus(int projectId)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(e => e.Id == projectId);

            if (project != null)
            {
                return await _context.MRFClientRequest.FirstOrDefaultAsync(e => e.ClientUuid == project.Prefix);
            }
            else
            {
                return null;
            }
        }
    }
}
