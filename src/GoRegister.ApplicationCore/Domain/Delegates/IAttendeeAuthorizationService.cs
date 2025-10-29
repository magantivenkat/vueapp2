using GoRegister.ApplicationCore.Domain.Delegates.Models;
using System;
using System.Linq;

namespace GoRegister.ApplicationCore.Domain.Delegates
{
    public interface IAttendeeAuthorizationService
    {
        bool Authorize(AttendeeAuthorizationModel attendeeModel, ContentAuthorizationModel contentModel);
    }

    public class AttendeeAuthorizationService : IAttendeeAuthorizationService
    {
        public bool Authorize(AttendeeAuthorizationModel attendeeModel, ContentAuthorizationModel contentModel)
        {

            // if anonymous attendee then only display if menu item allows for anonymous users
            if (attendeeModel.IsAnonymous)
            {
                return contentModel.AllowAnonymous;
            }

            // if regstatus is empty allow else check regstatus list contains attendee reg status
            var passesRegStatusCheck = !(contentModel.RegistrationStatuses != null && contentModel.RegistrationStatuses.Any())
                || contentModel.RegistrationStatuses.Contains(attendeeModel.RegistrationStatus.Value);

            // if regtypes is empty allow else check regtype list contains attendee reg type id
            var passesRegTypeCheck = !(contentModel.RegistrationTypes != null && contentModel.RegistrationTypes.Any())
                || contentModel.RegistrationTypes.Contains(attendeeModel.RegistrationTypeId.Value);

            // must match both regstatus and regtype check
            return passesRegStatusCheck && passesRegTypeCheck;
        }
    }
}
