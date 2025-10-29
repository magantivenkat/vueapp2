using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Domain.Registration.Framework;
using GoRegister.ApplicationCore.Domain.Reports.Framework;
using GoRegister.ApplicationCore.Domain.Reports.Models;
using GoRegister.ApplicationCore.Framework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Encodings.Web;

namespace GoRegister.ApplicationCore.Domain.Reports
{
    public interface IReportService
    {
        byte[] ExportToExcel(IEnumerable<string> headers, IEnumerable<IDictionary<string, object>> data);
        ReportContext GetReportContext();
        Task<ReportDataModel> RunReport(ReportViewModel model, ReportContext reportContext);
    }

    public class ReportService : IReportService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IFieldDriverAccessor _fieldDriverAccessor;
        private readonly ProjectTenant _project;
        private readonly ILogger<ReportService> _logger;
        private readonly IEnumerable<IGroupBy> _groupBys;

        public ReportService(ApplicationDbContext context, IConfiguration configuration, IFieldDriverAccessor fieldDriverAccessor, ProjectTenant project, ILogger<ReportService> logger, IEnumerable<IGroupBy> groupBys)
        {
            _context = context;
            _configuration = configuration;
            _fieldDriverAccessor = fieldDriverAccessor;
            _project = project;
            _logger = logger;
            _groupBys = groupBys;
        }


        public ReportContext GetReportContext()
        {
            var reportContext = new ReportContext();
            var userReportProvider = new UserReportProvider(_context);
            var fieldReportProvider = new FieldReportProvider(_context);
            userReportProvider.Build(reportContext);
            fieldReportProvider.Build(reportContext);
            return reportContext;
        }

