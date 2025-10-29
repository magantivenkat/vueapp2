using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Domain.Registration.Events;
using GoRegister.ApplicationCore.Framework;
using GoRegister.ApplicationCore.Hangfire;
using GoRegister.ApplicationCore.Hangfire.Jobs;
using Hangfire;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Registration.EventHandlers
{
    public class CancellationEmailHandler : INotificationHandler<DelegateCancelledRegistrationEvent>
    {

        private readonly ProjectTenant _projectTenant;

        public CancellationEmailHandler(ProjectTenant projectTenant)
        {
            _projectTenant = projectTenant;
        }

        public Task Handle(DelegateCancelledRegistrationEvent notification, CancellationToken cancellationToken)
        {
            var options = new SendIndividualEmailOptions(_projectTenant)
            {
                EmailType = EmailType.Cancellation,
                UserId = notification.DelegateUser.Id
            };
            BackgroundJob.Enqueue<IJob<SendIndividualEmailOptions>>(i => i.Execute(options));
            return Task.CompletedTask;
        }
    }
}
