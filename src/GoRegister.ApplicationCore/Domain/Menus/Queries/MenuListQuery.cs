using AutoMapper;
using AutoMapper.QueryableExtensions;
using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Domain.Menus.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace GoRegister.ApplicationCore.Domain.Menus.Queries
{
    public class MenuListQuery : IRequest<MenuViewModel> 
    {
        public class Handler : IRequestHandler<MenuListQuery, MenuViewModel>
        {
            private readonly IMapper _mapper;
            private readonly ApplicationDbContext _db;

            public Handler(IMapper mapper, ApplicationDbContext db)
            {
                _mapper = mapper;
                _db = db;
            }

            public async Task<MenuViewModel> Handle(MenuListQuery request, CancellationToken cancellationToken)
            {
                var vm = new MenuViewModel();

                vm.Items = await _db.MenuItems
                    .OrderBy(mi => mi.Order)
                    .ProjectTo<MenuItemListModel>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                return vm;
            }
        }
    }
}
