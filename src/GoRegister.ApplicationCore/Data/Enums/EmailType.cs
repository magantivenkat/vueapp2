using System.ComponentModel.DataAnnotations;

namespace GoRegister.ApplicationCore.Data.Enums
{
    public enum EmailType
    {
        Invitation = RegistrationStatus.NotInvited,

        [Display(Name = "Invitation Reminder")]
        InvitationReminder = RegistrationStatus.Invited,

        Confirmation = RegistrationStatus.Confirmed,

        Decline = RegistrationStatus.Declined,

        Cancellation = RegistrationStatus.Cancelled,

        Waiting = RegistrationStatus.Waiting,

        Custom,

        //[Display(Name = "Final Information")]
        //FinalInformation

    }
}