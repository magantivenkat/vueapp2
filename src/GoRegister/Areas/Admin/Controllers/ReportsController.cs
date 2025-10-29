using GoRegister.ApplicationCore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GoRegister.ApplicationCore.Data.Models;
using System.IO;
using CsvHelper;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using GoRegister.ApplicationCore.Domain.Reports.Models;
using GoRegister.ApplicationCore.Domain.Reports.Framework;
using GoRegister.ApplicationCore.Domain.Reports;

namespace GoRegister.Areas.Admin.Controllers
{
    public class ReportsController : ProjectAdminControllerBase
    {
        private readonly IReportService _reportService;
        private readonly ApplicationDbContext _context;

        public ReportsController(IReportService reportService, ApplicationDbContext context)
        {
            _reportService = reportService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var reports = await _context.DataQueries.ToListAsync();
            return View(reports);
        }

        public IActionResult Create()
        {
            var model = new ReportViewModel();
            SetupReportViewModel(model);

            return View("Vue", model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var dataQuery = await _context.DataQueries.FirstOrDefaultAsync(e => e.Id == id);
            if (dataQuery == null) return NotFound();

            var model = JsonConvert.DeserializeObject<ReportViewModel>(dataQuery.Document);
            model.Id = dataQuery.Id;
            model.Name = dataQuery.Name;

            SetupReportViewModel(model);

            return View("Vue", model);
        }

        private void SetupReportViewModel(ReportViewModel model, ReportContext reportContext)
        {
            model.ReportFields = reportContext.Selects.Select(e => new SelectListItem(e.Name, e.Key)).ToList();
            model.ReportFilters = reportContext.Filters.Select(e => e.GetViewModel()).ToList();
            model.GroupBys = reportContext.GroupBys.ToList();
        }

        private void SetupReportViewModel(ReportViewModel model)
        {
            var reportContext = _reportService.GetReportContext();
            SetupReportViewModel(model, reportContext);
        }

        [HttpPost]
        public async Task<IActionResult> GetSummaryData([FromBody] ReportViewModel model)
        {
            var reportContext = _reportService.GetReportContext();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var resultModel = await _reportService.RunReport(model, reportContext);
            var results = resultModel.Results;

            var vm = new GeneratedReportViewModel();
            var table = new List<IEnumerable<object>>();
            foreach (var result in results)
            {
                var values = new List<object>();
                foreach (var col in result)
                {
                    values.Add(col.Value);
                }
                table.Add(values);
            }

            var headers = reportContext.GetHeaders(model);

            vm.Table = table;
            vm.Results = results;
            vm.Headers = resultModel.Headers.Select(e => e.Value);
            stopwatch.Stop();
            vm.QueryTimeElapsed = stopwatch.ElapsedMilliseconds;

            return Json(vm);
        }

        [HttpPost]
        public async Task<IActionResult> GetData([FromBody] ReportViewModel model)
        {
            var reportContext = _reportService.GetReportContext();


            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var results = (await _reportService.RunReport(model, reportContext)).Results;

            var vm = new GeneratedReportViewModel();
            var table = new List<IEnumerable<object>>();
            foreach (var result in results)
            {
                var values = new List<object>();
                foreach (var col in result)
                {
                    values.Add(col.Value);
                }
                table.Add(values);
            }

            var headers = reportContext.GetHeaders(model);

            vm.Table = table;
            vm.Results = results;
            vm.Headers = headers;
            stopwatch.Stop();
            vm.QueryTimeElapsed = stopwatch.ElapsedMilliseconds;

            return PartialView(vm);
        }

        //[HttpPost]
        public async Task<IActionResult> ExportReport(string query)
        {
            var model = JsonConvert.DeserializeObject<ReportViewModel>(query);
            var reportContext = _reportService.GetReportContext();

            var results = (await _reportService.RunReport(model, reportContext)).Results;
            var headers = reportContext.GetHeaders(model);
            var spreadsheet = _reportService.ExportToExcel(headers, results);
            return File(spreadsheet, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Report_{DateTime.Now.ToString("yyyy-MMM-dd-HHmm")}.xlsx");
        }

        public async Task<IActionResult> GetSheetsShareUrl(int id)
        {
            var dataQuery = await _context.DataQueries.FirstOrDefaultAsync(e => e.Id == id);
            if (dataQuery == null) return NotFound();

            var url = $"=IMPORTDATA('https://localhost:5021/admin/project/{dataQuery.ProjectId}/reports/SheetShare/{dataQuery.AccessLinkId}')";
            return Json(new
            {
                Url = url
            });
        }

        [AllowAnonymous]
        public async Task<IActionResult> SheetShare(string id)
        {
            var dataQuery = await _context.DataQueries.FirstOrDefaultAsync(e => e.AccessLinkId == id);
            if (dataQuery == null) return NotFound();

            var model = JsonConvert.DeserializeObject<ReportViewModel>(dataQuery.Document);
            model.Id = dataQuery.Id;

            var reportContext = _reportService.GetReportContext();

            var resultModel = await _reportService.RunReport(model, reportContext);
            var results = resultModel.Results;
            var headers = reportContext.GetHeaders(model);

            var csv = WriteCsvToMemory(headers, results);
            return File(csv, "text/csv", $"Report_{DateTime.Now.ToString("yyyy-MMM-dd-HHmm")}.csv");
        }

        public byte[] WriteCsvToMemory(IEnumerable<string> headers, IEnumerable<IDictionary<string, object>> records)
        {
            using (var memoryStream = new MemoryStream())
            using (var streamWriter = new StreamWriter(memoryStream))
            using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
            {
                foreach(var header in headers)
                {
                    csvWriter.WriteField(header);
                }
                csvWriter.NextRecord();

                foreach (var record in records)
                {
                    foreach(var col in record)
                    {
                        csvWriter.WriteField(col.Value);
                    }

                    csvWriter.NextRecord();
                }
                streamWriter.Flush();
                return memoryStream.ToArray();
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveReport([FromBody] ReportDataViewModel model)
        {
            DataQuery report;
            if (model.Id.HasValue)
            {
                report = await _context.DataQueries.FirstOrDefaultAsync(e => e.Id == model.Id);
                if (report == null) return NotFound();
            }
            else
            {
                report = new DataQuery();
                report.AccessLinkId = Guid.NewGuid().ToString();
                report.CreatedById = 1;
                report.CreatedUtc = SystemTime.UtcNow;

                _context.DataQueries.Add(report);
            }

            report.Document = JsonConvert.SerializeObject(model);
            report.Name = string.IsNullOrWhiteSpace(model.Name) ? "Unnamed" : model.Name;
            await _context.SaveChangesAsync();

            return Json(new { Id = report.Id });
        }
        
    }
}
