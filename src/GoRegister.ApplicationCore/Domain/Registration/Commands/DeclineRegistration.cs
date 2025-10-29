using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Domain.Registration.Events;
using GoRegister.ApplicationCore.Domain.Registration.Services;
using GoRegister.ApplicationCore.Framework.Domain;
using GoRegister.ApplicationCore.Framework.Identity;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Registration.Commands
{
    public static class DeclineRegistration
    {
        public class Command : IRequest<Result<Unit>>
        {
            public FormModel Model { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly ApplicationDbContext _db;
            private readonly ICurrentUserAccessor _currentUserAccessor;
            private readonly IRegistrationService _registrationService;
            private readonly IFormService _formService;
            private readonly IPublisher _publisher;

            public Handler(ApplicationDbContext db, ICurrentUserAccessor currentUserAccessor, IRegistrationService registrationService, IFormService formService, IPublisher publisher)
            {
                _db = db;
                _currentUserAccessor = currentUserAccessor;
                _registrationService = registrationService;
                _formService = formService;
                _publisher = publisher;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var model = request.Model;
                var formModel = await _formService.GetForm(FormType.Decline);
                var userId = _currentUserAccessor.GetUserId().Value;
                var userResponseResult = await _formService.GetUserResponseModel(formModel.Form.Id, userId);
                var response = userResponseResult.Value;
                var delegateUser = response.Response.DelegateUser;

                // verify user is invited
                if (delegateUser.RegistrationStatusId != (int)RegistrationStatus.Invited)
                {
                    return Result.Invalid<Unit>("User cannot decline when they are not yet invited");
                }

                // verify user can decline
                if (!delegateUser.IsTest && !_registrationService.CanDecline(delegateUser.RegistrationType))
                {
                    return Result.Invalid<Unit>("Users can no longer decline their registrations for this event");
                }

                response.Response.SetupAudit();
                await _formService.ProcessFormResponse(formModel, response, model);
                var serializedForm = _formService.SerializeForm(response.Response, formModel);
                delegateUser.DeclineDocument = serializedForm;
                delegateUser.ChangeRegistrationStatus(RegistrationStatus.Declined);
                delegateUser.HasBeenModified();
                //add audit record
                var audit = response.Response.GetAudit(ActionedFrom.Form, delegateUser.ApplicationUser);
                _db.DelegateAudits.Add(audit);
                await _db.SaveChangesAsync();

                //TODO: send cancellation email confirmation ONLY if enabled (what are the rules?)
                await _publisher.Publish(new DelegateDeclinedRegistrationEvent(delegateUser));


                return Result.Ok(Unit.Value);
            }
        }
    }
}
