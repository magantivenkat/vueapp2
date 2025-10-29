using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Framework.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Delegates.Queries
{
    public static class GetTestDelegatesForPreview
    {
        public class Query : IRequest<DelegateModel> {
            public string CookieValue { get; set; }
        }

        public class DelegateModel : IRequest<Result<Unit>>
        {
            public List<SelectListItem> Delegates { get; set; } = new List<SelectListItem>();
        }

        public class QueryHandler : IRequestHandler<Query, DelegateModel>
        {
            private readonly ApplicationDbContext _db;

            public QueryHandler(ApplicationDbContext db)
            {
                _db = db;
            }

            public async Task<DelegateModel> Handle(Query request, CancellationToken cancellationToken)
            {
                var delegates = await _db.Delegates.Include(d => d.ApplicationUser).Where(d => d.IsTest).ToListAsync();

                var model = new DelegateModel();

                foreach (var d in delegates)
                {
                    model.Delegates.Add(new SelectListItem
                    {
                        Text = $"{d.ApplicationUser.FirstName} {d.ApplicationUser.LastName}",
                        Value = d.Id.ToString(),
                        Selected = d.Id.ToString() == request.CookieValue
                    });
                };

                return model;
            }
        }

    }

}
