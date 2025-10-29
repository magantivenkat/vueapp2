using GoRegister.ApplicationCore.Data.Enums;

namespace GoRegister.ApplicationCore.Domain.Delegates.Models
{
    public struct AttendeeAuthorizationModel
    {
        public bool IsAnonymous { get; set; }
        public RegistrationStatus? RegistrationStatus { get; set; }
        public int? RegistrationTypeId { get; set; }

        public static AttendeeAuthorizationModel CreateAnonymous()
        {
            return new AttendeeAuthorizationModel { IsAnonymous = true };
        }
    }
}
