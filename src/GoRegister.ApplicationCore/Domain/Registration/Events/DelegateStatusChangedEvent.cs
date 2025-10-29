using GoRegister.ApplicationCore.Data.Models;
using MediatR;

namespace GoRegister.ApplicationCore.Domain.Registration.Events
{
    public class DelegateStatusChangedEvent : INotification
    {
        public DelegateUser DelegateUser { get; set; }
    }
}
