using Microsoft.Extensions.DependencyInjection;
using GoRegister.ApplicationCore.Framework.Domain.Mediatr;
using MediatR;
using MediatR.Pipeline;

namespace GoRegister.ApplicationCore.Framework.Domain
{
    public static class DomainRegistration
    {
        public static IServiceCollection AddDomainFramework(this IServiceCollection services)
        {
            services.AddTransient<IPublisher, Publisher>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehaviour<,>));
            //services.AddTransient(typeof(IRequestPostProcessor<,>), typeof(PopulateViewModelPostProcessor<,>));

            return services;
        }
    }
}
