using GoRegister.ApplicationCore.Framework.Dapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GoRegister.Areas.Admin.Features.Emails
{
    public static class SearchUsersQuery
    {
        public class Query : IRequest<List<UserResult>>
        {
            public string Value { get; set; }
            public int ProjectId { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, List<UserResult>>
        {
            private readonly ISlickConnection _slickConnection;


            public QueryHandler(ISlickConnection slickConnection)
            {
                _slickConnection = slickConnection;
            }

            public async Task<List<UserResult>> Handle(Query request, CancellationToken cancellationToken)
            {
                if (string.IsNullOrWhiteSpace(request.Value))
                {
                    return new List<UserResult>();
                }

                var likeQuery = $"%{string.Join("%", request.Value.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))}%";

                //var likeQuery = $"%{request.Value.Trim().Replace(' ', '%')}%";

                using (var connection = _slickConnection.Get())
                {
                    var results = (await connection.QueryAsync<UserResult>(
                    @"
                    select top 10 [user].[Id], [FirstName], [LastName], [Email], [user].[ProjectId], rt.[Name] as 'RegistrationType', rs.[Description] as 'RegistrationStatus'
                    from [User] 
                    join [delegateuser] du on du.Id = [user].id
                    join registrationtype rt on du.RegistrationTypeId = rt.Id
                    join RegistrationStatus rs on rs.Id = du.RegistrationStatusId
                    where [user].[ProjectId] = @projectId 
                    and ([FirstName] like @likeQuery or [LastName] like @likeQuery or [Email] like @likeQuery)", new { likeQuery, projectId = request.ProjectId })
                    ).ToList();

                    return results;
                }
            }
        }
    }
}
