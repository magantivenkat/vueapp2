using AutoMapper;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Data.Models.Fields;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoRegister.ApplicationCore.Domain.Registration.Fields.MRFDestination
{
    public class MRFDestinationFieldProfile : Profile
    {
        public MRFDestinationFieldProfile()
        {
            CreateMap<MRFDestinationField, MRFDestinationFieldEditorModel>();

            CreateMap<MRFDestinationFieldEditorModel, MRFDestinationField>()
                .ForMember(e => e.Id, e => e.Ignore());
        }
    }
}
