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
    public class DeclineEmailHandler : INotificationHandler<DelegateDeclinedRegistrationEvent>
    {

        private readonly ProjectTenant _projectTenant;

        public DeclineEmailHandler(ProjectTenant projectTenant)
        {
            _projectTenant = projectTenant;
        }

        public Task Handle(DelegateDeclinedRegistrationEvent notification, CancellationToken cancellationToken)
        {
            var options = new SendIndividualEmailOptions(_projectTenant)
            {
                EmailType = EmailType.Decline,
                UserId = notification.DelegateUser.Id
            };
            BackgroundJob.Enqueue<IJob<SendIndividualEmailOptions>>(i => i.Execute(options));
            return Task.CompletedTask;
        }
    }
}
