using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Domain.Registration.Services;
using GoRegister.ApplicationCore.Framework.Domain;
using GoRegister.ApplicationCore.Framework.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Registration.Queries
{
    public static class CancelHandler
    {
        public class Query : IRequest<Result<Response>>
        {
        }

        public class Response : IRequest<Result<Unit>>
        {
            public FormModel Model { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, Result<Response>>
        {
            private readonly ApplicationDbContext _db;
            private readonly ICurrentUserAccessor _currentUserAccessor;
            private readonly IRegistrationService _registrationService;
            private readonly IFormService _formService;

            public QueryHandler(ApplicationDbContext db, ICurrentUserAccessor currentUserAccessor, IRegistrationService registrationService, IFormService formService)
            {
                _db = db;
                _currentUserAccessor = currentUserAccessor;
                _registrationService = registrationService;
                _formService = formService;
            }

            public async Task<Result<Response>> Handle(Query request, CancellationToken cancellationToken)
            {
                var response = new Response();

                var userId = _currentUserAccessor.GetUserId().Value;
                var delegateUser = await _db.Delegates
                    .Include(e => e.ApplicationUser)
                    .Include(e => e.RegistrationType)
                        .ThenInclude(e => e.RegistrationPath)
                    .FirstOrDefaultAsync(e => e.Id == userId);

                var regStatus = (ApplicationCore.Data.Enums.RegistrationStatus)delegateUser.RegistrationStatusId;

                if (regStatus != ApplicationCore.Data.Enums.RegistrationStatus.Confirmed)
                {
                    
                    return Result.Invalid<Response>("Users can only cancel if they are confirmed for the project");
                }

                // if delegate is not a test user check that registration is open
                if (!delegateUser.IsTest && !_registrationService.CanCancel(delegateUser.RegistrationType))
                {
                    return Result.Invalid<Response>("Cancellations are not available at this time");
                }

                var formModel = await _formService.GetForm(FormType.Cancel);
                var formResponse = await _formService.GetUserResponseModel(formModel.Form.Id, userId);
                var model = await _formService.GetFormDisplayModel(formModel, formResponse.Value);
                response.Model = model;
                return Result.Ok(response);
            }
        }

        public class ResponseHandler : IRequestHandler<Response, Result<Unit>>
        {
            private readonly ApplicationDbContext _db;
            private readonly ICurrentUserAccessor _currentUserAccessor;
            private readonly IRegistrationService _registrationService;
            private readonly IFormService _formService;

            public ResponseHandler(ApplicationDbContext db, ICurrentUserAccessor currentUserAccessor, IRegistrationService registrationService, IFormService formService)
            {
                _db = db;
                _currentUserAccessor = currentUserAccessor;
                _registrationService = registrationService;
                _formService = formService;
            }

            public async Task<Result<Unit>> Handle(Response request, CancellationToken cancellationToken)
            {
                var model = request.Model;
                var formModel = await _formService.GetRegistrationForm();
                var userId = _currentUserAccessor.GetUserId().Value;
                var userResponseResult = await _formService.GetUserResponseModel(formModel.Form.Id, userId);
                var response = userResponseResult.Value;


                var delegateUser = response.Response.DelegateUser;

                // verify user is confirmed
                if (delegateUser.RegistrationStatusId != (int)ApplicationCore.Data.Enums.RegistrationStatus.Confirmed)
                {
                    return Result.Invalid<Unit>("User cannot be cancelled when they are not yet confirmed");
                }

                // verify user can cancel
                if (!delegateUser.IsTest && !_registrationService.CanCancel(delegateUser.RegistrationType))
                {
                    return Result.Invalid<Unit>("Users can no longer cancel their registrations for this project");
                }

                response.Response.SetupAudit();
                await _formService.ProcessFormResponse(formModel, response, model);
                var serializedForm = _formService.SerializeForm(response.Response, formModel);
                delegateUser.RegistrationDocument = serializedForm;
                delegateUser.ChangeRegistrationStatus(ApplicationCore.Data.Enums.RegistrationStatus.Cancelled);
                delegateUser.HasBeenModified();
                //add audit record
                var audit = response.Response.GetAudit(ActionedFrom.Form, delegateUser.ApplicationUser);
                _db.DelegateAudits.Add(audit);

                await _db.SaveChangesAsync();
                return Result.Ok(Unit.Value);
            }
        }
    }
}
