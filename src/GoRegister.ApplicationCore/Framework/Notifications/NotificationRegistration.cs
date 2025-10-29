using Microsoft.Extensions.DependencyInjection;

namespace GoRegister.ApplicationCore.Framework.Notifications
{
    public static class NotificationRegistration {
        public static IServiceCollection AddNotifications(this IServiceCollection services) {
            services.AddScoped<INotifier, Notifier>();

            return services;
        }
    }
}
