using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Domain.Reports;
using GoRegister.ApplicationCore.Domain.Reports.Framework;
using GoRegister.ApplicationCore.Domain.Reports.Models;
using GoRegister.ApplicationCore.Framework.Domain;
using MediatR;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GoRegister.Areas.Admin.Features.Emails
{
    public static class CustomSearchUsers
    {
        public class Query : IRequest<Result<Response>>
        {
            public List<JToken> Filters { get; set; } = new List<JToken>();
        }

        public class Response
        {
            public IEnumerable<IDictionary<string, object>> Results { get; internal set; }
        }

        public class QueryHandler : IRequestHandler<Query, Result<Response>>
        {
            private readonly ApplicationDbContext _db;
            private readonly IReportService _reportService;

            static List<string> _fields = new List<string> { "Id", "FirstName", "LastName", "Email", "RegistrationType", "RegistrationStatus" };

            public QueryHandler(ApplicationDbContext db, IReportService reportService)
            {
                _db = db;
                _reportService = reportService;
            }

            public async Task<Result<Response>> Handle(Query request, CancellationToken cancellationToken)
            {
                var reportContext = _reportService.GetReportContext();

                var searchModel = new ReportViewModel()
                {
                    SelectedFilters = request.Filters,
                    SelectedFields = _fields,
                    OrderByKey = "FirstName"
                };

                var resultModel = await _reportService.RunReport(searchModel, reportContext);
                var results = resultModel.Results;
                var response = new Response
                {
                    Results = results
                };

                return Result.Ok(response);
            }
        }
    }
}
