using GoRegister.ApplicationCore.Domain.Registration.Framework;

namespace GoRegister.ApplicationCore.Domain.Delegates.Models
{
    public class AttendeeUserWrapper
    {
        public AttendeeUserWrapper(DelegateDataTagAccessor attendee)
        {
            Value = attendee;
        }

        public bool IsAnonymous => Value == null;
        public DelegateDataTagAccessor Value { get; }
    }
}
