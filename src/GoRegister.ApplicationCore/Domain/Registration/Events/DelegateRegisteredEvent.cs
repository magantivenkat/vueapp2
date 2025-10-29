using GoRegister.ApplicationCore.Data.Models;
using MediatR;

namespace GoRegister.ApplicationCore.Domain.Registration.Events
{
    public class DelegateRegisteredEvent : INotification
    {
        public DelegateUser DelegateUser { get; }

        public DelegateRegisteredEvent(DelegateUser delegateUser)
        {
            DelegateUser = delegateUser;
        }
    }
}
