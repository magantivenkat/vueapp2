using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Framework.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Menus.Commands
{
    public class MenuListUpdateCommand : IRequest<Result<Unit>> 
    {
        public List<int> Order { get; set; }

        public class Handler : IRequestHandler<MenuListUpdateCommand, Result<Unit>>
        {
            private readonly ApplicationDbContext _db;

            public Handler(ApplicationDbContext db)
            {
                _db = db;
            }

            public async Task<Result<Unit>> Handle(MenuListUpdateCommand request, CancellationToken cancellationToken)
            {
                // get menu items
                var menuItems = await _db.MenuItems.ToListAsync();

                // order by provided id order
                int i = 0;
                foreach(var id in request.Order)
                {
                    var item = menuItems.First(e => e.Id == id);
                    item.Order = i;
                    i++;
                }

                // save
                await _db.SaveChangesAsync();

                return Result.Ok(Unit.Value);
            }
        }
    }
}
