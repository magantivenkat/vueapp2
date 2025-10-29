using GoRegister.ApplicationCore.Domain.Delegates.Models;

namespace GoRegister.ApplicationCore.Domain.Delegates
{
    public static class AttendeeUserWrapperExtensions
    {
        public static AttendeeAuthorizationModel GetAuthModel(this AttendeeUserWrapper wrapper)
        {
            if (wrapper.IsAnonymous) return AttendeeAuthorizationModel.CreateAnonymous();

            return new AttendeeAuthorizationModel
            {
                RegistrationStatus = wrapper.Value.RegistrationStatus,
                RegistrationTypeId = wrapper.Value.RegistrationTypeId
            };
        }
    }
}
