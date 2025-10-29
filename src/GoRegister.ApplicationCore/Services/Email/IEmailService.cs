using Fluid;
using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Liquid;
using GoRegister.ApplicationCore.Domain.Registration;
using GoRegister.ApplicationCore.Domain.Registration.Framework;
using GoRegister.ApplicationCore.Domain.Registration.Models;
using GoRegister.ApplicationCore.Domain.Settings.Services;
using GoRegister.ApplicationCore.Framework.Domain;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Services.Email
{
    public interface IEmailService
    {
        Task<EmailAudit> GenerateAndSendEmail(Data.Models.Email email, DelegateLiquidQueryModel user);
        Task<Guid> GenerateEmails(Data.Models.Email email, List<DelegateLiquidQueryModel> users);
        Task<Result<Guid>> SendEmails(Guid batchId);
    }

    public class EmailService : IEmailService
    {
        //private readonly IConfiguration _smtpConfig;

        private readonly IConfiguration _configuration;
        private readonly ILiquidTemplateManager _liquidTemplateManager;
        private readonly ApplicationDbContext _context;
        private readonly IProjectSettingsAccessor _projectSettingsAccessor;
        private readonly IEmailSchedulingService _emailSchedulingService;
        private readonly IDelegateUserCacheService _delegateUserCacheService;
        private readonly IEmailSendingService _emailSendingService;

        public EmailService(IConfiguration configuration, ILiquidTemplateManager liquidTemplateManager, ApplicationDbContext context, IProjectSettingsAccessor projectSettingsAccessor, IEmailSchedulingService emailSchedulingService, IDelegateUserCacheService delegateUserCacheService, IEmailSendingService emailSendingService)
        {
            _configuration = configuration;
            _liquidTemplateManager = liquidTemplateManager;
            _context = context;
            _projectSettingsAccessor = projectSettingsAccessor;
            _emailSchedulingService = emailSchedulingService;
            _delegateUserCacheService = delegateUserCacheService;
            _emailSendingService = emailSendingService;
            //_smtpConfig = configuration.GetSection("SMTPConfig");
        }

        public async Task<Guid> GenerateEmails(Data.Models.Email email, List<DelegateLiquidQueryModel> users)
        {
            var settings = await _projectSettingsAccessor.GetAsync();
            var batch = new EmailAuditBatch();
            batch.BatchId = Guid.NewGuid();
            batch.DateCreatedUtc = SystemTime.UtcNow;
            batch.CreatedByUserId = 1; //TODO: send current user
            batch.Status = EmailAuditBatchStatus.Preview;
            _context.EmailAuditBatches.Add(batch);

            foreach (var user in users)
            {
                var cachedUser = _delegateUserCacheService.Get(user);
                var emailAudit = await BuildAudit(email, cachedUser, settings, batch);
                _context.EmailAudits.Add(emailAudit);
            }
            await _context.SaveChangesAsync();

            return batch.BatchId;
        }

        public async Task<EmailAudit> GenerateAndSendEmail(Data.Models.Email email, DelegateLiquidQueryModel user)
        {
            var settings = await _projectSettingsAccessor.GetAsync();

            var cachedUser = _delegateUserCacheService.Get(user);
            var emailAudit = await BuildAudit(email, cachedUser, settings, null);

            var emailObject = new EmailObject
            {
                Subject = emailAudit.Subject,
                Bcc = emailAudit.Bcc,
                Cc = emailAudit.Cc,
                Body = emailAudit.Body,
                FromEmail = emailAudit.FromEmail,
                FromEmailDisplayName = emailAudit.FromEmailDisplayName,
                To = emailAudit.To
            };

            var result = await _emailSendingService.Send(emailObject);

            if (result)
            {
                emailAudit.StatusId = EmailAuditStatus.Success;
            }
            else
            {
                emailAudit.StatusId = EmailAuditStatus.Failed;
            }

            _context.EmailAudits.Add(emailAudit);
            await _context.SaveChangesAsync();

            return emailAudit;
        }

        public async Task<Result<Guid>> SendEmails(Guid batchId)
        {
            return await
                _context.EmailAuditBatches.FindResultAsync(e => e.BatchId == batchId)
                .MapAsync(async batch =>
                {
                    batch.DateSentUtc = SystemTime.UtcNow;
                    batch.Status = EmailAuditBatchStatus.ToSend;
                    await _context.SaveChangesAsync();
                    await _emailSchedulingService.Schedule(batch);
                    return Result.Ok(batchId);
                });
        }

        private async Task<EmailAudit> BuildAudit(Data.Models.Email email, DelegateDataTagAccessor user, Project settings, EmailAuditBatch batch = null)
        {
            // get template for current reg type, use default template if no specific
            var template = email.EmailTemplates.FirstOrDefault(e => e.RegistrationTypes.Select(e => e.RegistrationTypeId).Contains(user.RegistrationTypeId))
                ?? email.EmailTemplates.First(e => e.IsDefault);

            var emailBody = email.EmailLayout != null ? 
                email.EmailLayout.Html.Replace("{{email_content}}", template.BodyHtml) : 
                template.BodyHtml;

            var context = new TemplateContext();
            context.SetValue("user", user);
            var emailAudit = new EmailAudit
            {
                To = user.Email,
                FromEmail = settings.EmailAddress,
                FromEmailDisplayName = settings.EmailDisplayFrom,
                Subject = await _liquidTemplateManager.RenderAsync(email.Subject, context),
                Body = await _liquidTemplateManager.RenderAsync(emailBody, context),
                DelegateUserId = user.Id,
                EmailTemplateId = template.Id,
                EmailAuditBatch = batch,
                Bcc = await _liquidTemplateManager.RenderAsync(email.Bcc, context),
                Cc = await _liquidTemplateManager.RenderAsync(email.Cc, context),
                DateCreatedUtc = SystemTime.UtcNow
            };

            return emailAudit;
        }
    }
}
