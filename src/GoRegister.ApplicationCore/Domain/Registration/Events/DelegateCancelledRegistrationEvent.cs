using GoRegister.ApplicationCore.Data.Models;
using MediatR;

namespace GoRegister.ApplicationCore.Domain.Registration.Events
{
    public class DelegateCancelledRegistrationEvent : INotification
    {
        public DelegateUser DelegateUser { get; }

        public DelegateCancelledRegistrationEvent(DelegateUser delegateUser)
        {
            DelegateUser = delegateUser;
        }
    }
}
