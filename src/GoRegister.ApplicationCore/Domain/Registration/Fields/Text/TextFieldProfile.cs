using AutoMapper;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Registration.Fields.Text
{
    public class TextFieldProfile : Profile
    {
        public TextFieldProfile()
        {
            CreateMap<TextField, TextFieldEditorModel>();

            CreateMap<TextFieldEditorModel, TextField>()
                .ForMember(e => e.Id, e => e.Ignore());
        }
    }
}
