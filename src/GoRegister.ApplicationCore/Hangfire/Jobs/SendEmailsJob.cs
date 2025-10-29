using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Framework;
using GoRegister.ApplicationCore.Framework.Multitenancy;
using GoRegister.ApplicationCore.Services.Email;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Jobs
{
    public interface ISendEmailsJob
    {
        Task Execute(int batchId, ProjectTenant tenant);
    }

    public class SendEmailsJob : ISendEmailsJob
    {
        private readonly ApplicationDbContext _db;
        private readonly IEmailSendingService _emailSendingService;
        private readonly ITenantAccessor _tenantAccessor;

        public SendEmailsJob(ApplicationDbContext context, IEmailSendingService emailSendingService, ITenantAccessor tenantAccessor)
        {
            _db = context;
            _emailSendingService = emailSendingService;
            _tenantAccessor = tenantAccessor;
        }

        public async Task Execute(int batchId, ProjectTenant tenant)
        {
            _tenantAccessor.SetTenant(tenant);

            var emailsToSend = await _db.EmailAudits
                .Include(ea => ea.EmailAuditBatch)
                .Include(ea => ea.EmailTemplate)
                    .ThenInclude(et => et.Email)
                .Include(ea => ea.DelegateUser)
                .Where(e => e.EmailAuditBatchId == batchId && e.StatusId == EmailAuditStatus.Review)
                .ToListAsync();

            foreach (var email in emailsToSend)
            {
                var emailObject = new EmailObject
                {
                    Subject = email.Subject,
                    Bcc = email.Bcc,
                    Cc = email.Cc,
                    Body = email.Body,
                    FromEmail = email.FromEmail,
                    FromEmailDisplayName = email.FromEmailDisplayName,
                    To = email.To
                };

                var result = await _emailSendingService.Send(emailObject);

                if (result)
                {
                    email.StatusId = EmailAuditStatus.Success;

                    if (email.EmailTemplate.Email.EmailType == EmailType.Invitation &&
                        email.DelegateUser.RegistrationStatusId == (int)RegistrationStatus.NotInvited)
                    {
                        email.DelegateUser.ChangeRegistrationStatus(RegistrationStatus.Invited);
                        email.DelegateUser.HasBeenModified();

                        var audit = email.DelegateUser.GetAudit(ActionedFrom.SendEmail, email.EmailAuditBatch.CreatedByUserId);
                        _db.DelegateAudits.Add(audit);
                    }
                }
                else
                {
                    email.StatusId = EmailAuditStatus.Failed;
                }

                await _db.SaveChangesAsync();
            }

        }
    }
}
