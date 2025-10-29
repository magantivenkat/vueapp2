using AutoMapper;
using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Domain.ProjectThemes.Commands;
using GoRegister.ApplicationCore.Domain.ProjectThemes.Services;
using GoRegister.ApplicationCore.Framework.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using GoRegister.ApplicationCore.Domain.Settings.Services;
using System.Collections.Generic;
using GoRegister.ApplicationCore.Domain.ProjectThemes.Variables;

namespace GoRegister.ApplicationCore.Domain.ProjectThemes.Queries
{
    public class ProjectThemeEditQuery : IRequest<ProjectThemeEditViewModel>
    {
        public class Handler : IRequestHandler<ProjectThemeEditQuery, ProjectThemeEditViewModel>
        {
            private readonly IProjectThemeService _projectThemeService;
            private readonly IMapper _mapper;
            private readonly ApplicationDbContext _db;
            private readonly IProjectSettingsAccessor _projectSettingsAccessor;

            public Handler(IProjectThemeService projectThemeService, IMapper mapper, ApplicationDbContext db, IProjectSettingsAccessor projectSettingsAccessor)
            {
                _projectThemeService = projectThemeService;
                _mapper = mapper;
                _db = db;
                _projectSettingsAccessor = projectSettingsAccessor;
            }

            public async Task<ProjectThemeEditViewModel> Handle(ProjectThemeEditQuery request, CancellationToken cancellationToken)
            {
                var vm = new ProjectThemeEditViewModel();
                vm.Variables = VariableFactory.BuildVariableList().ToDictionary(e => e.Key, e => e);

                var settings = await _projectSettingsAccessor.GetAsync();
                var projectTheme = await _db.ProjectThemes.Include(p => p.Fonts).OrderByDescending(e => e.DateUpdated).FirstAsync(pt => pt.ProjectId == settings.Id);
                var model = _mapper.Map<ProjectThemeEditCommand>(projectTheme);                

                var variableValues = vm.Variables.ToDictionary(e => e.Key, e => e.Value.DefaultValue);
                var currentVariables = !string.IsNullOrWhiteSpace(projectTheme.ThemeVariableObject) ?
                    Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(projectTheme.ThemeVariableObject) :
                    new Dictionary<string, string>();
                foreach (var item in currentVariables)
                {
                    if (!string.IsNullOrWhiteSpace(item.Value) && variableValues.ContainsKey(item.Key))
                    {
                        variableValues[item.Key] = item.Value;
                    }
                }

                model.Variables = variableValues;

                vm.Model = model;

                return vm;
            }
        }
    }

    public class ProjectThemeEditViewModel
    {
        public Dictionary<string, ICssVariable> Variables { get; set; }

        public ProjectThemeEditCommand Model { get; set; }
    }

    //public class ProjectThemeEditQuery : IRequest<ProjectThemeEditCommand>
    //{
    //    public class Handler : PopulateViewModelHandlerBase<ProjectThemeEditCommand>, IRequestHandler<ProjectThemeEditQuery, ProjectThemeEditCommand>
    //    {
    //        private readonly IProjectThemeService _projectThemeService;
    //        private readonly IMapper _mapper;
    //        private readonly ApplicationDbContext _db;
    //        private readonly IProjectSettingsAccessor _projectSettingsAccessor;

    //        public Handler(IProjectThemeService projectThemeService, IMapper mapper, ApplicationDbContext db, IProjectSettingsAccessor projectSettingsAccessor)
    //        {
    //            _projectThemeService = projectThemeService;
    //            _mapper = mapper;
    //            _db = db;
    //            _projectSettingsAccessor = projectSettingsAccessor;
    //        }

    //        public async Task<ProjectThemeEditCommand> Handle(ProjectThemeEditQuery request, CancellationToken cancellationToken)
    //        {
    //            var settings = await _projectSettingsAccessor.GetAsync();
    //            var projectTheme = await _db.ProjectThemes.OrderByDescending(e => e.DateUpdated).FirstAsync(pt => pt.ProjectId == settings.Id);
    //            var model = _mapper.Map<ProjectThemeEditCommand>(projectTheme);
    //            await PopulateViewModel(model);
    //            return model;
    //        }

    //        public override async Task PopulateViewModel(ProjectThemeEditCommand model)
    //        {
    //            var settings = await _projectSettingsAccessor.GetAsync();

    //            model.ProjectIsTemplate = settings.ProjectType == Data.Enums.ProjectTypeEnum.Template;
    //            model.LayoutOptions = _projectThemeService.GetLayoutOptions();
    //        }
    //    }
    //}
}