        public async Task<ReportDataModel> RunReport(ReportViewModel model, ReportContext reportContext)
        {
            var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            var compiler = new SqlServerCompiler();
            var headers = new Dictionary<string, string>();

            var db = new QueryFactory(connection, compiler);

            Query query;

            var joinsToApply = new HashSet<string>();
            var joinDictionary = new Dictionary<string, IJoin>();

            if (model.Type == ReportViewModel.ReportType.Delegates)
            {

                query = new Query("DelegateUser")
                    .Join("User", "User.Id", "DelegateUser.Id")
                    .Join("RegistrationType", "RegistrationType.Id", "DelegateUser.RegistrationTypeId")
                    .Join("RegistrationStatus", "RegistrationStatus.Id", "DelegateUser.RegistrationStatusId");

                // apply selects
                foreach (var select in reportContext.GetSelects(model.SelectedFields))
                {
                    select.JoinsOLD.ForEach(e => joinsToApply.Add(e));
                    foreach(var join in select.Joins) foreach(var sj in join.GetJoins()) joinDictionary[sj.Alias] = sj;
                    select.Execute(query);
                }


                // apply order by
                if (!string.IsNullOrWhiteSpace(model.OrderByKey))
                {
                    var select = reportContext.Selects.FirstOrDefault(e => e.Key == model.OrderByKey);

                    if (model.OrderByDirection == ReportViewModel.ReportOrder.asc)
                    {
                        query.OrderBy(select.SelectStatement);
                    }
                    else
                    {
                        query.OrderByDesc(select.SelectStatement);
                    }
                    select.JoinsOLD.ForEach(e => joinsToApply.Add(e));
                }

                query.Where(q =>
                {
                    return ApplyFilters(q);
                });

                //ApplyJoins(query);
                foreach (var j in joinDictionary) j.Value.Apply(query);
            }

            else if (model.Type == ReportViewModel.ReportType.Summary)
            {
                //var groupBy = reportContext.GroupBys.FirstOrDefault(e => e.Key == model.GroupByStartFrom);
                //query = groupBy.CreateQuery();
                //var groupBy = new SingleSelectGroupBy()
                //{
                //    ModelType = "SingleSelectField",
                //    FieldId = 7,
                //    FormId = 2,
                //    DisplayName = "something",
                //    Selects = new HashSet<string>() { "Name" }
                //};

                //var groupBy2 = new RegistrationStatusGroupBy
                //{
                //    Selects = new HashSet<string>() { "Name" }
                //};

                var serializer = new Newtonsoft.Json.JsonSerializer
                {
                    ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
                };

                //model.SelectedGroupBys.Add(JToken.FromObject(groupBy, serializer));
                //model.SelectedGroupBys.Add(JToken.FromObject(groupBy2, serializer));

                var groupByTypes = _groupBys.ToDictionary(e => e.Type, e => e);

                var groupBys = model.SelectedGroupBys.Select(e =>
                {
                    var type = groupByTypes[e["modelType"].ToString()];
                    return (IGroupBy)e.ToObject(type.GetType());
                });

                var subQuery = new Query("DelegateUser")
                    .Join("User", "User.Id", "DelegateUser.Id")
                    .Join("RegistrationType", "RegistrationType.Id", "DelegateUser.RegistrationTypeId")
                    .Join("RegistrationStatus", "RegistrationStatus.Id", "DelegateUser.RegistrationStatusId");

                subQuery.Where("User.ProjectId", _project.Id);

                var subQueryContext = new QueryContext(subQuery, _project.Id);

                foreach (var gb in groupBys)
                {
                    gb.ApplyGroupBy(subQueryContext);
                }

                subQuery.SelectRaw("count(DelegateUser.Id) as [Total]");
                subQuery.SelectRaw("count(*) * 100.0 / sum(count(*)) over() as [Percentage]");

                foreach (var join in subQueryContext.Joins)
                {
                    //joinDictionary.Add(join.Alias, join);
                    foreach (var subJoin in join.GetJoins())
                    {
                        joinDictionary[subJoin.Alias] = subJoin;
                    }
                }

                //TODO: move to use IJoin
                ApplyFilters(subQuery);
                foreach (var join in joinDictionary)
                {
                    join.Value.Apply(subQuery);
                }




                //ApplyJoins(subQuery);
                var mainGroupBy = groupBys.First();

                query = new Query(mainGroupBy.GroupByTable);

                // cross join any additional tables
                foreach (var gb in groupBys.Skip(1))
                {
                    query.CrossJoin(gb.GroupByTable);
                }

                foreach (var gb in groupBys)
                {
                    var gbVM = reportContext.GroupBys.FirstOrDefault(e => e.Key == gb.GetKey());

                    //gb.ApplySelects(query);
                    var selects = gb.GetSelects();
                    foreach (var s in gb.Selects)
                    {
                        var select = selects.First(e => e.Key == s);
                        query.Select($"{select.QueryValue} as {select.Alias}");


                        headers.Add(select.Alias, gbVM.DisplayName + " " + select.HeaderName);
                    }

                    gb.ApplyResultsQuery(query);
                }

                query.LeftJoin(subQuery.As("userData"), j =>
                {
                    foreach (var gb in groupBys)
                    {
                        j.On("userData." + gb.Join.Item1, gb.Join.Item2);
                    }

                    return j;
                });

                query.SelectRaw("ISNULL([userData].[Total],0) as [Total]");
                headers.Add("Total", "Total");

                query.SelectRaw("CAST(isnull([userData].[percentage], 0) AS DECIMAL(18, 2)) as [Percentage]");
                headers.Add("Percentage", "Percentage");

                // skip empty rows
                if (model.GroupBySkipRowsWithNoData)
                {
                    query.Where("userdata.total", ">", 0);
                }
            }
            else
            {
                throw new NotImplementedException();
            }



            // apply filters
            Query ApplyFilters(Query filterQuery)
            {
                // filter users for the current project
                filterQuery.Where("User.ProjectId", _project.Id);
                // filter out test users
                filterQuery.Where("DelegateUser.IsTest", false);
                foreach (var filter in model.SelectedFilters)
                {
                    var reportFilter = reportContext.Filters.FirstOrDefault(e => e.Key == (string)filter["key"]);
                    var reportFilterModel = (IReportFilterModel)filter.ToObject(reportFilter.GetFilterType());
                    if (reportFilter != null)
                    {
                        //reportFilter.JoinsOld.ForEach(e => joinsToApply.Add(e));
                        foreach (var join in reportFilter.Joins)
                        {
                            //joinDictionary.Add(join.Alias, join);
                            foreach (var subJoin in join.GetJoins())
                            {
                                joinDictionary[subJoin.Alias] = subJoin;
                            }
                        }

                        reportFilter.Apply(filterQuery, reportFilterModel);
                    }
                }

                return filterQuery;
            }

            void ApplyJoins(Query joinQuery)
            {
                // apply joins
                foreach (var joinAlias in joinsToApply)
                {
                    reportContext.Joins[joinAlias](joinQuery);
                }
            }



            SqlResult kataResult = compiler.Compile(query);

            _logger.LogDebug(kataResult.RawSql);

            var results = (await db.GetAsync(query)).Select(e => e as IDictionary<string, object>);
            return new ReportDataModel
            {
                Results = results,
                Headers = headers
            };
        }

        public byte[] ExportToExcel(IEnumerable<string> headers, IEnumerable<IDictionary<string, object>> data)
        {
            using (var p = new ExcelPackage())
            {
                var ws = p.Workbook.Worksheets.Add("Data");

                int headerColumnNumber = 1;
                foreach (var col in headers)
                {
                    ws.Cells[1, headerColumnNumber++].Value = HtmlEncoder.Default.Encode(col);
                }

                var rowNumber = 2;

                foreach (var row in data)
                {
                    int columnNumber = 1;
                    foreach (var col in row)
                    {
                        ws.Cells[rowNumber, columnNumber++].Value = col.Value;
                    }
                    rowNumber++;
                }

                return p.GetAsByteArray();
            }
        }
    }
}
