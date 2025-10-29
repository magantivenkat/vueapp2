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
    public static class DeclineHandler
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

                var regStatus = (RegistrationStatus)delegateUser.RegistrationStatusId;

                if (regStatus != RegistrationStatus.Invited)
                {
                    
                    return Result.Invalid<Response>("Users can only decline if they are invited for the project");
                }

                // if delegate is not a test user check that registration is open
                if (!delegateUser.IsTest && !_registrationService.CanDecline(delegateUser.RegistrationType))
                {
                    return Result.Invalid<Response>("Declining is not available at this time");
                }

                var formModel = await _formService.GetForm(FormType.Decline);
                var formResponse = await _formService.GetUserResponseModel(formModel.Form.Id, userId);
                var model = await _formService.GetFormDisplayModel(formModel, formResponse.Value);
                response.Model = model;
                return Result.Ok(response);
            }
        }
    }
}
