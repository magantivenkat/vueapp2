using AutoMapper;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.ProjectPages.Models;

namespace GoRegister.ApplicationCore.Domain.ProjectPages
{
    public class ProjectPagesProfile : Profile
    {
        public ProjectPagesProfile()
        {
            CreateMap<ProjectPage, ProjectPageModel>()
                .ReverseMap();
        }


    }
}
