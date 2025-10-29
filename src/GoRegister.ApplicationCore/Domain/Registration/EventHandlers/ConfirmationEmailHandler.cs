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
    public class ConfirmationEmailHandler : INotificationHandler<DelegateRegisteredEvent>
    {
        private readonly ProjectTenant _projectTenant;

        public ConfirmationEmailHandler(ProjectTenant projectTenant)
        {
            _projectTenant = projectTenant;
        }

        public Task Handle(DelegateRegisteredEvent notification, CancellationToken cancellationToken)
        {
            var options = new SendIndividualEmailOptions(_projectTenant)
            {
                EmailType = Data.Enums.EmailType.Confirmation,
                UserId = notification.DelegateUser.Id
            };
            BackgroundJob.Enqueue<IJob<SendIndividualEmailOptions>>(i => i.Execute(options));
            return Task.CompletedTask;
        }
    }
}
