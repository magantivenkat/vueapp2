using MediatR;
using Microsoft.EntityFrameworkCore;
using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Framework.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Menus.Commands
{
    public class MenuItemDeleteCommand : IRequest<Result>
    {
        public int Id { get; set; }

        public class Handler : IRequestHandler<MenuItemDeleteCommand, Result>
        {

            private readonly ApplicationDbContext _db;

            public Handler(ApplicationDbContext db)
            {

                _db = db;
            }

            public async Task<Result> Handle(MenuItemDeleteCommand request, CancellationToken cancellationToken)
            {
                var menuItem = await _db.MenuItems
                    .Include(mi => mi.MenuItemRegistrationTypes)
                    .FirstOrDefaultAsync(mi => mi.Id == request.Id);

                if (menuItem == null)
                {
                    return Result.Fail();
                }

                _db.Remove(menuItem);
                await _db.SaveChangesAsync();
                return Result.Ok();
            }
        }

    }

}
