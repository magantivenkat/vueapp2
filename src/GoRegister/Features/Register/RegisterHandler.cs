/*  MRF Changes : Create MRF Client Form details in Json format 
    Modified Date : 06th October 2022
    Modified By : Mandar.Khade@amexgbt.com
    Team member : Harish.Rane@amexgbt.com
    JIRA Ticket No : GoRegister/GOR-238   

   MRF Changes : Post user form data to meeting central
    Modified Date : 31st October 2022
    Modified By : Mandar.Khade@amexgbt.com
    Team member : Harish.Rane@amexgbt.com
    JIRA Ticket No : GoRegister/GOR-228-3
 */

using Fluid;
using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Liquid;
using GoRegister.ApplicationCore.Domain.Registration;
using GoRegister.ApplicationCore.Domain.Registration.Events;
using GoRegister.ApplicationCore.Domain.Registration.Framework;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Models;
using GoRegister.ApplicationCore.Domain.Registration.Services;
using GoRegister.ApplicationCore.Framework.Domain;
using GoRegister.ApplicationCore.Framework.Identity;
using GoRegister.ApplicationCore.Services.Email;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GoRegister.Features.Register
{
    public static class RegisterHandler
    {
        public class Query : IRequest<Response>
        {
            public FormModel Model { get; set; }
        }

        public class Response
        {
            public Response(FormValidationContext validationContext)
            {
                ValidationContext = validationContext;
            }

            public FormValidationContext ValidationContext { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, Response>
        {
            private readonly ApplicationDbContext _db;
            private readonly ICurrentUserAccessor _currentUserAccessor;
            private readonly IDelegateUserCacheService _delegateUserCacheService;
            private readonly ILiquidTemplateManager _liquidTemplateManager;
            private readonly IRegistrationService _registrationService;
            private readonly IFormService _formService;
            private readonly IPublisher _publisher;
            private readonly IConfiguration _configuration;
            private readonly IEmailSendingService _emailSendingService;


            public QueryHandler(ApplicationDbContext db, ICurrentUserAccessor currentUserAccessor, IDelegateUserCacheService delegateUserCacheService, ILiquidTemplateManager liquidTemplateManager, IRegistrationService registrationService, IFormService formService, IPublisher publisher, IConfiguration configuration, IEmailSendingService emailSendingService)
            {
                _db = db;
                _currentUserAccessor = currentUserAccessor;
                _delegateUserCacheService = delegateUserCacheService;
                _liquidTemplateManager = liquidTemplateManager;
                _registrationService = registrationService;
                _formService = formService;
                _publisher = publisher;
                _configuration = configuration;
                _emailSendingService = emailSendingService;

            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = request.Model;
                    var formModel = await _formService.GetRegistrationForm();
                    string keyId = string.Empty;
                    string emailKeyId = string.Empty;
                    string fNameKeyId = string.Empty;
                    string lNameKeyId = string.Empty;
                    string servicingCountryKeyId = string.Empty;
                    string servicingCountry = string.Empty;
                    string requestorCountryId = string.Empty;
                    string servicingCountryId = string.Empty;
                    string requestorCountryKeyId = string.Empty;
                    string requestorCountry = string.Empty;
                    bool allowedTPNCountry = false;
                    var accessToken = this._configuration.GetSection("APIDetails")["AccessToken"];
                    var mrfSubmissionAPI = this._configuration.GetSection("APIDetails")["MRFSubmissionAPI"];
                    var mrfFromEmailId = this._configuration.GetSection("MRFNotifications")["MRFSubmisionFromEmailId"];
                    bool isSubmitToMC = true;
                    bool isSendMail = false;
                    bool isTPNCountry = false;
                    StringBuilder emailIds = new StringBuilder();
                    StringBuilder tpnEmailIds = new StringBuilder();
                    string clientName = string.Empty;
                    int clientId = 0;
                    Dictionary<string, string> tpnReportKeyVal = new Dictionary<string, string>();
                    int mrfClientResponseDetailsId = 0;

                    FormValidationContext formValidation = new FormValidationContext();

                    foreach (var item in formModel.Form.RegistrationPages)
                    {
                        keyId = item.Fields.FirstOrDefault(x => x.FieldTypeId == FieldTypeEnum.MRFDestination).Id.ToString();
                        emailKeyId = item.Fields.FirstOrDefault(x => x.DataTag == "Email").Id.ToString();
                        fNameKeyId = item.Fields.FirstOrDefault(x => x.DataTag == "FirstName").Id.ToString();
                        lNameKeyId = item.Fields.FirstOrDefault(x => x.DataTag == "LastName").Id.ToString();
                        allowedTPNCountry = Convert.ToBoolean(item.Fields.FirstOrDefault(x => x.DataTag == "ServicingCountry").AllowTPNCountries);
                        servicingCountryKeyId = item.Fields.FirstOrDefault(x => x.DataTag == "ServicingCountry").Id.ToString();
                        var rq = item.Fields.Where(x => x.DataTag == "RequestorCountry");
                        if (rq.Count() >0)
                        {
                            requestorCountryKeyId = item.Fields.FirstOrDefault(x => x.DataTag == "RequestorCountry").Id.ToString();
                        }
                    }

                    var userModel = model.User.Model[keyId];
                    var email = model.User.Model[emailKeyId];
                    var fName = model.User.Model[fNameKeyId];
                    var lName = model.User.Model[lNameKeyId];
                    if (requestorCountryKeyId != "")
                    {
                        requestorCountryId = Convert.ToString(model.User.Model[requestorCountryKeyId]);
                    }
                    else
                    {
                        servicingCountry = Convert.ToString(model.User.Model[servicingCountryKeyId]);
                    }

                    emailIds.Append(email + ";");

                    Newtonsoft.Json.Linq.JToken value = userModel.First.First + "|^|" + userModel.Last.First;

                    model.User.Model[keyId] = value;

                    UserFormResponseModel response = null;
                    TPNReportDetails tpnReportDetails = null;

                    if (_currentUserAccessor.Get.Identity.IsAuthenticated)
                    {
                        var userId = _currentUserAccessor.GetUserId().Value;
                        var userResponseResult = await _formService.GetUserResponseModel(formModel.Form.Id, userId);
                        response = userResponseResult.Value;
                    }
                    else
                    {
                        //TODO:handle anonymous reg
                    }

                    var delegateUser = response.Response.DelegateUser;

                    // verify reg is open
                    if (!delegateUser.IsTest && !_registrationService.CanRegister(delegateUser.RegistrationType))
                    {
                        return null;
                    }

                    response.Response.SetupAudit();

                    formValidation = await _formService.ProcessFormResponse(formModel, response, model);

                    if (!formValidation.IsValid)
                    {
                        return new Response(formValidation);
                    }

                    var serializedForm = _formService.SerializeForm(response.Response, formModel);
                    delegateUser.RegistrationDocument = serializedForm;

                    var project = await _db.Projects.SingleOrDefaultAsync(p => p.Id == response.Response.ProjectId);

                    MRFClientRequest mrfClientRequest = await _db.MRFClientRequest.FirstOrDefaultAsync(e => e.ClientUuid == project.Prefix);

                    Guid guid = Guid.NewGuid();

                    delegateUser.ChangeRegistrationStatus(ApplicationCore.Data.Enums.RegistrationStatus.Invited);

                    delegateUser.HasBeenModified();

                    MRFClientResponseDetails clientDetails = new MRFClientResponseDetails();

                    clientDetails.FormId = response.Response.FormId;

                    clientDetails.UserGUID = guid.ToString();
                    clientDetails.ProjectId = response.Response.ProjectId;
                    if (project != null)
                    {
                        clientDetails.ClientGUID = project.Prefix;
                    }
                    clientDetails.ClientUserResponse = delegateUser.MRFClientResponse;
                    if (requestorCountryKeyId != "")
                    {
                        var countryMapping = await _db.MRFServiceCountryMapping.FirstOrDefaultAsync(e => e.ClientUuid == clientDetails.ClientGUID && e.ProjectId == project.Id && e.RequestCountryId == requestorCountryId && e.IsActive == true);
                        servicingCountry = countryMapping.ServiceCountry;
                        requestorCountry = countryMapping.RequestCountry;

                        var rsp = response.Response.UserFieldResponses;

                        UserFieldResponse sc = rsp.FirstOrDefault(e => e.FieldId == Convert.ToInt32(servicingCountryKeyId));
                        if (sc != null)
                        {
                            sc.StringValue = countryMapping.ServiceCountryUuid;
                        }
                        else
                        {
                            UserFieldResponse servcountry = new UserFieldResponse();
                            servcountry.FieldId = Convert.ToInt32(servicingCountryKeyId);
                            servcountry.StringValue = countryMapping.ServiceCountryUuid;
                            rsp.Add(servcountry);
                        }
                    }

                    clientDetails.ClientUserResponse = _formService.SerializeFormMRF(response.Response, formModel, guid.ToString());
                    //clientDetails.ClientUserResponseWithID = delegateUser.RegistrationDocument;
                    clientDetails.DateTimeCreated = DateTime.Now;




                    //add audit record
                    var audit = response.Response.GetAudit(ActionedFrom.Form, delegateUser.ApplicationUser);
                    _db.DelegateAudits.Add(audit);

                    await _publisher.Publish(new DelegateRegisteredEvent(delegateUser));

                    if (allowedTPNCountry)
                    {
                        isSendMail = true;

                        if (servicingCountry != null)
                        {
                            var TPNClientList = await _db.TPNCountryClientEmails.Where(e => e.ClientUuid == project.Prefix && e.TPNCountry == servicingCountry && e.IsDeleted == false).ToListAsync();

                            if (TPNClientList.Count > 0)
                            {
                                clientDetails.AllowTPNCountries = true;

                                isTPNCountry = true;

                                foreach (var tpnEmail in TPNClientList)
                                {
                                    emailIds.Append(tpnEmail.TPNEmail + ";");
                                    tpnEmailIds.Append(tpnEmail.TPNEmail + ";");
                                }

                                isSubmitToMC = false;

                                var client = await _db.Clients.SingleOrDefaultAsync(c => c.ClientUuid == clientDetails.ClientGUID);
                                clientName = client.Name;
                                clientId = client.Id;
                            }
                            else
                            {
                                allowedTPNCountry = false;
                                isSubmitToMC = true;
                            }
                        }
                    }

                    List<MRFFormDetails> mRFFormDetails = _formService.SerializeFormMRFEmail(response.Response, formModel, guid.ToString(), isTPNCountry);

                    StringBuilder mrf = new StringBuilder();
                    StringBuilder mrfValue = new StringBuilder();

                    if (mRFFormDetails.Count > 0)
                    {
                        foreach (var item in mRFFormDetails)
                        {
                            mrf.Append("<tr><td cellspacing=\"0\" cellpadding=\"0\" style=\"height:30px;padding-left:5px;border:1px solid #776f7f;font-size:12px;font-family: Helvetica, Arial, sans-serif;\">" + item.FieldName + "</td><td cellspacing=\"0\" cellpadding=\"0\" style=\"height:30px;padding-left:5px;border:1px solid #776f7f;font-size:12px;font-family: Helvetica, Arial, sans-serif;\">" + item.FieldValue + "</td></tr>");

                            if (allowedTPNCountry)
                            {
                                if (servicingCountry != null)
                                {
                                    tpnReportKeyVal.Add(item.FieldDataTag, item.FieldValue);
                                }
                            }
                        }
                    }

                    if (mrfClientRequest.MRFClientStatus == "Published")
                    {
                        if (isSubmitToMC)
                        {
                            var data = new StringContent(clientDetails.ClientUserResponse);

                            HttpClient client = new HttpClient();
                            client.BaseAddress = new Uri(mrfSubmissionAPI + "/" + project.Prefix);
                            client.DefaultRequestHeaders.Add("Authorization", accessToken);
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                            var result = await client.PostAsync(client.BaseAddress, data);
                            string resultContent = await result.Content.ReadAsStringAsync();

                            clientDetails.APIErrorCode = (int)result.StatusCode;

                            if (result.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                isSendMail = true;
                            }
                            else
                            {
                                isSendMail = false;

                                formValidation = new FormValidationContext();
                                formValidation.SetFieldContext(Convert.ToString((int)result.StatusCode));
                                formValidation.AddError(Convert.ToString((int)System.Net.HttpStatusCode.Locked), "Our database is currently facing technical difficulties.  Please contact your Meeting Planning team to proceed with your request while the issue is being resolved.");
                            }
                        }

                        _db.MRFClientResponseDetails.Add(clientDetails);

                        await _db.SaveChangesAsync();

                        mrfClientResponseDetailsId = clientDetails.Id;

                        if (allowedTPNCountry)
                        {
                            if (servicingCountry != null)
                            {
                                tpnReportDetails = new TPNReportDetails();

                                tpnReportDetails.MRFResponseDetailsId = mrfClientResponseDetailsId;
                                tpnReportDetails.MRFFormName = project.Name;
                                tpnReportDetails.ClientId = clientId;
                                tpnReportDetails.ClientUuid = clientDetails.ClientGUID;
                                tpnReportDetails.GBTClient = mrfClientRequest.ClientID;
                                tpnReportDetails.ClientName = clientName;
                                // tpnReportDetails.TPNCountry = tpnReportKeyVal.Single(x => x.Key.ToLower() == "servicingcountry").Value;
                                tpnReportDetails.TPNCountry = servicingCountry;
                                tpnReportDetails.TPNSharedMailbox = tpnEmailIds.ToString().TrimEnd(';');
                                tpnReportDetails.ContactFirstName = tpnReportKeyVal.Single(x => x.Key.ToLower() == "firstname").Value;
                                tpnReportDetails.ContactLastName = tpnReportKeyVal.Single(x => x.Key.ToLower() == "lastname").Value;
                                tpnReportDetails.ContactEmail = tpnReportKeyVal.Single(x => x.Key.ToLower() == "email").Value;
                                tpnReportDetails.DepartureDate = Convert.ToDateTime(tpnReportKeyVal.Single(x => x.Key.ToLower() == "eventstartdate").Value);
                                tpnReportDetails.Destination = tpnReportKeyVal.Single(x => x.Key.ToLower() == "destinationexternalid").Value;
                                tpnReportDetails.EventName = tpnReportKeyVal.Single(x => x.Key.ToLower() == "eventname").Value;
                                tpnReportDetails.FormDateTimeCreated = DateTime.Now;

                                _db.TPNReportDetails.Add(tpnReportDetails);

                                await _db.SaveChangesAsync();

                                TPNReportDataStatus tpnReportDataStatus = new TPNReportDataStatus();

                                tpnReportDataStatus.TPNReportDetailsId = tpnReportDetails.Id;
                                tpnReportDataStatus.MRFClientResponseDetailsId = mrfClientResponseDetailsId;
                                tpnReportDataStatus.IsSendWeekly = false;
                                tpnReportDataStatus.IsSendFortNightly = false;
                                tpnReportDataStatus.IsSendMonthly = false;

                                _db.TPNReportDataStatus.Add(tpnReportDataStatus);

                            }
                        }
                    }
                    else
                    {
                        isSendMail = false;

                        formValidation = new FormValidationContext();
                        formValidation.SetFieldContext(Convert.ToString((int)System.Net.HttpStatusCode.Locked));
                        formValidation.AddError(Convert.ToString((int)System.Net.HttpStatusCode.Locked), "There is a technical issue preventing your meeting request from being submitted. Please contact your Meeting Planning team for further assistance.");
                    }

                    if (isSendMail)
                    {
                        var emailTemplate = await _db.EmailTemplates.SingleOrDefaultAsync(x => x.ProjectId == project.Id);
                        var emailnotification = await _db.Emails.SingleOrDefaultAsync(x => x.ProjectId == project.Id);
                        StringBuilder emailBody = new StringBuilder(emailTemplate.BodyHtml);

                        emailBody.Replace("[UserName]", fName.ToString() + " " + lName.ToString());
                        emailBody.Replace("[UserEmailId]", email.ToString());
                        emailBody.Replace("[FromEmailId]", mrfFromEmailId.ToString());
                        emailBody.Replace("[MRFFormName]", project.Name);
                        emailBody.Replace("&lt;", "<");
                        emailBody.Replace("&gt;", ">");
                        emailBody.Replace("<br>", "");
                        emailBody.Replace("&nbsp;", "");

                        emailBody.Replace("[MRFFormInformation]", Convert.ToString(mrf));

                        var emailObject = new EmailObject
                        {
                            Subject = "Meeting Request Form Submission: " + project.Name,
                            //Bcc = "",
                            //Cc = "",
                            Bcc = emailnotification.Bcc,
                            Cc = emailnotification.Cc,
                            Body = emailBody.ToString(),
                            FromEmail = mrfFromEmailId.ToString(),
                            FromEmailDisplayName = "MRF",
                            To = emailIds.ToString().TrimEnd(';')
                        };

                        await _emailSendingService.Send(emailObject);

                        if (allowedTPNCountry)
                        {
                            if (servicingCountry != null)
                            {
                                var clientDetailsRow = _db.MRFClientResponseDetails.FirstOrDefault(e => e.Id == mrfClientResponseDetailsId);
                                clientDetailsRow.SendTPNEmailDateTime = DateTime.Now;
                                clientDetailsRow.CopyToReport = true;
                                await _db.SaveChangesAsync();
                            }
                        }
                    }
                    return new Response(formValidation);
                }
                catch(Exception ex)
                {
                    Serilog.Log.Error(ex, "Error Message in RegisterHandler: {Message}", ex.Message);                
                    throw;
                }

                
            }
        }
    }
}