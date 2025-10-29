using AutoMapper;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Data.Models.Query;
using GoRegister.ApplicationCore.Domain.Sessions.ViewModels;
using System.Linq;

namespace GoRegister.ApplicationCore.Domain.Sessions
{
    public class SessionMappingProfile : Profile
    {
        public SessionMappingProfile()
        {
            CreateMap<Session, SessionCreateEditViewModel>()
                .ForMember(dest => dest.RegTypeIds, opt => opt.MapFrom(s => s.SessionRegistrationTypes.Select(srt => srt.RegistrationTypeId)))
                .ReverseMap();


            CreateMap<Session, SessionModel>()
                .ForMember(dest => dest.RegistrationCount, opt => opt.MapFrom(s => s.DelegateSessionBookings.Count))
                .ForMember(dest => dest.AnonRegistrationCount, opt => opt.MapFrom(s => s.AnonSessionBookings.Count));

            CreateMap<SessionCategory, SessionCategoryModel>().ReverseMap();

            CreateMap<SessionModel, GetUserSession_Result>().ReverseMap();
        }
    }
}
