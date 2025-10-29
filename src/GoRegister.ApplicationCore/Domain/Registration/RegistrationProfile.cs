using AutoMapper;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Registration.Fields;
using GoRegister.ApplicationCore.Domain.Registration.Models;

namespace GoRegister.ApplicationCore.Domain.Registration
{
    public class RegistrationProfile : Profile
    {
        public RegistrationProfile()
        {
            FieldsMap();


            CreateMap<RegistrationType, RegistrationTypeModel>()
                .ReverseMap();

            CreateMap<DelegateUser, DelegateListItemModel>()
                .ForMember(e => e.Email, opt => opt.MapFrom(src => src.ApplicationUser.Email))
                .ForMember(e => e.FirstName, opt => opt.MapFrom(src => src.ApplicationUser.FirstName))
                .ForMember(e => e.LastName, opt => opt.MapFrom(src => src.ApplicationUser.LastName))
                .ForMember(e => e.RegistrationStatus, opt => opt.MapFrom(src => src.RegistrationStatus.Description))
                .ForMember(e => e.RegistrationType, opt => opt.MapFrom(src => src.RegistrationType.Name));

            CreateMap<DelegateUser, DelegateCreateModel>()
                .ForMember(e => e.Email, opt => opt.MapFrom(src => src.ApplicationUser.Email))
                .ForMember(e => e.FirstName, opt => opt.MapFrom(src => src.ApplicationUser.FirstName))
                .ForMember(e => e.LastName, opt => opt.MapFrom(src => src.ApplicationUser.LastName))
                .ForMember(e => e.RegistrationTypeId, opt => opt.MapFrom(src => src.RegistrationType.Id))
                .ReverseMap();

            CreateMap<EmailTemplateDesignModel, EmailTemplateCreateModel>()
                .ReverseMap();

            CreateMap<EmailAudit, EmailAuditModel>()
                .ForMember(e => e.FirstName, opt => opt.MapFrom(src => src.DelegateUser.ApplicationUser.FirstName))
                .ForMember(e => e.LastName, opt => opt.MapFrom(src => src.DelegateUser.ApplicationUser.LastName))
                .ForMember(e => e.Email, opt => opt.MapFrom(src => src.DelegateUser.ApplicationUser.Email))
                .ReverseMap();

            CreateMap<EmailAuditBatch, EmailAuditBatchModel>()
                .ReverseMap();

            CreateMap<EmailAuditNotification, EmailAuditNotificationModel>()
                .ReverseMap();

        }

        public void FieldsMap()
        {
            CreateMap<FieldOptionsEditModel.OptionEditModel, FieldOption>()
                .ForMember(e => e.Id, e => e.Ignore());

            CreateMap<FieldOption, FieldOptionsEditModel.OptionEditModel>()
                .ForMember(e => e.Id, e => e.MapFrom(x => x.Id.ToString()));
        }
    }
}
