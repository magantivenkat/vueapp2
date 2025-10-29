using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Domain.Reports;
using GoRegister.ApplicationCore.Domain.Reports.Framework;
using GoRegister.ApplicationCore.Framework.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GoRegister.Areas.Admin.Features.Emails
{
    public static class Send
    {
        public class Query : IRequest<Result<Response>>
        {
            public int Id { get; set; }
        }

        public class Response
        {
            public IEnumerable<IReportFilterViewModel> Filters { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, Result<Response>>
        {
            private readonly ApplicationDbContext _db;
            private readonly IReportService _reportService;

            public QueryHandler(ApplicationDbContext db, IReportService reportService)
            {
                _db = db;
                _reportService = reportService;
            }

            public async Task<Result<Response>> Handle(Query request, CancellationToken cancellationToken)
            {
                var reportContext = _reportService.GetReportContext();

                var response = new Response()
                {
                    Filters = reportContext.Filters.Select(e => e.GetViewModel())
                };

                return Result.Ok(response);
            }
        }
    }
}
