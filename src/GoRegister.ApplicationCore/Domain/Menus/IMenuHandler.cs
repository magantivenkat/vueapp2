using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Domain.Delegates.Models;
using GoRegister.ApplicationCore.Domain.Menus.Models;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Menus
{
    public interface IMenuHandler
    {
        MenuItemType MenuItemType { get; }
        bool ShouldDisplay(BuildMenuItemModel menuItem, AttendeeAuthorizationModel attendee);
        Task ProcessMenuItem(BuildMenuItemModel menuItem, MenuLinkModel displayModel);
    }
}
