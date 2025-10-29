using GoRegister.ApplicationCore.Framework.Dapper;
using Microsoft.Extensions.DependencyInjection;

namespace GoRegister.ApplicationCore.Data.Extensions
{
    public static class ServicesRegistration
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services)
        {
            services.AddTransient<ISlickConnection, SlickConnection>();
            services.AddTransient<IRepository, Repository>();

            return services;
        }
    }
}
