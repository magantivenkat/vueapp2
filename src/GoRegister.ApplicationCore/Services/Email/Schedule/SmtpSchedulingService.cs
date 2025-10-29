using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Services.Email.Schedule
{
    public class SmtpSchedulingService : IEmailSchedulingService
    {
        private readonly ApplicationDbContext _db;
        private readonly IEmailSendingService _emailSendingService;

        public SmtpSchedulingService(ApplicationDbContext context, IEmailSendingService emailSendingService)
        {
            _db = context;
            _emailSendingService = emailSendingService;
        }

        public async Task Schedule(EmailAuditBatch batch)
        {
            var emailsToSend = await _db.EmailAudits
                .Where(e => e.EmailAuditBatchId == batch.Id && e.StatusId == Data.Enums.EmailAuditStatus.Review)
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
                var status = result ? Data.Enums.EmailAuditStatus.Success : Data.Enums.EmailAuditStatus.Failed;
                email.StatusId = status;

                await _db.SaveChangesAsync();
            }

        }
    }
}
