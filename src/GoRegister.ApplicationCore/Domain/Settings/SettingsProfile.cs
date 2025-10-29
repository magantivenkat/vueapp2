using AutoMapper;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Settings.Models;

namespace GoRegister.ApplicationCore.Domain.Settings
{
    public class SettingsProfile : Profile
    {
        public SettingsProfile()
        {
            CreateMap<Project, SeoSettingsModel>()
                .ReverseMap();
            CreateMap<Project, PasswordSettingsModel>()
                .ReverseMap();

            CreateMap<Project, GeneralSettingsModel>()
                .ForMember(e => e.ClientId, opt => opt.MapFrom(e => e.Client.Id));

            CreateMap<GeneralSettingsModel, Project>()
                .ForMember(e => e.Code, opt => opt.Ignore())
                .ForMember(e => e.Id, opt => opt.Ignore());

        }
    }
}
