using AutoMapper;
using GoRegister.ApplicationCore.Data.Models;
using System.Linq;

namespace GoRegister.Areas.Admin.Features.Emails
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CreateEditEmail.Command, Email>()
                 .ForMember(e => e.EmailLayoutId, cfg => cfg.MapFrom(e => e.LayoutId))
                .ForMember(e => e.EmailType, cfg => cfg.Ignore());
            CreateMap<CreateEditEmail.EmailTemplateModel, EmailTemplate>()
                .ForMember(e => e.RegistrationTypes, cfg => cfg.Ignore());


            CreateMap<Email, CreateEditEmail.Command>()
                 .ForMember(e => e.LayoutId, cfg => cfg.MapFrom(e => e.EmailLayoutId))
                .ForMember(e => e.Templates, cfg => cfg.MapFrom(e => e.EmailTemplates));
            CreateMap<EmailTemplate, CreateEditEmail.EmailTemplateModel>()
                .ForMember(e => e.RegistrationTypes, cfg => cfg.MapFrom(e => e.RegistrationTypes.Select(rt => rt.RegistrationTypeId)));

            CreateMap<EmailAudit, GetEmailPreviews.EmailAuditResponse>();
        }
    }
}
