using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Domain.Menus.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Domain.Delegates.Models;

namespace GoRegister.ApplicationCore.Domain.Menus.Queries
{
    public class BuildMenuQuery : IRequest<BuildMenuViewModel>
    {
        public AttendeeAuthorizationModel AttendeeAuthorizationModel { get; set; }

        public class Handler : IRequestHandler<BuildMenuQuery, BuildMenuViewModel>
        {
            private readonly ApplicationDbContext _db;
            private readonly IMenuFactory _menuFactory;

            public Handler(ApplicationDbContext db, IMenuFactory menuFactory)
            {
                _db = db;
                _menuFactory = menuFactory;
            }

            public async Task<BuildMenuViewModel> Handle(BuildMenuQuery request, CancellationToken cancellationToken)
            {
                var vm = new BuildMenuViewModel();

                var items = await _db.MenuItems
                    .OrderBy(mi => mi.Order)
                    .Select(mi => new BuildMenuItemModel
                    {
                        Id = mi.Id,
                        Label = mi.Label,
                        MenuItemType = mi.MenuItemType,
                        OpenInNewTab = mi.OpenInNewTab,
                        Fragment = mi.Fragment,
                        CssClass = mi.CssClass,
                        AnchorLink = mi.AnchorLink,
                        RegistrationTypeIds = mi.MenuItemRegistrationTypes.Select(rt => rt.RegistrationTypeId),
                        // custom page link
                        CustomPageSlug = mi.CustomPage != null ? mi.CustomPage.Slug : null,
                        CustomPageTitle = mi.CustomPage != null ? mi.CustomPage.Title : null,
                        CustomPageType = mi.CustomPage != null ? (PageType?)mi.CustomPage.PageType : null,
                        CustomPageRegistrationTypes = mi.CustomPage != null ? mi.CustomPage.CustomPageRegistrationTypes.Select(e => e.RegistrationTypeId) : null,
                        CustomPageRegistrationStatuses = mi.CustomPage != null ? mi.CustomPage.CustomPageRegistrationStatuses.Select(e => (Data.Enums.RegistrationStatus)e.RegistrationStatusId) : null
                    })
                    .ToListAsync();

                var attendeeAuthModel = request.AttendeeAuthorizationModel;

                var links = new List<MenuLinkModel>();
                foreach (var item in items)
                {
                    var handler = _menuFactory.GetHandler(item.MenuItemType);

                    if (!handler.ShouldDisplay(item, attendeeAuthModel)) continue;

                    // create model with default values
                    var link = new MenuLinkModel();
                    link.Label = item.Label;
                    link.CssClass = item.CssClass;
                    link.OpenInNewTab = item.OpenInNewTab;

                    await handler.ProcessMenuItem(item, link);

                    links.Add(link);
                }

                vm.Links = links;

                return vm;
            }
        }
    }
}
