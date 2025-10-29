using AutoMapper;
using GoRegister.ApplicationCore.Data.Models;

namespace GoRegister.ApplicationCore.Domain.Clients
{
    public class ClientMappingProfile : Profile
    {
        public ClientMappingProfile()
        {
            CreateMap<Client, ClientModel>().ReverseMap();
            CreateMap<ClientEmail, ClientEmailModel>().ReverseMap();
            CreateMap<TPNCountryClientEmail, TPNCountryClientEmailModel>().ReverseMap();
            CreateMap<TPNCountryAdminMapping, TPNClientGAMappingModel>().ReverseMap();
        }
    }
}
