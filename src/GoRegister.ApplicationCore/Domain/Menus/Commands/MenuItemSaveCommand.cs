using AutoMapper;
using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Menus.Models;
using GoRegister.ApplicationCore.Framework.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Menus.Commands
{
    public class MenuItemSaveCommand : IRequest<Result<int>>
    {
        public MenuItemModel Model { get; set; }

        public MenuItemSaveCommand(MenuItemModel model)
        {
            Model = model;
        }

        public class Handler : IRequestHandler<MenuItemSaveCommand, Result<int>>
        {
            private readonly IMapper _mapper;
            private readonly ApplicationDbContext _db;

            public Handler(IMapper mapper, ApplicationDbContext db)
            {
                _mapper = mapper;
                _db = db;
            }

            public async Task<Result<int>> Handle(MenuItemSaveCommand request, CancellationToken cancellationToken)
            {
                var model = request.Model;
                MenuItem item;
                // get menu item if id > 0
                if (model.Id > 0)
                {
                    item = await _db.MenuItems
                        .Include(e => e.MenuItemRegistrationTypes)
                        .FirstOrDefaultAsync(e => e.Id == model.Id);
                    if (item == null)
                    {
                        return Result.NotFound<int>("Menu Item could not be found");
                    }

                    // reset type specific links so we don't map foreign keys that aren't required
                    item.FormId = null;
                    item.CustomPageId = null;
                }
                // else create new
                else
                {
                    item = new MenuItem();

                    // get order, add to end
                    // the cast to null fixes issue where there are no menu items in db
                    item.Order = (await _db.MenuItems.MaxAsync(e => (int?)e.Order) ?? 0) + 1;

                    _db.MenuItems.Add(item);
                }

                // if custom page check it exists
                if (model.MenuItemType == Data.Enums.MenuItemType.CustomPage)
                {
                    var customPageExists = await _db.CustomPages.AnyAsync(cp => cp.Id == model.CustomPageId.Value);
                    if (!customPageExists)
                    {
                        return Result.NotFound<int>("Custom page could not be found");
                    }

                    item.CustomPageId = model.CustomPageId;
                }
                else if (model.MenuItemType == Data.Enums.MenuItemType.Link)
                {

                }

                // mapping with automapper, ignoring foreign keys
                // should have used ef core TPH...
                _mapper.Map(model, item);

                // map reg types
                var regTypes = await _db.RegistrationTypes.Where(rt => model.RegistrationTypes.Contains(rt.Id)).ToListAsync();
                item.MenuItemRegistrationTypes = regTypes.Select(e => new MenuItemRegistrationType { MenuItem = item, RegistrationType = e }).ToList();

                // save
                await _db.SaveChangesAsync();

                return Result.Ok(item.Id);
            }
        }
    }
}
