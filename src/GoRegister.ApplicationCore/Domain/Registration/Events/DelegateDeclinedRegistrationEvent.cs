using GoRegister.ApplicationCore.Data.Models;
using MediatR;

namespace GoRegister.ApplicationCore.Domain.Registration.Events
{
    public class DelegateDeclinedRegistrationEvent : INotification
    {
        public DelegateUser DelegateUser { get; }

        public DelegateDeclinedRegistrationEvent(DelegateUser delegateUser)
        {
            DelegateUser = delegateUser;
        }
    }
}
