using AutoMapper;
using GoRegister.ApplicationCore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.Areas.Admin.Features.RegistrationPaths
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegistrationPath, Edit.Command>().ReverseMap();
        }
    }
}
