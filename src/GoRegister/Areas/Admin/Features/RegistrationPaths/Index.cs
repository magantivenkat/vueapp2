using GoRegister.ApplicationCore.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GoRegister.Areas.Admin.Features.RegistrationPaths
{
    public static class Index
    {
        public class Query : IRequest<Result>
        {

        }

        public class Result
        {
            public bool IsPathsEnabled { get; set; } = true;
            public int OnlyPathId { get; set; }
            public List<Item> Items { get; set; }

            public class Item
            {
                public int Id { get; set; }
                public string Name { get; set; }
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
                result.Items = await _db.RegistrationPaths.Select(e => new Result.Item()
                {
                    Id = e.Id,
                    Name = e.Name
                }).ToListAsync();

                if(result.Items.Count() == 1)
                {
                    result.IsPathsEnabled = false;
                    result.OnlyPathId = result.Items.FirstOrDefault().Id;
                }

                return result;
            }
        }
    }
}
