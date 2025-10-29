using AutoMapper;
using GoRegister.ApplicationCore.Data.Models;

namespace GoRegister.ApplicationCore.Domain.Domains
{
    public class DomainMappingProfile : Profile
    {
        public DomainMappingProfile()
        {           
            CreateMap<TenantUrl, DomainModel>().ReverseMap();
        }
    }
}
