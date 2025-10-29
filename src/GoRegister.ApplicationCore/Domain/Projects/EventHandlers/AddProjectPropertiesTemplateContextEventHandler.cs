using AutoMapper;
using GoRegister.ApplicationCore.Domain.Liquid.Events;
using GoRegister.ApplicationCore.Domain.Projects.Models;
using GoRegister.ApplicationCore.Domain.Settings.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Projects.EventHandlers
{
    public class AddProjectPropertiesTemplateContextEventHandler : INotificationHandler<CreatingGoRegisterTemplateContextEvent>
    {
        private readonly IProjectSettingsAccessor _projectSettings;
        private readonly IMapper _mapper;

        public AddProjectPropertiesTemplateContextEventHandler(IProjectSettingsAccessor projectSettings, IMapper mapper)
        {
            _projectSettings = projectSettings;
            _mapper = mapper;
        }

        public async Task Handle(CreatingGoRegisterTemplateContextEvent notification, CancellationToken cancellationToken)
        {
            var settings = await _projectSettings.GetAsync();
            var liquidModel = _mapper.Map<ProjectSettingsLiquidModel>(settings);
            notification.TemplateContext.SetValue("project", liquidModel);
        }
    }
}
