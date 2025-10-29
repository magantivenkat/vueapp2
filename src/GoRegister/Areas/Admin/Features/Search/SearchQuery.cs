using GoRegister.ApplicationCore.Framework.Dapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GoRegister.Areas.Admin.Features.Search
{
    public static class SearchQuery
    {
        public class Query : IRequest<List<Result>>
        {
            public string Value { get; set; }
            public int ProjectId { get; set; }
        }

        public class Result
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string FullName => $"{FirstName} {LastName}";
            public string Email { get; set; }
            public int ProjectId { get; set; }
            public string ManageUrl => $"/admin/project/{ProjectId}/Delegates/Manage/{Id}";
        }

        public class QueryHandler : IRequestHandler<Query, List<Result>>
        {
            private readonly ISlickConnection _slickConnection;


            public QueryHandler(ISlickConnection slickConnection)
            {
                _slickConnection = slickConnection;
            }

            public async Task<List<Result>> Handle(Query request, CancellationToken cancellationToken)
            {
                if (string.IsNullOrWhiteSpace(request.Value))
                {
                    return new List<Result>();
                }

                var likeQuery = $"%{string.Join("%", request.Value.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))}%";

                //var likeQuery = $"%{request.Value.Trim().Replace(' ', '%')}%";

                using (var connection = _slickConnection.Get())
                {
                    var results = (await connection.QueryAsync<Result>(
                        @"select top 10 [Id], [FirstName], [LastName], [Email], [ProjectId] 
                    from [User] 
                    where [ProjectId] = @projectId 
                    and ([FirstName] like @likeQuery or [LastName] like @likeQuery or [Email] like @likeQuery)", new { likeQuery, projectId = request.ProjectId })
                        ).ToList();

                    return results;
                }
            }
        }
    }
}
