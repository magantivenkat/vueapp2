using AutoMapper;
using GoRegister.ApplicationCore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Registration.Framework.Builder
{
    public class BuilderProfile : Profile
    {
        public BuilderProfile()
        {
            CreateMap<Field, FieldEditorModel>()
                .IncludeAllDerived();

            CreateMap<FieldEditorModel, Field>()
                .ForMember(e => e.Id, e => e.Ignore())
                .IncludeAllDerived();
        }
    }
}
