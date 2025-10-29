using GoRegister.ApplicationCore.Hangfire.Jobs;
using GoRegister.ApplicationCore.Jobs;
using Microsoft.Extensions.DependencyInjection;

namespace GoRegister.ApplicationCore.Hangfire
{
    public static class JobRegistration
    {
        public static IServiceCollection AddJobs(this IServiceCollection services)
        {
            services.AddTransient<ISendEmailsJob, SendEmailsJob>();
            services.AddTransient<IJob<SendIndividualEmailOptions>, SendIndividualEmailJob>();

            return services;
        }
    }
}
