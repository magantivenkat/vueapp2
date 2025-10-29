using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Framework.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Templates.Queries
{
    public static class GetTemplates
    {
        public class Query : IRequest<List<ProjectModel>> { }

        public class Response : IRequest<Result<Unit>>
        {
        }

        public class ProjectModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public DateTime DateCreated { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, List<ProjectModel>>
        {
            private readonly ApplicationDbContext _db;

            public QueryHandler(ApplicationDbContext db)
            {
                _db = db;
            }

            public async Task<List<ProjectModel>> Handle(Query request, CancellationToken cancellationToken)
            {
                var templates = await _db.Projects
                    .Where(p => p.ProjectType == Data.Enums.ProjectTypeEnum.Template)
                    .Select(p => new ProjectModel
                    {
                        Id = p.Id,
                        Name = p.Name,
                        DateCreated = p.DateCreated
                    }).OrderBy(p => p.DateCreated).ToListAsync();
                return templates;
            }
        }
    }
}
