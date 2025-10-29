using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace GoRegister.ApplicationCore.Domain
{
    public static class AutoMapperServiceCollectionExtension
    {
        public static IServiceCollection AddMappings(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperServiceCollectionExtension));

            return services;
        }
    }
}
