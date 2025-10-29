using AutoMapper;
using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Domain.Registration.Models;
using GoRegister.ApplicationCore.Framework.Domain;
using GoRegister.ApplicationCore.Services.Email;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GoRegister.Areas.Admin.Features.Emails
{
    public static class GetEmailPreviews
    {
        public class Query : IRequest<Result<Response>>
        {
            public Guid Id { get; set; }
        }

        public class Response
        {
            public IEnumerable<EmailAuditResponse> Emails { get; set; }
        }

        public class EmailAuditResponse
        {
            public int Id { get; set; }
            public string To { get; set; }
            public string Subject { get; set; }
            public string PreviewUrl { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, Result<Response>>
        {
            private readonly ApplicationDbContext _db;
            private readonly IMapper _mapper;
            private readonly IUrlHelper _urlHelper;

            public QueryHandler(ApplicationDbContext db, IMapper mapper, IUrlHelper urlHelper)
            {
                _db = db;
                _mapper = mapper;
                _urlHelper = urlHelper;
            }

            public async Task<Result<Response>> Handle(Query request, CancellationToken cancellationToken)
            {
                var batch = await _db.EmailAuditBatches
                    .Include(e => e.EmailAudits)
                    .FirstOrDefaultAsync(e => e.BatchId == request.Id);

                if(batch == null)
                {
                    return Result.ResourceNotFound<Response>("Batch");
                }

                var response = new Response();
                response.Emails = batch.EmailAudits.Select(e =>
                {
                    var item = _mapper.Map<EmailAuditResponse>(e);
                    item.PreviewUrl = _urlHelper.Action("PreviewEmail", "Emails", new { area = "Admin", id = item.Id, projectId = batch.ProjectId });
                    return item;
                });

                return Result.Ok(response);
            }
        }
    }
}
