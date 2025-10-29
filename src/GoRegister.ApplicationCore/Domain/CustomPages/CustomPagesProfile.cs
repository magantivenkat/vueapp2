using AutoMapper;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.CustomPages.Models;

namespace GoRegister.ApplicationCore.Domain.CustomPages
{
    public class CustomPagesProfile : Profile
    {
        public CustomPagesProfile()
        {
            CreateMap<CustomPage, CustomPageListItemModel>()
                .ReverseMap();

            CreateMap<CustomPage, CustomPageCreateEditModel>()
                .ForMember(c => c.CustomPageRegistrationTypes, opt
                    => opt.MapFrom(src => src.CustomPageRegistrationTypes))
                .ForMember(c => c.CustomPageRegistrationStatuses, opt
                    => opt.MapFrom(src => src.CustomPageRegistrationStatuses))
                .ReverseMap();

            CreateMap<CustomPageAudit, CustomPageAuditModel>()
                .ForMember(c => c.Title, opt
                    => opt.MapFrom(src => src.CustomPage.Title))
                .ReverseMap();

            CreateMap<CustomPageAuditRegistrationType, CustomPageAuditRegistrationTypeModel>()
                .ReverseMap();

            CreateMap<CustomPage, CustomPageVersionModel>()
                .ForMember(c => c.CustomPageId, opt
                    => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Id, act => act.Ignore())
                .ReverseMap();

            CreateMap<CustomPageVersion, CustomPageVersionModel>()
                .ReverseMap();

            CreateMap<CustomPageCreateEditModel, CustomPageVersionModel>()
                .ReverseMap();

        }
    }
}