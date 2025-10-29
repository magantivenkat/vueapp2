using GoRegister.ApplicationCore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Registration.Fields.SingleSelect
{
    public class Profile : AutoMapper.Profile
    {
        public Profile()
        {
            CreateMap<FieldOptionsEditModel, SingleSelectField>()
                .ForMember(e => e.FieldOptions, rule => rule.Ignore());

            CreateMap<SingleSelectField, FieldOptionsEditModel>()
                .ForMember(e => e.Options, rule => rule.MapFrom(e => e.FieldOptions));
        }
    }
}
