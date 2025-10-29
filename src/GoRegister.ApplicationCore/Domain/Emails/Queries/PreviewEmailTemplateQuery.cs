using GoRegister.ApplicationCore.Data;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Emails.Queries
{
    public static class PreviewEmailTemplate
    {
        public class Query : IRequest<string>
        {
            public int AttendeeId { get; set; }
            public int EmailTemplateId { get; set; }
            public string EmailBody { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, string>
        {
            private readonly ApplicationDbContext _db;

            public QueryHandler(ApplicationDbContext db)
            {
                _db = db;
            }

            public async Task<string> Handle(Query query, CancellationToken cancellationToken)
            {
                var emailTemplate = await _db.EmailLayouts.FindAsync(query.EmailTemplateId);
                return emailTemplate == null ? query.EmailBody : emailTemplate.Html.Replace("{{email_content}}", query.EmailBody) ;
            }
        }
    }
}
