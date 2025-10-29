using GoRegister.ApplicationCore.Domain.Settings.Services;
using GoRegister.ApplicationCore.Framework.Multitenancy.Filters;
using GoRegister.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace GoRegister.Controllers
{
    [TenantFilter]
    [ThemedResultFilter]
    public abstract class GoRegisterControllerBase : Controller
    {
        private IMediator _mediatorOverride;
        public IMediator Mediator
        {
            get
            {
                return _mediatorOverride ?? HttpContext?.RequestServices.GetRequiredService<IMediator>();
            }
            set { _mediatorOverride = value; }
        }

        private IProjectSettingsAccessor _projectSettingsAccessorOverride;
        public IProjectSettingsAccessor ProjectSettingsAccessor
        {
            get
            {
                return _projectSettingsAccessorOverride ?? HttpContext?.RequestServices.GetRequiredService<IProjectSettingsAccessor>();
            }
            set { _projectSettingsAccessorOverride = value; }
        }

        public GoRegisterControllerBase()
        {
        }
    }
}
