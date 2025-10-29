using System.ComponentModel.DataAnnotations;

namespace GoRegister.ApplicationCore.Data.Enums
{
    public enum RegistrationStatus
    {
        NotInvited,
        Invited,
        Confirmed,
        Declined,
        Cancelled,
        Waiting
    }
}
