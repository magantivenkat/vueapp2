using Fluid;
using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Liquid;
using GoRegister.ApplicationCore.Domain.Registration;
using GoRegister.ApplicationCore.Domain.Registration.Services;
using GoRegister.ApplicationCore.Framework.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GoRegister.Features.Register
{
    public static class IndexHandler
    {
        public class Query : IRequest<Response>
        {
            public int? RegistrationTypeId { get; set; }
        }

        public class Response
        {
            public ResponseStatus Status { get; set; }
            public RegistrationStatusModel RegistrationStatusModel { get; set; }
            public FormModel RegisterModel { get; set; }
        }

        public class RegistrationStatusModel
        {
            public ApplicationCore.Data.Enums.RegistrationStatus RegistrationStatus { get; set; }
            public string DisplayText { get; set; }
            public bool CanCancel { get; set; }
        }

        public enum ResponseStatus
        {
            NotFound,
            RegistrationTypeNotFound,
            RegistrationTypeSelectRequired,
            DisplayRegistrationStatusPage,
            RegistrationClosed,
            DisplayRegistration
        }

        public class QueryHandler : IRequestHandler<Query, Response>
        {
            private readonly ApplicationDbContext _db;
            private readonly ICurrentUserAccessor _currentUserAccessor;
            private readonly IDelegateUserCacheService _delegateUserCacheService;
            private readonly ILiquidTemplateManager _liquidTemplateManager;
            private readonly IRegistrationService _registrationService;
            private readonly IFormService _formService;

            public QueryHandler(ApplicationDbContext db, ICurrentUserAccessor currentUserAccessor, IDelegateUserCacheService delegateUserCacheService, ILiquidTemplateManager liquidTemplateManager, IRegistrationService registrationService, IFormService formService)
            {
                _db = db;
                _currentUserAccessor = currentUserAccessor;
                _delegateUserCacheService = delegateUserCacheService;
                _liquidTemplateManager = liquidTemplateManager;
                _registrationService = registrationService;
                _formService = formService;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var response = new Response();

                if (_currentUserAccessor.Get.Identity.IsAuthenticated)
                {
                    var userId = _currentUserAccessor.GetUserId().Value;
                    var delegateUser = await _db.Delegates
                        .Include(e => e.ApplicationUser)
                        .Include(e => e.RegistrationType)
                            .ThenInclude(e => e.RegistrationPath)
                        .FirstOrDefaultAsync(e => e.Id == userId);

                    var regStatus = (ApplicationCore.Data.Enums.RegistrationStatus)delegateUser.RegistrationStatusId;
                    if (regStatus == ApplicationCore.Data.Enums.RegistrationStatus.Invited)
                    {
                        // if delegate is not a test user check that registration is open
                        if(!delegateUser.IsTest && !_registrationService.CanRegister(delegateUser.RegistrationType))
                        {
                            response.Status = ResponseStatus.RegistrationClosed;
                            return response;
                        }

                        var formModel = await _formService.GetRegistrationForm();
                        var formResponse = await _formService.GetUserResponseModel(formModel.Form.Id, userId);
                        var model = await _formService.GetFormDisplayModel(formModel, formResponse.Value);
                        response.Status = ResponseStatus.DisplayRegistration;
                        response.RegisterModel = model;

                        return response;
                    }
                    else
                    {
                        var liquidHtml = GetStatusLiquidTemplate(delegateUser);
                        var cachedUser = _delegateUserCacheService.Get(delegateUser);
                        var context = new TemplateContext();
                        context.SetValue("User", cachedUser);
                        var statusText = await _liquidTemplateManager.RenderAsync(liquidHtml, context);

                        response.Status = ResponseStatus.DisplayRegistrationStatusPage;
                        response.RegistrationStatusModel = new RegistrationStatusModel
                        {
                            RegistrationStatus = regStatus,
                            DisplayText = statusText,
                            CanCancel = _registrationService.CanCancel(delegateUser.RegistrationType) && regStatus == ApplicationCore.Data.Enums.RegistrationStatus.Confirmed,
                        };

                        return response;
                    }
                }
                else
                {
                    if (request.RegistrationTypeId == null)
                    {
                        // get all active reg types
                        var regTypes = await _db.RegistrationTypes
                            .Include(e => e.RegistrationPath)
                            .Where(e => e.RegistrationPath.IsActive)
                            .ToListAsync();

                        // check if any allow open registrations
                        //if(!regTypes.Any(e => e.RegistrationPath.AllowOpenRegistration)) {
                        //response.Status = ResponseStatus.NotFound;
                        //return response;
                        //}
                    }
                    else
                    {
                        var regType = await _db.RegistrationTypes.FirstOrDefaultAsync(e => e.Id == request.RegistrationTypeId);
                        if (regType == null)
                        {
                            response.Status = ResponseStatus.RegistrationTypeNotFound;
                            return response;
                        }
                    }


                }

                return null;
            }

            private string GetStatusLiquidTemplate(DelegateUser delegateUser)
            {
                var registrationPath = delegateUser.RegistrationType.RegistrationPath;
                var status = (ApplicationCore.Data.Enums.RegistrationStatus)delegateUser.RegistrationStatusId;
                switch(status)
                {
                    case ApplicationCore.Data.Enums.RegistrationStatus.Confirmed:
                        return registrationPath.ConfirmedText;
                    case ApplicationCore.Data.Enums.RegistrationStatus.NotInvited:
                        return registrationPath.NotInvitedText;
                    case ApplicationCore.Data.Enums.RegistrationStatus.Cancelled:
                        return registrationPath.CancelledText;
                    case ApplicationCore.Data.Enums.RegistrationStatus.Declined:
                        return registrationPath.DeclinedText;
                    case ApplicationCore.Data.Enums.RegistrationStatus.Waiting:
                        return registrationPath.WaitingText;
                }

                return "";
            }
        }
    }
}
