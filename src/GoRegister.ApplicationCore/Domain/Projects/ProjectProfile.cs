using AutoMapper;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Projects.Commands.CreateProject;
using GoRegister.ApplicationCore.Domain.Projects.Models;

namespace GoRegister.ApplicationCore.Domain.Projects
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            CreateMap<CreateProjectCommand, Project>()
                .ForMember(e => e.ProjectThemes, opt => opt.Ignore())
                .ForMember(e => e.Id, opt => opt.Ignore());

            // liquid
            CreateMap<Project, ProjectSettingsLiquidModel>();
                //.ForMember(p => p.Tel, opts => opts.MapFrom(p => p.Telephone));
        }
    }
}
