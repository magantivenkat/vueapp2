using AutoMapper;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.ProjectThemes.Commands;
using GoRegister.ApplicationCore.Domain.ProjectThemes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.ProjectThemes
{
    public class ProjectThemeProfile : Profile
    {
        public ProjectThemeProfile()
        {
            CreateMap<ProjectTheme, ProjectThemeModel>().ReverseMap();
            CreateMap<ProjectTheme, ProjectThemeListItemModel>();

            CreateMap<ProjectTheme, ProjectThemeEditCommand>();
            CreateMap<ThemeFont, FontViewModel>()
                .ForMember(e => e.Variants, cfg => cfg.MapFrom(f => f.Variants.Split(',',StringSplitOptions.None)));            

        }
    }
}
