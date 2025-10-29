using GoRegister.ApplicationCore.Framework.Dapper;
using GoRegister.ApplicationCore.Framework.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GoRegister.Areas.Admin.Features.Emails
{
    public static class EmailLookup
    {
        public class Query : IRequest<Result<Response>>
        {
            public string[] Values { get; set; }
            public int ProjectId { get; set; }
        }

        public class Response
        {
            public List<UserResult> Users { get; set; } = new List<UserResult>();
            public int TotalAdded { get; set; }
            public int TotalSent { get; set; }
            public int Duplicates { get; set; }
            public int TotalNotFound { get; set; }
            public HashSet<string> UsersNotFound { get; set; } = new HashSet<string>();

            public bool AllFound => TotalNotFound == 0;
        }

        public class QueryHandler : IRequestHandler<Query, Result<Response>>
        {
            private readonly ISlickConnection _slickConnection;


            public QueryHandler(ISlickConnection slickConnection)
            {
                _slickConnection = slickConnection;
            }

            public async Task<Result<Response>> Handle(Query request, CancellationToken cancellationToken)
            {
                if (!request.Values.Any())
                {
                    return Result.Ok(new Response());
                }

                var allEmails = request.Values.Where(e => !string.IsNullOrWhiteSpace(e));
                var emails = allEmails.ToHashSet(StringComparer.OrdinalIgnoreCase);

                var emailsCount = emails.Count();
                if(emailsCount > 999)
                {
                    //TODO: I think SQLite errors here, SQL server at 2100?
                }

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
and [user].email in @emails", new { emails, projectId = request.ProjectId })
                    ).ToList();

                    //TODO check counts match

                    return Result.Ok(new Response
                    {
                        Users = results,
                        TotalAdded = results.Count,
                        Duplicates = allEmails.Count() - emailsCount,
                        TotalSent = allEmails.Count()
                    });
                }
            }
        }
    }
}
