using Microsoft.Extensions.DependencyInjection;

namespace GoRegister.ApplicationCore.Services
{
    public static class ServicesRegistration
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            // Email
            services.AddTransient<Email.IEmailService, Email.EmailService>();

            services.AddTransient<Email.IEmailSchedulingService, Email.Schedule.HangfireEmailSchedulingService>();
            services.AddTransient<Email.IEmailSendingService, Email.SmtpEmailSendingService>();

            // FileStorage
            services.AddSingleton<FileStorage.IFileStorage, FileStorage.S3.S3FileStorage>();

            return services;
        }
    }
}