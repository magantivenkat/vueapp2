using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GoRegister.Areas.Admin.Features.Emails
{
    public static class CheckBatchStatus
    {
        public class Query : IRequest<Response>
        {
            public Guid BatchId { get; set; }
        }

        public class Response
        {
            public int ToSend { get; set; }
            public int Sent { get; internal set; }
            public int Failed { get; internal set; }
            public int Total { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, Response>
        {
            private readonly ApplicationDbContext _context;

            public QueryHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var emailBatch = await _context.EmailAuditBatches.Include(b => b.EmailAudits).SingleOrDefaultAsync(batch => batch.BatchId == request.BatchId);
                
                var emailsToSend = emailBatch.EmailAudits.Count(ea => ea.StatusId == EmailAuditStatus.Send);
                var emailsSent = emailBatch.EmailAudits.Count(ea => ea.StatusId == EmailAuditStatus.Success);
                var emailsFailed = emailBatch.EmailAudits.Count(ea =>ea.StatusId == EmailAuditStatus.Failed);

                return new Response
                {
                    ToSend = emailsToSend,
                    Sent = emailsSent,
                    Failed = emailsFailed,
                    Total = emailsToSend + emailsSent + emailsFailed
                };
            }
        }
    }
}



