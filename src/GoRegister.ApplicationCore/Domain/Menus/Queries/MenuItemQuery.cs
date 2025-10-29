using AutoMapper;
using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Domain.Menus.Models;
using GoRegister.ApplicationCore.Extensions;
using GoRegister.ApplicationCore.Framework.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Menus.Queries
{

    public class MenuItemQuery : IRequest<Result<MenuItemViewModel>>
    {
        public MenuItemQuery(int? id)
        {
            Id = id;
        }

        public int? Id { get; set; }

        public class Handler : IRequestHandler<MenuItemQuery, Result<MenuItemViewModel>>
        {
            private readonly IMapper _mapper;
            private readonly ApplicationDbContext _db;

            public Handler(IMapper mapper, ApplicationDbContext db)
            {
                _mapper = mapper;
                _db = db;
            }

            public async Task<Result<MenuItemViewModel>> Handle(MenuItemQuery request, CancellationToken cancellationToken)
            {
                var vm = new MenuItemViewModel();
                if (request.Id.HasValue)
                {
                    var item = await _db.MenuItems
                        .Include(mi => mi.MenuItemRegistrationTypes)
                        .FirstOrDefaultAsync(mi => mi.Id == request.Id);
                    if (item == null)
                    {
                        return Result.NotFound<MenuItemViewModel>("Menu item could not be found");
                    }

                    vm.Model = _mapper.Map<MenuItemModel>(item);
                }
                else
                {
                    vm.Model = new MenuItemModel();
                }

                vm.MenuItemTypes = Enum.GetValues(typeof(MenuItemType)).Cast<MenuItemType>().Select(e => new TextValueModel((int)e, e.ToString().SplitCamelCase()));
                vm.CustomPages = await _db.CustomPages.OrderBy(cp => cp.Title).Select(cp => new TextValueModel(cp.Id, cp.Title)).ToListAsync();
                vm.RegistrationTypes = await _db.RegistrationTypes.Select(rt => new TextValueModel(rt.Id, rt.Name)).ToListAsync();

                return Result.Ok(vm);
            }
        }
    }
}
