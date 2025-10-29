using AutoMapper;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Menus.Models;
using System.Linq;

namespace GoRegister.ApplicationCore.Domain.Menus
{
    public class MenuProfile : Profile
    {
        public MenuProfile()
        {
            CreateMap<MenuItem, MenuItemModel>()
                .ForMember(e => e.RegistrationTypes, cfg => cfg.MapFrom(e => e.MenuItemRegistrationTypes.Select(rt => rt.RegistrationTypeId)));
            CreateMap<MenuItemModel, MenuItem>()
                .ForMember(e => e.Id, cfg => cfg.Ignore())
                .ForMember(e => e.CustomPageId, cfg => cfg.Ignore())
                .ForMember(e => e.FormId, cfg => cfg.Ignore())
                .ForMember(x => x.Fragment, opt => opt.MapFrom(y => y.Fragment.TrimStart('#').Trim()));

            CreateMap<MenuItem, MenuItemListModel>()
                .ForMember(mi => mi.CustomPageLabel, cfg => cfg.MapFrom(mi => mi.CustomPageId.HasValue ? mi.CustomPage.Title : null));
        }
    }
}
