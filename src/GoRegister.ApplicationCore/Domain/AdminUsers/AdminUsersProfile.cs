using AutoMapper;
using GoRegister.ApplicationCore.Data.Models;

namespace GoRegister.ApplicationCore.Domain.AdminUsers
{
    public class AdminUsersProfile : Profile
    {
        public AdminUsersProfile()
        {
            CreateMap<ApplicationUser, AdminUserListItemModel>()
                .ReverseMap();
            CreateMap<ApplicationUser, AdminUserCreateEditModel>()
                .ReverseMap();

            CreateMap<ApplicationUser, AccountSettingsModel>()
                .ReverseMap();
        }
    }
}
