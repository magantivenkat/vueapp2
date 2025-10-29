using AutoMapper;
using GoRegister.ApplicationCore.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoRegister.ApplicationCore.Domain.Registration.Fields.TextArea
{
    public class TextAreaFieldProfile : Profile
    {
        public TextAreaFieldProfile()
        {
            CreateMap<TextAreaField, TextAreaFieldEditorModel>();

            CreateMap<TextAreaFieldEditorModel, TextAreaField>()
                .ForMember(e => e.Id, e => e.Ignore());
        }
    }
}
