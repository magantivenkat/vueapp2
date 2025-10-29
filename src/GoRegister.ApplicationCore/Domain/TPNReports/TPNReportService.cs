using AutoMapper;
using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.TPNReports.Models;
using GoRegister.ApplicationCore.Extensions;
using GoRegister.ApplicationCore.Framework;
using GoRegister.ApplicationCore.Framework.DataTables;
using GoRegister.ApplicationCore.Framework.Multitenancy;
using GoRegister.ApplicationCore.Services.Email;
using GoRegister.ApplicationCore.Services.FileStorage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.TPNReports
{

    public interface ITPNReportService
    {
        Task<List<TPNReportRequestModel>> GetTPNReportRequestsList();
        List<TPNReportRequestModel> FilterTPNReportRequestTable(DataTables.DtParameters dtParameters, List<TPNReportRequestModel> tpnReportRequests);
        List<TPNReportRequestModel> OrderTPNReportRequestTable(DataTables.DtParameters dtParameters, List<TPNReportRequestModel> tpnReportRequests);
        Task<TPNReportRequest> Save(TPNReportModel model);

        Task GenrateAutoTPNReport();
        Task GenerateManualTPNReport(ProjectTenant projectTenant, int requestId);

    }
    public class TPNReportService : ITPNReportService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IFileStorage _fileStorage;
        private readonly IEmailSendingService _emailSendingService;
        private readonly ITenantAccessor _tenantAccessor;
        private readonly ILogger<TPNReportService> _logger;
        private readonly IWebHostEnvironment _env;


        public TPNReportService(ApplicationDbContext context, IMapper mapper, IConfiguration configuration, IFileStorage fileStorage, IEmailSendingService emailSendingService, ITenantAccessor tenantAccessor, ILogger<TPNReportService> logger, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
            _fileStorage = fileStorage;
            _emailSendingService = emailSendingService;
            _tenantAccessor = tenantAccessor;
            _logger = logger;
            _env = env;

        }

        public async Task<List<TPNReportRequestModel>> GetTPNReportRequestsList()
        {

            var qry = await (from t in _context.TPNReportRequest
                             join c in _context.Clients on t.ClientId equals c.Id
                             join u in _context.Users on t.RequestedBy equals u.Id into grp
                             from g in grp.DefaultIfEmpty()
                             where t.ReportType == "Manual"
                             select new
                             {
                                 Id = t.Id,
                                 ClientId = t.ClientId,
                                 ClientName = c.Name,
                                 TPNCountry = t.TPNCountry,
                                 DateRequested = t.RequestedDate,
                                 RequestBy = g.FirstName + " " + g.LastName,
                                 DownloadPath = t.DownloadPath,
                                 ReportType = t.ReportType,
                                 ClientUuid = t.ClientUuid,
                                 ReportStatus = t.ReportStatusId == ReportStatus.New ? "New" : t.ReportStatusId == ReportStatus.ReportCreated ? "Report Created" : t.ReportStatusId == ReportStatus.Completed ? "Completed" : t.ReportStatusId == ReportStatus.Failed ? "Failed" : t.ReportStatusId == ReportStatus.DetailsNotFound ? "Details Not Found" : string.Empty
                             }).Distinct().OrderByDescending(d => d.DateRequested).ToListAsync();

            List<TPNReportRequestModel> list = new List<TPNReportRequestModel>();
            foreach (var item in qry)
            {
                TPNReportRequestModel model = new TPNReportRequestModel();

                model.Id = item.Id;
                model.ClientId = item.ClientId;
                model.ClientName = item.ClientName;
                model.TPNCountry = item.TPNCountry;
                model.DateRequested = item.DateRequested.ToString("MM/dd/yyyy HH:mm:ss");
                model.RequestBy = item.RequestBy;
                model.DownloadPath = item.DownloadPath;
                model.ReportType = item.ReportType;
                model.ReportStatus = item.ReportStatus;

                list.Add(model);
            }

            return list;
        }

        public List<TPNReportRequestModel> FilterTPNReportRequestTable(DataTables.DtParameters dtParameters, List<TPNReportRequestModel> tpnReportRequests)
        {
            var searchBy = dtParameters.Search?.Value;
            if (!string.IsNullOrEmpty(searchBy))
            {
                tpnReportRequests = tpnReportRequests
                    .Where(c => c.TPNCountry?.ToUpper().Contains(searchBy.ToUpper()) == true
                                || c.ClientName?.ToUpper().Contains(searchBy.ToUpper()) == true


                    ).ToList();
            }

            return tpnReportRequests;
        }

        public List<TPNReportRequestModel> OrderTPNReportRequestTable(DataTables.DtParameters dtParameters, List<TPNReportRequestModel> tpnReportRequests)
        {
            string orderCriteria;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "Id";
            }

            tpnReportRequests = orderAscendingDirection
                ? tpnReportRequests.AsQueryable().OrderByDynamic(orderCriteria, DataTablesOrderExtensions.Order.Asc).ToList()
                : tpnReportRequests.AsQueryable().OrderByDynamic(orderCriteria, DataTablesOrderExtensions.Order.Desc).ToList();
            return tpnReportRequests;
        }

        public async Task<TPNReportRequest> Save(TPNReportModel model)
        {
            TPNReportRequest tpnReportRequest = new TPNReportRequest();

            tpnReportRequest.TPNCountry = model.TPNCountry;
            tpnReportRequest.ClientId = model.ClientId;
            tpnReportRequest.ClientUuid = model.ClientUuid;
            //tpnReportRequest.ClientName = model.Name;
            tpnReportRequest.ReportType = model.ReportType;
            tpnReportRequest.ReportStatusId = model.ReportStatusId;
            tpnReportRequest.RequestedDate = model.RequestedDate;
            tpnReportRequest.RequestedBy = model.RequestedBy;

            _context.TPNReportRequest.Add(tpnReportRequest);

            await _context.SaveChangesAsync();

            return tpnReportRequest;
        }

        public async Task GenrateAutoTPNReport()
        {
            ProjectTenant _projectTenant = new ProjectTenant();

            DateTime endDate = DateTime.Now;
            DateTime startDate = DateTime.Now;

            string reportFrequencyVal = string.Empty;
            string executionTime = string.Empty;
            string weekDayVal = string.Empty;

            ReportFrequency reportFrequency;

            var mrfSysConfig = await (from td in _context.MRFSysConfig
                                      where td.MRFKey == "TPNReportFrequency"
                                      select td).ToListAsync();

            foreach (var item in mrfSysConfig)
            {
                reportFrequencyVal = item.MRFKeyVal;
            }

            Enum.TryParse<ReportFrequency>(reportFrequencyVal, out reportFrequency);

            if (reportFrequency == ReportFrequency.Weekly)
            {
                mrfSysConfig = await (from td in _context.MRFSysConfig
                                      where td.MRFKey == "TPNReportExecDay"
                                      select td).ToListAsync();

                foreach (var item in mrfSysConfig)
                {
                    weekDayVal = item.MRFKeyVal;
                }

                if (DateTime.Now.DayOfWeek.ToString() == weekDayVal)
                {
                    mrfSysConfig = await (from td in _context.MRFSysConfig
                                          where td.MRFKey == "TPNReportExecTime"
                                          select td).ToListAsync();

                    foreach (var item in mrfSysConfig)
                    {
                        executionTime = item.MRFKeyVal;
                    }

                    if (DateTime.Now.ToString("HH:mm") == executionTime)
                    {
                        endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
                        startDate = endDate.AddDays(-7);
                        await PopulateAutoTPNReportRequest(reportFrequency, startDate, endDate, _projectTenant);
                    }
                }
            }

            if (reportFrequency == ReportFrequency.FortNightly)
            {
                if (DateTime.Now.Day == 15)
                {
                    mrfSysConfig = await (from td in _context.MRFSysConfig
                                          where td.MRFKey == "TPNReportExecTime"
                                          select td).ToListAsync();

                    foreach (var item in mrfSysConfig)
                    {
                        executionTime = item.MRFKeyVal;
                    }

                    if (DateTime.Now.ToString("HH:mm") == executionTime)
                    {
                        endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 15, 23, 59, 59);
                        startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01, 00, 00, 00);
                        await PopulateAutoTPNReportRequest(reportFrequency, startDate, endDate, _projectTenant);
                    }
                }
            }

            if (reportFrequency == ReportFrequency.Monthly)
            {
                DateTime today = DateTime.Today;
                DateTime endOfMonth = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));

                if (today.Day == endOfMonth.Day)
                {
                    mrfSysConfig = await (from td in _context.MRFSysConfig
                                          where td.MRFKey == "TPNReportExecTime"
                                          select td).ToListAsync();

                    foreach (var item in mrfSysConfig)
                    {
                        executionTime = item.MRFKeyVal;
                    }

                    if (DateTime.Now.ToString("HH:mm") == executionTime)
                    {
                        endDate = new DateTime(endOfMonth.Year, endOfMonth.Month, endOfMonth.Day, 23, 59, 59);
                        startDate = new DateTime(endOfMonth.Year, endOfMonth.Month, 01, 00, 00, 00);
                        await PopulateAutoTPNReportRequest(reportFrequency, startDate, endDate, _projectTenant);
                    }
                }
            }
        }

        public async Task<bool> PopulateAutoTPNReportRequest(ReportFrequency reportFrequency, DateTime startDate, DateTime endDate, ProjectTenant projectTenant)
        {
            _tenantAccessor.SetTenant(projectTenant);
            _logger.LogInformation("Step 1: TPN Report Auto data preparatio");
            var reportDetailsAuto = await (from td in _context.TPNReportDetails
                                           join ta in _context.TPNCountryAdminMapping on td.ClientUuid equals ta.ClientUuid
                                           join ts in _context.TPNReportDataStatus on td.Id equals ts.TPNReportDetailsId
                                           where td.TPNCountry == ta.TPNCountry && ta.IsDeleted == false && ta.ReportFrequency == ReportFrequency.Weekly
                                           && td.FormDateTimeCreated >= startDate && td.FormDateTimeCreated <= endDate && ts.MRFClientResponseDetailsId == td.MRFResponseDetailsId && ts.IsSendWeekly == false
                                           select new
                                           {
                                               MRFResponseDetailsId = td.MRFResponseDetailsId,
                                               MRFFormName = td.MRFFormName,
                                               ClientId = td.ClientId,
                                               ClientUuid = td.ClientUuid,
                                               GBTClient = td.GBTClient,
                                               ClientName = td.ClientName,
                                               TPNCountry = td.TPNCountry,
                                               TPNSharedMailbox = td.TPNSharedMailbox,
                                               ContactFirstName = td.ContactFirstName,
                                               ContactLastName = td.ContactLastName,
                                               ContactEmail = td.ContactEmail,
                                               DepartureDate = td.DepartureDate,
                                               Destination = td.Destination,
                                               EventName = td.EventName,
                                               FormDateTimeCreated = td.FormDateTimeCreated,
                                               AdminUserId = ta.AdminUserId,
                                               GAMEmail = ta.GAMEmail
                                           }).Distinct().ToListAsync();


            var requestDetailsAuto = reportDetailsAuto.Select(o => new { ClientId = o.ClientId, ClientUuid = o.ClientUuid, ClientName = o.ClientName, TPNCountry = o.TPNCountry, AdminUserId = o.AdminUserId, GAMEmail = o.GAMEmail }).Distinct();

            List<int> requestIds = new List<int>();

            foreach (var requestDetail in requestDetailsAuto)
            {
                if (!_context.TPNReportRequest.Any(e => e.ClientUuid == requestDetail.ClientUuid && e.RequestedBy == requestDetail.AdminUserId && e.TPNCountry == requestDetail.TPNCountry && e.ReportStatusId == ReportStatus.New && e.ReportType == "Auto"))
                {
                    TPNReportRequest tpnReportRequest = new TPNReportRequest();

                    tpnReportRequest.ClientId = (int)requestDetail.ClientId;
                    tpnReportRequest.ClientUuid = requestDetail.ClientUuid;
                    tpnReportRequest.RequestedBy = requestDetail.AdminUserId;
                    tpnReportRequest.ReportStatusId = ReportStatus.New;
                    tpnReportRequest.TPNCountry = requestDetail.TPNCountry;
                    tpnReportRequest.RequestedDate = DateTime.Now;
                    tpnReportRequest.ReportType = "Auto";
                    tpnReportRequest.GAMEmail = requestDetail.GAMEmail;
                    tpnReportRequest.ReportFrequency = ReportFrequency.Weekly;

                    _context.Add(tpnReportRequest);

                    await _context.SaveChangesAsync();

                    requestIds.Add(tpnReportRequest.Id);
                }
            }

            if (requestIds.Count > 0)
            {
                bool result = await CreateTPNReportAuto(requestIds, reportFrequency);

                if (result)
                {
                    result = await SendEmailsToAutoRequest(requestIds);
                    _logger.LogInformation("Step 8: TPN Report Auto process completed.");
                }
            }

            return true;
        }


        public async Task GenerateManualTPNReport(ProjectTenant projectTenant, int requestId)
        {
            _tenantAccessor.SetTenant(projectTenant);
            _logger.LogInformation("Step 1: TPN Report Manual started.");
            var reportDetails = await (from td in _context.TPNReportDetails
                                       join r in _context.TPNReportRequest on td.ClientUuid equals r.ClientUuid
                                       join m in _context.MRFClientResponseDetails on r.ClientUuid equals m.ClientGUID
                                       join u in _context.Users on r.RequestedBy equals u.Id
                                       where r.ReportType == "Manual" && td.TPNCountry == r.TPNCountry && r.ReportStatusId == ReportStatus.New && r.DownloadPath == null && r.Id == requestId
                                       && td.MRFResponseDetailsId==m.Id
                                       select new TPNRequestReportDetailsModel
                                       {
                                           RequestId = r.Id,
                                           MRFResponseDetailsId = td.MRFResponseDetailsId,
                                           MRFFormName = td.MRFFormName,
                                           ClientId = td.ClientId,
                                           ClientUuid = td.ClientUuid,
                                           GBTClient = td.GBTClient,
                                           ClientName = td.ClientName,
                                           TPNCountry = td.TPNCountry,
                                           TPNSharedMailbox = td.TPNSharedMailbox,
                                           ContactFirstName = td.ContactFirstName,
                                           ContactLastName = td.ContactLastName,
                                           ContactEmail = td.ContactEmail,
                                           DepartureDate = td.DepartureDate,
                                           Destination = td.Destination,
                                           EventName = td.EventName,
                                           FormDateTimeCreated = m.DateTimeCreated,
                                           AdminUserId = r.RequestedBy,
                                           AdminUserName = u.FirstName + " " + u.LastName
                                       }).ToListAsync();


            if (reportDetails.Count > 0)
            {
                var requestIds = reportDetails.Select(o => new { RequestId = o.RequestId, ClientUuid = o.ClientUuid, ClientName = o.ClientName, TPNCountry = o.TPNCountry }).Distinct().ToList();
                _logger.LogInformation("Step 2: TPN Report Manual with record details count more than 0");
                foreach (var ids in requestIds)
                {
                    var requestDetails = reportDetails.Where(r => r.ClientUuid == ids.ClientUuid && r.RequestId == ids.RequestId && r.TPNCountry == ids.TPNCountry);

                    var details = (from d in requestDetails
                                   where d.ClientUuid == ids.ClientUuid
                                   orderby d.MRFResponseDetailsId ascending
                                   select new TPNReportExcelModel
                                   {
                                       RequestDate = d.FormDateTimeCreated.ToString("MM/dd/yyyy HH:mm:ss"),
                                       TPNServicingCountry = d.TPNCountry,
                                       TPNSharedMailbox = d.TPNSharedMailbox,
                                       GBTClient = d.GBTClient,
                                       CompanyName = d.ClientName,
                                       ClientContactFirstName = d.ContactFirstName,
                                       ClientContactLastName = d.ContactLastName,
                                       ClientContactEmail = d.ContactEmail,
                                       DepartureDate = d.DepartureDate.ToString("MM/dd/yyyy"),
                                       Destination = d.Destination,
                                       EventName = d.EventName
                                   }).ToList();


                    DataTable dtSrc = LINQResultToDataTable<TPNReportExcelModel>(details);
                    dtSrc.TableName = ids.ClientName;
                    dtSrc.Columns[0].ColumnName = "Request Date";
                    dtSrc.Columns[1].ColumnName = "TPN Servicing Country";
                    dtSrc.Columns[2].ColumnName = "TPN Shared Mailbox";
                    dtSrc.Columns[3].ColumnName = "GBT Client";
                    dtSrc.Columns[4].ColumnName = "Company Name";
                    dtSrc.Columns[5].ColumnName = "Client Contact First Name";
                    dtSrc.Columns[6].ColumnName = "Client Contact Last Name";
                    dtSrc.Columns[7].ColumnName = "Client Contact Email";
                    dtSrc.Columns[8].ColumnName = "Departure Date";
                    dtSrc.Columns[9].ColumnName = "Destination";
                    dtSrc.Columns[10].ColumnName = "Event Name";

                    using (ExcelPackage objExcelPackage = new ExcelPackage())
                    {
                        _logger.LogDebug("Step 3: TPN Report Manual preparation of excel report");
                        //Create the worksheet    
                        ExcelWorksheet objWorksheet = objExcelPackage.Workbook.Worksheets.Add(dtSrc.TableName);
                        //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1    
                        objWorksheet.Cells["A1"].LoadFromDataTable(dtSrc, true);

                        int rowStart = 1;
                        int colStart = 1;
                        int rowEnd = dtSrc.Rows.Count + 1;
                        int colEnd = 11;

                        using (ExcelRange objRange = objWorksheet.Cells[rowStart, colStart, rowEnd, colEnd])
                        {
                            //objRange.Style.Font.SetFromFont(new Font("Calibri", 10));
                            //objRange.AutoFitColumns();

                            objRange.Style.Border.Top.Style =
                            objRange.Style.Border.Bottom.Style =
                            objRange.Style.Border.Left.Style =
                            objRange.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        }

                        //Format the header    
                        using (ExcelRange objRange = objWorksheet.Cells["A1:K1"])
                        {
                            objRange.Style.Font.Bold = true;
                            objRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            objRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            objRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            objRange.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(234, 234, 234));
                        }

                        var documentName = "TPNReport_" + ids.ClientName + "_" + ids.TPNCountry + "_" + ids.RequestId + ".xlsx";
                        var tpnRptSettings = _configuration.GetSection("AwsS3TPNRptManualUpload").Get<TPNReportsAwsSettings>();

                        var projectUuid = await (from p in _context.Projects
                                                 where p.Prefix == ids.ClientUuid
                                                 select p).ToListAsync();

                        string projectUniqueId = string.Empty;

                        foreach (var uuid in projectUuid)
                        {
                            projectUniqueId = Convert.ToString(uuid.UniqueId);
                        }

                        var path = StringExtensions.CombineWithSlash(
                           "Project",
                           projectUniqueId,
                           tpnRptSettings.ParentFolder,
                           tpnRptSettings.SubFolder,
                           documentName);

                        string p_strPath = string.Empty;
                        _logger.LogInformation("Step 4: TPN Report Manual upload report into S3 bucket: ", path);
                        using (var stream = new MemoryStream(objExcelPackage.GetAsByteArray()))
                        {
                            var result = await _fileStorage.UploadFile(path, stream);
                            p_strPath = result.AbsoluteUrl;
                        }

                        TPNReportRequest requestDetailsRow = _context.TPNReportRequest.FirstOrDefault(e => e.Id == ids.RequestId);
                        requestDetailsRow.DownloadPath = p_strPath;
                        requestDetailsRow.ReportStatusId = ReportStatus.Completed;
                    }
                }
            }
            else
            {
                _logger.LogInformation("Step 5: TPN Report Manual data not found");
                TPNReportRequest requestDetailsRow = _context.TPNReportRequest.FirstOrDefault(e => e.Id == requestId);
                requestDetailsRow.ReportStatusId = ReportStatus.DetailsNotFound;
            }
            _logger.LogInformation("Step 6: TPN Report Manual completed.");
            await _context.SaveChangesAsync();
        }

        //public async Task<bool> CreateTPNReportAuto(List<int> requestIds, ReportFrequency reportFrequency)
        //{
        //    _logger.LogDebug("TPN Report Auto 1");
        //    var reportDetails = await (from td in _context.TPNReportDetails
        //                               join r in _context.TPNReportRequest on td.ClientUuid equals r.ClientUuid
        //                               join u in _context.Users on r.RequestedBy equals u.Id
        //                               join ts in _context.TPNReportDataStatus on td.Id equals ts.TPNReportDetailsId
        //                               where (requestIds.Contains(r.Id) || r.ReportStatusId== ReportStatus.New) && r.TPNCountry == td.TPNCountry
        //                               select new TPNRequestReportDetailsModel
        //                               {
        //                                   TPNReportDetailsId = td.Id,
        //                                   RequestId = r.Id,
        //                                   MRFResponseDetailsId = td.MRFResponseDetailsId,
        //                                   MRFFormName = td.MRFFormName,
        //                                   ClientId = td.ClientId,
        //                                   ClientUuid = td.ClientUuid,
        //                                   GBTClient = td.GBTClient,
        //                                   ClientName = td.ClientName,
        //                                   TPNCountry = td.TPNCountry,
        //                                   TPNSharedMailbox = td.TPNSharedMailbox,
        //                                   ContactFirstName = td.ContactFirstName,
        //                                   ContactLastName = td.ContactLastName,
        //                                   ContactEmail = td.ContactEmail,
        //                                   DepartureDate = td.DepartureDate,
        //                                   Destination = td.Destination,
        //                                   EventName = td.EventName,
        //                                   FormDateTimeCreated = td.FormDateTimeCreated,
        //                                   AdminUserId = r.RequestedBy,
        //                                   AdminUserName = u.FirstName + " " + u.LastName,
        //                                   GAManagerEmail = r.GAMEmail,
        //                                   IsSendWeekly = ts.IsSendWeekly,
        //                                   IsSendMonthly = ts.IsSendMonthly,
        //                                   IsSendFortNightly = ts.IsSendFortNightly
        //                               }).Distinct().ToListAsync();

        //    if (reportFrequency == ReportFrequency.Weekly)
        //    {
        //        reportDetails = reportDetails.Where(p => p.IsSendWeekly == false).ToList();
        //    }
        //    else if (reportFrequency == ReportFrequency.Monthly)
        //    {
        //        reportDetails = reportDetails.Where(p => p.IsSendMonthly == false).ToList();
        //    }
        //    else
        //    {
        //        reportDetails = reportDetails.Where(p => p.IsSendFortNightly == false).ToList();
        //    }


        //    var adminUsers = reportDetails.Select(o => new { AdminUserId = o.AdminUserId, AdminUserName = o.AdminUserName }).Distinct().ToList();

        //    foreach (var user in adminUsers)
        //    {
        //        var clients = reportDetails.Where(r => r.AdminUserId == user.AdminUserId).Select(o => new { ClientUuid = o.ClientUuid, ClientName = o.ClientName,TPNCountry = o.TPNCountry }).Distinct().ToList();

        //        foreach (var client in clients)
        //        {
        //            using (ExcelPackage objExcelPackage = new ExcelPackage())
        //            {

        //                var details = (from d in reportDetails
        //                               where d.ClientUuid == client.ClientUuid && d.AdminUserId == user.AdminUserId && d.TPNCountry == client.TPNCountry
        //                               orderby d.MRFResponseDetailsId ascending
        //                               select new TPNReportExcelModel
        //                               {
        //                                   RequestDate = d.FormDateTimeCreated.ToString("MM/dd/yyyy HH:mm:ss"),
        //                                   TPNServicingCountry = d.TPNCountry,
        //                                   TPNSharedMailbox = d.TPNSharedMailbox,
        //                                   GBTClient = d.GBTClient,
        //                                   CompanyName = d.ClientName,
        //                                   ClientContactFirstName = d.ContactFirstName,
        //                                   ClientContactLastName = d.ContactLastName,
        //                                   ClientContactEmail = d.ContactEmail,
        //                                   DepartureDate = d.DepartureDate.ToString("MM/dd/yyyy"),
        //                                   Destination = d.Destination,
        //                                   EventName = d.EventName
        //                               }).Distinct().ToList();

        //                DataTable dtSrc = LINQResultToDataTable<TPNReportExcelModel>(details);
        //                dtSrc.TableName = client.ClientName;
        //                dtSrc.Columns[0].ColumnName = "Request Date";
        //                dtSrc.Columns[1].ColumnName = "TPN Servicing Country";
        //                dtSrc.Columns[2].ColumnName = "TPN Shared Mailbox";
        //                dtSrc.Columns[3].ColumnName = "GBT Client";
        //                dtSrc.Columns[4].ColumnName = "Company Name";
        //                dtSrc.Columns[5].ColumnName = "Client Contact First Name";
        //                dtSrc.Columns[6].ColumnName = "Client Contact Last Name";
        //                dtSrc.Columns[7].ColumnName = "Client Contact Email";
        //                dtSrc.Columns[8].ColumnName = "Departure Date";
        //                dtSrc.Columns[9].ColumnName = "Destination";
        //                dtSrc.Columns[10].ColumnName = "Event Name";


        //                ExcelWorksheet objWorksheet = objExcelPackage.Workbook.Worksheets.Add(dtSrc.TableName);

        //                objWorksheet.Cells["A1"].LoadFromDataTable(dtSrc, true);

        //                int rowStart = 1;
        //                int colStart = 1;
        //                int rowEnd = dtSrc.Rows.Count + 1;
        //                int colEnd = 11;

        //                using (ExcelRange objRange = objWorksheet.Cells[rowStart, colStart, rowEnd, colEnd])
        //                {
        //                    //objRange.Style.Font.SetFromFont(new Font("Calibri", 10));
        //                    //objRange.AutoFitColumns();

        //                    objRange.Style.Border.Top.Style =
        //                    objRange.Style.Border.Bottom.Style =
        //                    objRange.Style.Border.Left.Style =
        //                    objRange.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        //                }

        //                //Format the header    
        //                using (ExcelRange objRange = objWorksheet.Cells["A1:K1"])
        //                {
        //                    objRange.Style.Font.Bold = true;
        //                    objRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                    objRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //                    objRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
        //                    objRange.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(234, 234, 234));
        //                }
        //                _logger.LogDebug("TPN Report Auto 2");
        //                _logger.LogDebug("TPN Report Auto 2 user: " + user.AdminUserName);
        //                _logger.LogDebug("TPN Report Auto 2 client: " + client.ClientName);
        //                var documentName = "TPNReport_" + client.ClientName + "_" + client.TPNCountry +"_" + DateTime.Now.ToString("MMddyyyyhhmmss") + ".xlsx";

        //                _logger.LogDebug("TPN Report Auto 2 content Root path: " + _env.ContentRootPath);
        //                var filePath = (Path.Combine(_env.ContentRootPath, "wwwroot", "TPNReportAttachment"));
        //                _logger.LogDebug("TPN Report Auto 2 file path: " + filePath);
        //                if (!Directory.Exists(filePath))
        //                {
        //                    System.IO.Directory.CreateDirectory(filePath);
        //                }

        //                filePath = filePath + $@"{documentName}";

        //                using (FileStream fStream = File.Create(filePath))
        //                {
        //                    objExcelPackage.SaveAs(fStream);
        //                    fStream.Close();
        //                }

        //                var tpnRequestIds = reportDetails.Where(a => a.ClientUuid == client.ClientUuid && a.TPNCountry == client.TPNCountry && a.AdminUserId == user.AdminUserId).Select(o => new { RequestId = o.RequestId }).Distinct().ToArray();

        //                foreach (var item in tpnRequestIds)
        //                {
        //                    TPNReportRequest requestDetailsRow = _context.TPNReportRequest.FirstOrDefault(e => e.Id == item.RequestId);
        //                    //requestDetailsRow.DownloadPath = p_strPath;
        //                    requestDetailsRow.FileName = documentName;
        //                    //requestDetailsRow.ReportStatusId = result.AbsoluteUrl != null ? ReportStatus.ReportCreated : ReportStatus.Failed;
        //                    requestDetailsRow.ReportStatusId = ReportStatus.ReportCreated;
        //                }
        //            }

        //            var tpnDetailsIds = reportDetails.Where(a => a.ClientUuid == client.ClientUuid && a.TPNCountry == client.TPNCountry && a.AdminUserId == user.AdminUserId).Select(o => new { MRFClientResponseDetailsId = o.MRFResponseDetailsId, TPNReportDetailsId = o.TPNReportDetailsId }).Distinct().ToList();

        //            foreach (var tpnDetails in tpnDetailsIds)
        //            {
        //                TPNReportDataStatus tpnReportDataStatus = _context.TPNReportDataStatus.FirstOrDefault(e => e.TPNReportDetailsId == tpnDetails.TPNReportDetailsId && e.MRFClientResponseDetailsId == tpnDetails.MRFClientResponseDetailsId);
        //                tpnReportDataStatus.IsSendWeekly = true;
        //            }
        //        }
        //    }

        //    await _context.SaveChangesAsync();

        //    return true;
        //}

        public async Task<bool> CreateTPNReportAuto(List<int> requestIds, ReportFrequency reportFrequency)
        {
            _logger.LogInformation("Step 2: TPN Report Auto started");
            var reportDetails = await (from td in _context.TPNReportDetails
                                       join r in _context.TPNReportRequest on td.ClientUuid equals r.ClientUuid
                                       join m in _context.MRFClientResponseDetails on r.ClientUuid equals m.ClientGUID
                                       join u in _context.Users on r.RequestedBy equals u.Id
                                       join ts in _context.TPNReportDataStatus on td.Id equals ts.TPNReportDetailsId
                                       where requestIds.Contains(r.Id) && r.TPNCountry == td.TPNCountry && td.MRFResponseDetailsId==m.Id
                                       select new TPNRequestReportDetailsModel
                                       {
                                           TPNReportDetailsId = td.Id,
                                           RequestId = r.Id,
                                           MRFResponseDetailsId = td.MRFResponseDetailsId,
                                           MRFFormName = td.MRFFormName,
                                           ClientId = td.ClientId,
                                           ClientUuid = td.ClientUuid,
                                           GBTClient = td.GBTClient,
                                           ClientName = td.ClientName,
                                           TPNCountry = td.TPNCountry,
                                           TPNSharedMailbox = td.TPNSharedMailbox,
                                           ContactFirstName = td.ContactFirstName,
                                           ContactLastName = td.ContactLastName,
                                           ContactEmail = td.ContactEmail,
                                           DepartureDate = td.DepartureDate,
                                           Destination = td.Destination,
                                           EventName = td.EventName,
                                           FormDateTimeCreated = m.DateTimeCreated,
                                           AdminUserId = r.RequestedBy,
                                           AdminUserName = u.FirstName + " " + u.LastName,
                                           GAManagerEmail = r.GAMEmail,
                                           IsSendWeekly = ts.IsSendWeekly,
                                           IsSendMonthly = ts.IsSendMonthly,
                                           IsSendFortNightly = ts.IsSendFortNightly
                                       }).Distinct().ToListAsync();

            if (reportFrequency == ReportFrequency.Weekly)
            {
                reportDetails = reportDetails.Where(p => p.IsSendWeekly == false).ToList();
            }
            else if (reportFrequency == ReportFrequency.Monthly)
            {
                reportDetails = reportDetails.Where(p => p.IsSendMonthly == false).ToList();
            }
            else
            {
                reportDetails = reportDetails.Where(p => p.IsSendFortNightly == false).ToList();
            }


            var gamUsers = reportDetails.Select(o => new { GAManagerEmail = o.GAManagerEmail }).Distinct().ToList();

            foreach (var gamUser in gamUsers)
            {
                //var clients = reportDetails.Where(r => r.GAManagerEmail == gamUser.GAManagerEmail).Select(o => new { ClientUuid = o.ClientUuid, ClientName = o.ClientName, TPNCountry = o.TPNCountry }).Distinct().ToList();
                var clients = reportDetails.Where(r => r.GAManagerEmail == gamUser.GAManagerEmail).Select(o => new { ClientUuid = o.ClientUuid, ClientName = o.ClientName}).Distinct().ToList();

                foreach (var client in clients)
                {
                    using (ExcelPackage objExcelPackage = new ExcelPackage())
                    {

                        var details = (from d in reportDetails
                                       //where d.ClientUuid == client.ClientUuid && d.GAManagerEmail == gamUser.GAManagerEmail && d.TPNCountry == client.TPNCountry
                                       where d.ClientUuid == client.ClientUuid && d.GAManagerEmail == gamUser.GAManagerEmail
                                       orderby d.MRFResponseDetailsId ascending
                                       select new TPNReportExcelModel
                                       {
                                           RequestDate = d.FormDateTimeCreated.ToString("MM/dd/yyyy HH:mm:ss"),
                                           TPNServicingCountry = d.TPNCountry,
                                           TPNSharedMailbox = d.TPNSharedMailbox,
                                           GBTClient = d.GBTClient,
                                           CompanyName = d.ClientName,
                                           ClientContactFirstName = d.ContactFirstName,
                                           ClientContactLastName = d.ContactLastName,
                                           ClientContactEmail = d.ContactEmail,
                                           DepartureDate = d.DepartureDate.ToString("MM/dd/yyyy"),
                                           Destination = d.Destination,
                                           EventName = d.EventName
                                       }).Distinct().ToList();

                        DataTable dtSrc = LINQResultToDataTable<TPNReportExcelModel>(details);
                        dtSrc.TableName = client.ClientName;
                        dtSrc.Columns[0].ColumnName = "Request Date";
                        dtSrc.Columns[1].ColumnName = "TPN Servicing Country";
                        dtSrc.Columns[2].ColumnName = "TPN Shared Mailbox";
                        dtSrc.Columns[3].ColumnName = "GBT Client";
                        dtSrc.Columns[4].ColumnName = "Company Name";
                        dtSrc.Columns[5].ColumnName = "Client Contact First Name";
                        dtSrc.Columns[6].ColumnName = "Client Contact Last Name";
                        dtSrc.Columns[7].ColumnName = "Client Contact Email";
                        dtSrc.Columns[8].ColumnName = "Departure Date";
                        dtSrc.Columns[9].ColumnName = "Destination";
                        dtSrc.Columns[10].ColumnName = "Event Name";

                        _logger.LogInformation("Step 3: TPN Report Auto Excel Preparation");
                        ExcelWorksheet objWorksheet = objExcelPackage.Workbook.Worksheets.Add(dtSrc.TableName);

                        objWorksheet.Cells["A1"].LoadFromDataTable(dtSrc, true);

                        int rowStart = 1;
                        int colStart = 1;
                        int rowEnd = dtSrc.Rows.Count + 1;
                        int colEnd = 11;

                        using (ExcelRange objRange = objWorksheet.Cells[rowStart, colStart, rowEnd, colEnd])
                        {
                            //objRange.Style.Font.SetFromFont(new Font("Calibri", 10));
                            //objRange.AutoFitColumns();

                            objRange.Style.Border.Top.Style =
                            objRange.Style.Border.Bottom.Style =
                            objRange.Style.Border.Left.Style =
                            objRange.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        }

                        //Format the header    
                        using (ExcelRange objRange = objWorksheet.Cells["A1:K1"])
                        {
                            objRange.Style.Font.Bold = true;
                            objRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            objRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            objRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            objRange.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(234, 234, 234));
                        }

                        _logger.LogInformation("TPN Report Auto user: " + gamUser.GAManagerEmail);
                        _logger.LogInformation("TPN Report Auto client: " + client.ClientName);
                        // _logger.LogInformation("TPN Report Auto tpnCountry: " + client.TPNCountry);
                        // var documentName = "TPNReport_" + client.ClientName + "_" + client.TPNCountry + "_" + DateTime.Now.ToString("MMddyyyyhhmmssfffff") + ".xlsx";

                        var documentName = "TPNReport_" + client.ClientName  + "_" + DateTime.Now.ToString("MMddyyyyhhmmssfffff") + ".xlsx";

                        _logger.LogInformation("TPN Report Auto content Root path: " + _env.ContentRootPath);
                        var filePath = (Path.Combine(_env.ContentRootPath, "wwwroot", "TPNReportAttachment"));
                        _logger.LogInformation("TPN Report Auto file path: " + filePath);

                        if (!Directory.Exists(filePath))
                        {
                            System.IO.Directory.CreateDirectory(filePath);
                        }

                        _logger.LogInformation("TPN Report Auto file name: " + documentName);
                        filePath = filePath + $@"{documentName}";
                        _logger.LogInformation("TPN Report Auto file name with autual path: " + filePath);


                        using (FileStream fStream = File.Create(filePath))
                        {
                            objExcelPackage.SaveAs(fStream);
                            fStream.Close();
                        }

                        // var tpnRequestIds = reportDetails.Where(a => a.ClientUuid == client.ClientUuid && a.TPNCountry == client.TPNCountry && a.GAManagerEmail == gamUser.GAManagerEmail).Select(o => new { RequestId = o.RequestId }).Distinct().ToArray();

                        var tpnRequestIds = reportDetails.Where(a => a.ClientUuid == client.ClientUuid  && a.GAManagerEmail == gamUser.GAManagerEmail).Select(o => new { RequestId = o.RequestId }).Distinct().ToArray();


                        foreach (var item in tpnRequestIds)
                        {
                            TPNReportRequest requestDetailsRow = _context.TPNReportRequest.FirstOrDefault(e => e.Id == item.RequestId);
                            requestDetailsRow.FileName = documentName;
                            requestDetailsRow.ReportStatusId = ReportStatus.ReportCreated;
                        }
                    }

                    //var tpnDetailsIds = reportDetails.Where(a => a.ClientUuid == client.ClientUuid && a.TPNCountry == client.TPNCountry && a.GAManagerEmail == gamUser.GAManagerEmail).Select(o => new { MRFClientResponseDetailsId = o.MRFResponseDetailsId, TPNReportDetailsId = o.TPNReportDetailsId }).Distinct().ToList();

                    var tpnDetailsIds = reportDetails.Where(a => a.ClientUuid == client.ClientUuid && a.GAManagerEmail == gamUser.GAManagerEmail).Select(o => new { MRFClientResponseDetailsId = o.MRFResponseDetailsId, TPNReportDetailsId = o.TPNReportDetailsId }).Distinct().ToList();


                    foreach (var tpnDetails in tpnDetailsIds)
                    {
                        TPNReportDataStatus tpnReportDataStatus = _context.TPNReportDataStatus.FirstOrDefault(e => e.TPNReportDetailsId == tpnDetails.TPNReportDetailsId && e.MRFClientResponseDetailsId == tpnDetails.MRFClientResponseDetailsId);
                        tpnReportDataStatus.IsSendWeekly = true;
                    }
                }
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("Step 4: TPN Report Auto Excel file created.");
            return true;
        }


        //public async Task<bool> SendEmailsToAutoRequest(List<int> requestIds)
        //{
        //    var mrfFromEmailId = this._configuration.GetSection("MRFNotifications")["MRFSubmisionFromEmailId"];
        //    var tpnRptSettings = _configuration.GetSection("AwsS3TPNRptAutoUpload").Get<TPNReportsAwsSettings>();

        //    var requestDetails = await (from tr in _context.TPNReportRequest
        //                                join u in _context.Users on tr.RequestedBy equals u.Id
        //                                where (requestIds.Contains(tr.Id) || tr.ReportStatusId == ReportStatus.ReportCreated)
        //                                select new
        //                                {
        //                                    RequestId = tr.Id,
        //                                    ClientId = tr.ClientId,
        //                                    ClientUuid = tr.ClientUuid,
        //                                    TPNCountry = tr.TPNCountry,
        //                                    RequestById = tr.RequestedBy,
        //                                    RequestByName = u.FirstName + " " + u.LastName,
        //                                    RequestByEmail = u.Email,
        //                                    DownloadPath = tr.DownloadPath,
        //                                    GAMEmail = tr.GAMEmail,
        //                                    FileName = tr.FileName
        //                                }).ToListAsync();


        //    var userEmails = requestDetails.Select(o => new { RequestByEmail = o.RequestByEmail, RequestById = o.RequestById, RequestByName = o.RequestByName }).Distinct().ToArray();

        //    string emailTemplate = @"<figure class=""table""><table style = ""width:100%;"" cellpadding=""0"" cellspacing=""0"" border=""0""><tbody><tr><td style = ""width:100%;"" cellpadding=""0"" cellspacing=""0"" border=""0""><figure class=""table""><table style = ""width:600px;"" cellpadding=""0"" cellspacing=""0"" border=""0"" align=""center""><tbody><tr><td style = ""background-color:#f3f6f6;font-family:Helvetica, Arial, sans-serif;font-size:12px;padding-bottom:20px;padding-left:10px;padding-top:20px;"" cellpadding=""0"" cellspacing=""0"" border=""0""><p><font face = ""Helvetica, Arial, sans-serif"" > Dear[UserName],</font></p><p><font face = ""Helvetica, Arial, sans-serif"" > Please find attachment of TPN report.</font></p><p><font face = ""Helvetica, Arial, sans-serif"" > Best regards,</font></p><p><font face = ""Helvetica, Arial, sans-serif"" > Meetings and Events team</font></p></td></tr></tbody></table></figure></td></tr></tbody></table></figure>";

        //    foreach (var email in userEmails)
        //    {
        //        StringBuilder emailBody = new StringBuilder(emailTemplate);

        //        emailBody.Replace("[UserName]", email.RequestByName.ToString());
        //        emailBody.Replace("&nbsp;", "");

        //        EmailObject emailObj = new EmailObject();

        //        emailObj.To = email.RequestByEmail;
        //        emailObj.FromEmail = mrfFromEmailId.ToString();
        //        emailObj.Body = emailBody.ToString();
        //        emailObj.Subject = "Weekly TPN Report";
        //        emailObj.FromEmailDisplayName = "MRF";

        //        var emailDetails = (from r in requestDetails
        //                            where r.RequestByEmail == email.RequestByEmail && r.RequestById == email.RequestById
        //                            orderby r.RequestId ascending
        //                            select r).ToList();

        //        int i = 0;
        //        emailObj.AttachmentFilePaths = new string[emailDetails.Count];

        //        string GAMEmails = string.Empty;

        //        foreach (var detail in emailDetails)
        //        {
        //            if (!GAMEmails.Contains(detail.GAMEmail))
        //            {
        //                GAMEmails = GAMEmails + ";" + detail.GAMEmail;
        //            }

        //            emailObj.AttachmentFilePaths[i] = (Path.Combine(_env.ContentRootPath, "wwwroot", "TPNReportAttachment")) + $@"{detail.FileName}";

        //            i++;
        //        }

        //        if (GAMEmails != string.Empty)
        //        {
        //            emailObj.To = emailObj.To + ";" + GAMEmails;
        //        }

        //        var result = await _emailSendingService.SendWithAttachments(emailObj);
        //        _logger.LogError("TPN Report After Attachment sent 1");
        //        foreach (var item in emailDetails)
        //        {
        //            TPNReportRequest requestDetailsRow = _context.TPNReportRequest.FirstOrDefault(e => e.Id == item.RequestId);
        //            requestDetailsRow.ReportStatusId = result.Failed == false ? ReportStatus.Completed : ReportStatus.Failed;
        //            if (requestDetailsRow.ReportStatusId == ReportStatus.Completed)
        //            {
        //                _logger.LogError("TPN Report After Attachment sent 2");
        //                var projectUuid = await (from p in _context.Projects
        //                                         where p.Prefix == item.ClientUuid
        //                                         select p).ToListAsync();

        //                string projectUniqueId = string.Empty;

        //                foreach (var uuid in projectUuid)
        //                {
        //                    projectUniqueId = Convert.ToString(uuid.UniqueId);
        //                }
        //                _logger.LogError("TPN Report After Attachment sent 3");
        //                using (MemoryStream myfileInMemory = new MemoryStream(File.ReadAllBytes((Path.Combine(_env.ContentRootPath, "wwwroot", "TPNReportAttachment")) + $@"{item.FileName}")))
        //                {
        //                    var path = StringExtensions.CombineWithSlash("Project", projectUniqueId, tpnRptSettings.ParentFolder, tpnRptSettings.SubFolder, item.FileName);

        //                    var resultFileUpload = await _fileStorage.UploadFile(path, myfileInMemory);
        //                    requestDetailsRow.DownloadPath = resultFileUpload.AbsoluteUrl;
        //                }
        //                _logger.LogError("TPN Report After Attachment sent 4");
        //            }
        //        }

        //        foreach (var item in emailDetails)
        //        {
        //            TPNReportRequest requestDetailsRow = _context.TPNReportRequest.FirstOrDefault(e => e.Id == item.RequestId);

        //            if (requestDetailsRow.ReportStatusId == ReportStatus.Completed)
        //            {
        //                if (File.Exists((Path.Combine(_env.ContentRootPath, "wwwroot", "TPNReportAttachment")) + $@"{item.FileName}"))
        //                {
        //                    File.Delete(Path.Combine(_env.ContentRootPath, "wwwroot", "TPNReportAttachment") + $@"{item.FileName}");
        //                }
        //            }
        //        }

        //    }


        //    await _context.SaveChangesAsync();

        //    return true;
        //}


        public async Task<bool> SendEmailsToAutoRequest(List<int> requestIds)
        {
            var mrfFromEmailId = this._configuration.GetSection("MRFNotifications")["MRFSubmisionFromEmailId"];
            var tpnRptSettings = _configuration.GetSection("AwsS3TPNRptAutoUpload").Get<TPNReportsAwsSettings>();

            var requestDetails = await (from tr in _context.TPNReportRequest
                                        join u in _context.Users on tr.RequestedBy equals u.Id
                                        where requestIds.Contains(tr.Id)
                                        select new
                                        {
                                            RequestId = tr.Id,
                                            ClientId = tr.ClientId,
                                            ClientUuid = tr.ClientUuid,
                                            TPNCountry = tr.TPNCountry,
                                            RequestById = tr.RequestedBy,
                                            RequestByName = u.FirstName + " " + u.LastName,
                                            RequestByEmail = u.Email,
                                            DownloadPath = tr.DownloadPath,
                                            GAMEmail = tr.GAMEmail,
                                            FileName = tr.FileName
                                        }).Distinct().ToListAsync();


            var gamUserEmails = requestDetails.Select(o => new { GAMEmail = o.GAMEmail }).Distinct().ToArray();

            string emailTemplate = @"<figure class=""table""><table style = ""width:100%;"" cellpadding=""0"" cellspacing=""0"" border=""0""><tbody><tr><td style = ""width:100%;"" cellpadding=""0"" cellspacing=""0"" border=""0""><figure class=""table""><table style = ""width:600px;"" cellpadding=""0"" cellspacing=""0"" border=""0"" align=""center""><tbody><tr><td style = ""background-color:#f3f6f6;font-family:Helvetica, Arial, sans-serif;font-size:12px;padding-bottom:20px;padding-left:10px;padding-top:20px;"" cellpadding=""0"" cellspacing=""0"" border=""0""><p><font face = ""Helvetica, Arial, sans-serif"" > [UserName]</font></p><p><font face = ""Helvetica, Arial, sans-serif"" > Please see attached the latest MRF report for TPN subsidiaries. </font></p><p><font face = ""Helvetica, Arial, sans-serif"" > Best regards,</font></p><p><font face = ""Helvetica, Arial, sans-serif"" > Meetings and Events team</font></p></td></tr></tbody></table></figure></td></tr></tbody></table></figure>";

            foreach (var email in gamUserEmails)
            {
                StringBuilder emailBody = new StringBuilder(emailTemplate);

                emailBody.Replace("[UserName]", " Hi,");
                emailBody.Replace("&nbsp;", "");

                EmailObject emailObj = new EmailObject();

                emailObj.To = email.GAMEmail;
                emailObj.FromEmail = mrfFromEmailId.ToString();
                emailObj.Body = emailBody.ToString();
                emailObj.Subject = "Weekly TPN Report";
                emailObj.FromEmailDisplayName = "MRF";

                var emailDetails = (from r in requestDetails
                                    where r.GAMEmail == email.GAMEmail
                                    orderby r.RequestId ascending
                                    select r).ToList();

                int i = 0;
                emailObj.AttachmentFilePaths = new string[emailDetails.Count];

                string adminEmails = string.Empty;

                foreach (var detail in emailDetails)
                {
                    if (!adminEmails.Contains(detail.RequestByEmail))
                    {
                        if (adminEmails == string.Empty)
                        {
                            adminEmails = detail.RequestByEmail;
                        }
                        else
                        {
                            adminEmails = adminEmails + ";" + detail.RequestByEmail;
                        }
                    }

                    if (!emailObj.AttachmentFilePaths.Contains((Path.Combine(_env.ContentRootPath, "wwwroot", "TPNReportAttachment")) + $@"{detail.FileName}"))
                    {
                        emailObj.AttachmentFilePaths[i] = (Path.Combine(_env.ContentRootPath, "wwwroot", "TPNReportAttachment")) + $@"{detail.FileName}";
                        i++;
                    }
                }

                if (adminEmails != string.Empty)
                {
                    emailObj.Cc = adminEmails;
                }

                var result = await _emailSendingService.SendWithAttachments(emailObj);

                _logger.LogInformation("Step 5: TPN Report Auto sent mail with attachment.");
                foreach (var item in emailDetails)
                {
                    TPNReportRequest requestDetailsRow = _context.TPNReportRequest.FirstOrDefault(e => e.Id == item.RequestId);
                    requestDetailsRow.ReportStatusId = result.Failed == false ? ReportStatus.Completed : ReportStatus.Failed;

                    if (requestDetailsRow.ReportStatusId == ReportStatus.Completed)
                    {

                        var projectUuid = await (from p in _context.Projects
                                                 where p.Prefix == item.ClientUuid
                                                 select p).ToListAsync();

                        string projectUniqueId = string.Empty;

                        foreach (var uuid in projectUuid)
                        {
                            projectUniqueId = Convert.ToString(uuid.UniqueId);
                        }

                        using (MemoryStream myfileInMemory = new MemoryStream(File.ReadAllBytes((Path.Combine(_env.ContentRootPath, "wwwroot", "TPNReportAttachment")) + $@"{item.FileName}")))
                        {
                            var path = StringExtensions.CombineWithSlash("Project", projectUniqueId, tpnRptSettings.ParentFolder, tpnRptSettings.SubFolder, item.FileName);

                            var resultFileUpload = await _fileStorage.UploadFile(path, myfileInMemory);
                            requestDetailsRow.DownloadPath = resultFileUpload.AbsoluteUrl;
                        }
                        _logger.LogInformation("Step 6: TPN Report Auto upload file into S3 bucket.");
                    }
                }

                foreach (var item in emailDetails)
                {
                    TPNReportRequest requestDetailsRow = _context.TPNReportRequest.FirstOrDefault(e => e.Id == item.RequestId);

                    if (requestDetailsRow.ReportStatusId == ReportStatus.Completed)
                    {
                        if (File.Exists((Path.Combine(_env.ContentRootPath, "wwwroot", "TPNReportAttachment")) + $@"{item.FileName}"))
                        {
                            File.Delete(Path.Combine(_env.ContentRootPath, "wwwroot", "TPNReportAttachment") + $@"{item.FileName}");
                        }
                    }
                }

                _logger.LogInformation("Step 7: TPN Report Auto delete file from temp location.");
            }

            await _context.SaveChangesAsync();

            return true;
        }


        public DataTable LINQResultToDataTable<T>(IEnumerable<T> Linqlist)
        {
            DataTable dt = new DataTable();
            PropertyInfo[] columns = null;

            if (Linqlist == null) return dt;

            foreach (T Record in Linqlist)
            {

                if (columns == null)
                {
                    columns = ((Type)Record.GetType()).GetProperties();
                    foreach (PropertyInfo GetProperty in columns)
                    {
                        Type colType = GetProperty.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                        == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dt.Columns.Add(new DataColumn(GetProperty.Name, colType));
                    }
                }

                DataRow dr = dt.NewRow();

                foreach (PropertyInfo pinfo in columns)
                {
                    dr[pinfo.Name] = pinfo.GetValue(Record, null) == null ? DBNull.Value : pinfo.GetValue
                    (Record, null);
                }

                dt.Rows.Add(dr);
            }
            return dt;
        }



    }

}



