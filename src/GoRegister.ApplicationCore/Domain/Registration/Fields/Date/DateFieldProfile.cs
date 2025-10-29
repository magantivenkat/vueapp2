using AutoMapper;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Registration.Fields.Date;

namespace GoRegister.ApplicationCore.Domain.Registration.Fields.Text
{
    public class DateFieldProfile : Profile
    {
        public DateFieldProfile()
        {
            CreateMap<DateField, DateFieldEditorModel>();

            CreateMap<DateFieldEditorModel, DateField>()
                .ForMember(e => e.Id, e => e.Ignore());
        }
    }
}
