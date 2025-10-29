using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Framework;
using GoRegister.ApplicationCore.Jobs;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Services.Email.Schedule
{
    public class HangfireEmailSchedulingService : IEmailSchedulingService
    {
        private readonly ProjectTenant _projectTenant;

        public HangfireEmailSchedulingService(ProjectTenant projectTenant)
        {
            _projectTenant = projectTenant;
        }

        public Task Schedule(EmailAuditBatch batch)
        {
            BackgroundJob.Enqueue<ISendEmailsJob>(i => i.Execute(batch.Id, _projectTenant));
            return Task.CompletedTask;
        }
    }
}
