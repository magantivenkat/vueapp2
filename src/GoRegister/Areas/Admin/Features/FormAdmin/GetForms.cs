using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GoRegister.Areas.Admin.Features.FormAdmin
{
    public class GetForms
    {
        public class Query : IRequest<Result> { }

        public class Result
        {
            public Item RegistrationForm { get; set; }
            public Item DeclineForm { get; set; }
            public Item CancelForm { get; set; }

            public List<Item> Forms { get; set; }

            public class Item
            {
                public int Id { get; set; }
                public string Name { get; set; }
                public FormType FormTypeId { get; set; }
            }
        }


        public class QueryHandler : IRequestHandler<Query, Result>
        {
            private readonly ApplicationDbContext _db;

            public QueryHandler(ApplicationDbContext db)
            {
                _db = db;
            }

            public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = new Result();
                var items = await _db.Forms.Select(e => new Result.Item
                {
                    Id = e.Id,
                    FormTypeId = e.FormTypeId,
                    Name = e.AdminDisplayName
                }).ToListAsync();

                result.RegistrationForm = items.FirstOrDefault(e => e.FormTypeId == FormType.Registration);
                result.CancelForm = items.FirstOrDefault(e => e.FormTypeId == FormType.Cancel);
                result.DeclineForm = items.FirstOrDefault(e => e.FormTypeId == FormType.Decline);
                result.Forms = items.Where(e => e.FormTypeId == FormType.Custom).ToList();

                return result;
            }
        }
    }
}
