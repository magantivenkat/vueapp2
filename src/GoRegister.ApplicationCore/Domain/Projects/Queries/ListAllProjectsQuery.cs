
using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Framework.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GoRegister.ApplicationCore.Domain.Projects.Models;

namespace GoRegister.ApplicationCore.Domain.Projects.Queries
{
    public static class ListAllProjectsQuery
    {
        public class Query : IRequest<Response>
        {
            public int UserId { get; set; }
            public IList<string> Roles { get; set; }
        }
        public class QueryHandler : IRequestHandler<Query, Response>
        {
            private readonly ApplicationDbContext _db;

            public QueryHandler(ApplicationDbContext db)
            {
                _db = db;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = new Response();

                result.AllClientsMRF = await _db.Clients
                                        .Join(_db.MRFClientRequest,
                                        c => c.ClientUuid,
                                        m => m.ClientUuid,
                                        (c, m) => new ClientModelMRF
                                        {
                                            Id = c.Id,
                                            Name = c.Name,
                                            DateCreated = c.DateCreated,
                                            deletedAt = m.deletedAt,
                                            Uuid = m.ClientUuid,
                                            MRFClientStatus = m.MRFClientStatus
                                        }).Where(ms => ms.MRFClientStatus == "Not Published")
                                        .Where(dat => dat.deletedAt == null)
                                        .OrderByDescending(dtcr => dtcr.DateCreated)
                                        .ToListAsync();

                result.RecentProjects = await _db.RecentProjects
                    .Include(p => p.Project)
                    .Where(p => p.User.Id == request.UserId)
                    .OrderByDescending(p => p.DateVisited)
                    .Take(5)
                    .Select(p =>
                        new ProjectModel
                        {
                            Id = p.Project.Id,
                            Name = p.Project.Name,
                            DateCreated = p.Project.DateCreated
                        })
                    .ToListAsync();

                if (request.Roles.Contains(Roles.MeetingPlanner))
                {
                    result.AllProjects = await _db.Projects
                        .Where(p => p.ProjectType == Data.Enums.ProjectTypeEnum.Project &&
                            p.Id != 1 && (p.UserProjectMapping.Any(upm => upm.UserId.Equals(request.UserId)) || p.CreatedByUserId == request.UserId))
                        .Select(p => new ProjectModel
                        {
                            Id = p.Id,
                            Name = p.Name,
                            DateCreated = p.DateCreated,
                            ClientName = p.Client.Name,
                            IsActive = p.IsActive,                           
                        }).OrderByDescending(p => p.DateCreated).ToListAsync();
                }
                else
                {
                    result.AllProjects = await _db.Projects
                        .Where(p => p.ProjectType == Data.Enums.ProjectTypeEnum.Project &&
                            p.Id != 1 )
                         .Select(p => new ProjectModel
                        {
                            Id = p.Id,
                            Name = p.Name,
                            DateCreated = p.DateCreated,
                            ClientName = p.Client.Name,
                             IsActive = p.IsActive,
                         }).OrderByDescending(p => p.DateCreated).ToListAsync();
                }

                return result;
            }
        }
    }
}
