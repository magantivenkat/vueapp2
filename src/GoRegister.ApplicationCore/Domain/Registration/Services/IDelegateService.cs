/*  MRF Changes : Set registration status of Delegate user as Invited 
    Modified Date : 29th September 2022
    Modified By : Mandar.Khade @amexgbt.com
    Team member : Harish.Rame @amexgbt.com
    JIRA Ticket No : GoRegister/GOR-234 , GOR-236  */

using AutoMapper;
using AutoMapper.QueryableExtensions;
using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Delegates;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Models;
using GoRegister.ApplicationCore.Domain.Registration.Models;
using GoRegister.ApplicationCore.Framework.Domain;
using GoRegister.ApplicationCore.Framework.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Registration.Services
{
    public interface IDelegateService
    {
        Task<List<DelegateListItemModel>> GetList();

        Task<DelegateCreateModel> GetById(int id);

        Task<DelegateCreateModel> GetCreateModelAsync();

        Task<DelegateCreateModel> GetCreateModelAsync(DelegateCreateModel model);

        Task<Result<int>> CreateAsync(DelegateCreateModel model);

        Task<Result<int>> DeleteDelegate(int id);
        //Task<Result<bool>> DeleteDelegate(int id);
    }

    public class DelegateService : IDelegateService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFormService _formService;
        private readonly IAttendeeIdentifierService _attendeeIdentifierService;
        private readonly IRegistrationLinkService _registrationLinkService;

        public DelegateService(ApplicationDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager, IFormService formService, IAttendeeIdentifierService attendeeIdentifierService, IRegistrationLinkService registrationLinkService)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _formService = formService;
            _attendeeIdentifierService = attendeeIdentifierService;
            _registrationLinkService = registrationLinkService;
        }

        public async Task<List<DelegateListItemModel>> GetList()
        {
            var users = await _context.Delegates
                .Include(e => e.ApplicationUser)
                .Include(rs => rs.RegistrationStatus)
                .Include(rt => rt.RegistrationType)
                .AsNoTracking()
                .ToListAsync();

            var delegates = _mapper.Map<List<DelegateListItemModel>>(users);

            return delegates;
        }

        public async Task<DelegateCreateModel> GetById(int id)
        {
            var model = await _context.Delegates.Where(s => s.Id == id)
                .ProjectTo<DelegateCreateModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();


            var registrationTypes = await _context.RegistrationTypes.ToListAsync();

            model.HasMultipleRegistrationTypes = registrationTypes.Count > 1;

            if (model.HasMultipleRegistrationTypes)
            {
                model.RegistrationTypeSelectList = new SelectList(registrationTypes, "Id", "Name");
            }
            else
            {
                model.RegistrationTypeId = registrationTypes.FirstOrDefault().Id;
            }

            return model; // _mapper.Map<DelegateListItemModel>(user);
        }

        public async Task<DelegateCreateModel> GetCreateModelAsync()
        {
            return await GetCreateModelAsync(new DelegateCreateModel());
        }

        public async Task<DelegateCreateModel> GetCreateModelAsync(DelegateCreateModel model)
        {
            var registrationTypes = await _context.RegistrationTypes.ToListAsync();

            model.HasMultipleRegistrationTypes = registrationTypes.Count > 1;

            // set up all the dropdowns
            if (model.HasMultipleRegistrationTypes)
            {
                model.RegistrationTypeSelectList = new SelectList(registrationTypes, "Id", "Name");
            }
            else
            {
                model.RegistrationTypeId = registrationTypes.FirstOrDefault().Id;
            }

            return model;
        }

        public async Task<Result<int>> CreateAsync(DelegateCreateModel model)
        {
            // validate
            // Email unique etc.

            //var delegateUser = _mapper.Map<DelegateUser>(model);
            var delegateUser = new DelegateUser
            {
                RegistrationTypeId = model.RegistrationTypeId,
                UniqueIdentifier = Guid.NewGuid(),
                //GOR-236 changes
                //IsTest = model.IsTest
                IsTest = true
            };

            delegateUser.MRFClientUserInvitationlink = _registrationLinkService.GetInvitationLink(delegateUser.UniqueIdentifier).Result;

            var userName = Guid.NewGuid().ToString();
            var applicationUser = new ApplicationUser
            {
                DelegateUser = delegateUser,
                UserName = userName,
                NormalizedUserName = userName.ToUpperInvariant(),
                SecurityStamp = GetSecurityStamp()
            };
            delegateUser.ApplicationUser = applicationUser;
            delegateUser.AttendeeNumber = _attendeeIdentifierService.CreateAttendeeNumber();

            // build a registration form
            var formModel = new FormBuilderModel
            {
                Fields = new List<Field>
                {
                    new FirstNameField { Id = 1, FieldTypeId = FieldTypeEnum.FirstName, IsStandardField = true },
                    new LastNameField { Id = 2, FieldTypeId = FieldTypeEnum.LastName , IsStandardField = true  },
                    new EmailField { Id = 3, FieldTypeId = FieldTypeEnum.Email , IsStandardField = true  }
                },
                IsAdmin = true
            };

            var form = await _context.Forms.FirstOrDefaultAsync(e => e.FormTypeId == FormType.Registration);
            var userFormResponse = new UserFormResponse();
            userFormResponse.FormId = form?.Id ?? 0; //TODO: remove this debug hardcode
            userFormResponse.DelegateUser = delegateUser;
            var responseModel = new UserFormResponseModel(userFormResponse);

            var userData = new Dictionary<string, JToken>();
            userData.Add("1", JToken.FromObject(model.FirstName));
            userData.Add("2", JToken.FromObject(model.LastName));
            userData.Add("3", JToken.FromObject(model.Email));

            var formDataModel = new FormModel
            {
                IsAdmin = true,
                User = new FormResponseModel
                {
                    RegistrationTypeId = model.RegistrationTypeId,
                    Model = userData
                }
            };

            var validationContext = await _formService.ProcessFormResponse(formModel, responseModel, formDataModel);

            if(!validationContext.IsValid)
            {
                return Result.Fail<int>();
            }

            var serializedForm = _formService.SerializeForm(responseModel.Response, formModel);
            delegateUser.RegistrationDocument = serializedForm;
            //Commented by Mandar Khade for MRF Requirement : GOR-234 Hide registration status dropdown 
            //delegateUser.ChangeRegistrationStatus(Data.Enums.RegistrationStatus.NotInvited);
            delegateUser.ChangeRegistrationStatus(Data.Enums.RegistrationStatus.Invited);
            delegateUser.HasBeenModified();
            //add audit record
            if(form != null)
            {
                var audit = responseModel.Response.GetAudit(ActionedFrom.AdminForm, delegateUser.ApplicationUser);
                _context.DelegateAudits.Add(audit);
            }
            _context.Delegates.Add(delegateUser);


            await _context.SaveChangesAsync();

            return Result.Ok(delegateUser.Id);
        }

        private string GetSecurityStamp()
        {
            byte[] bytes = new byte[20];
            RandomNumberGenerator.Fill(bytes);
            return Base32.ToBase32(bytes);
        }

       
        public async Task<Result<int>> DeleteDelegate(int id)
        {
            var delUser = await _context.Delegates
            .Include(x => x.ApplicationUser)
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync();
            _context.Remove(delUser);
            _context.Remove(delUser.ApplicationUser);
            await _context.SaveChangesAsync();
            return Result.Ok(delUser.Id);

        }
    }
}
