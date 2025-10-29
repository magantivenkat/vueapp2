using FluentAssertions;
using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Framework;
using GoRegister.ApplicationCore.Framework.Domain;
using GoRegister.ApplicationCore.Jobs;
using GoRegister.ApplicationCore.Services.Email;
using GoRegister.TestingCore;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GoRegister.ApplicationCore.BuildTests.Hangfire.Jobs
{
    public class SendEmailJobTests : DatabaseContextTest
    {
        private Mock<IEmailSendingService> mockEmailSendingService = new Mock<IEmailSendingService>();
        private int EMAILAUDITBATCHID = 1;
        private int ACTIONEDBYID = 777;

        private void SetupDatabaseData(int delegateRegStatusId)
        {
            using (var db = GetDatabase())
            {
                // add db data
                var emailAudit = new EmailAudit
                {
                    EmailAuditBatchId = EMAILAUDITBATCHID,
                    StatusId = EmailAuditStatus.Review,
                    EmailAuditBatch = new EmailAuditBatch { CreatedByUserId = ACTIONEDBYID },
                    EmailTemplate = new EmailTemplate { Email = new Email { EmailType = EmailType.Invitation } },
                    DelegateUser = new DelegateUser { RegistrationStatusId = delegateRegStatusId }
                };
                db.EmailAudits.Add(emailAudit);
                db.SaveChanges();
            }
        }

        [Fact]
        public async Task SuccessfullySendEmail_UpdateDelegateStatusANDAudit()
        {
            SetupDatabaseData((int)Data.Enums.RegistrationStatus.NotInvited);

            mockEmailSendingService.Setup(ess => ess.Send(It.IsAny<EmailObject>())).Returns(Task.FromResult(Result.Ok()));

            using (var db = GetDatabase())
            {
                var sut = new SendEmailsJob(db, mockEmailSendingService.Object, ProjectAdminTenantAccessor.Object);
                await sut.Execute(EMAILAUDITBATCHID, It.IsAny<ProjectTenant>());

                // get updated email and check if changes successful
                var emailAuditResult = db.EmailAudits.SingleOrDefault(ea => ea.EmailAuditBatchId == EMAILAUDITBATCHID);
                var delegateAuditResult = db.DelegateAudits.LastOrDefault();
                emailAuditResult.DelegateUser.RegistrationStatusId.Should().Be((int)Data.Enums.RegistrationStatus.Invited);
                delegateAuditResult.ActionedById.Should().Be(ACTIONEDBYID);
            }
        }

        [Theory]
        [InlineData(Data.Enums.RegistrationStatus.Confirmed)]
        [InlineData(Data.Enums.RegistrationStatus.Declined)]
        public async Task SendEmail_BUT_DONOTUpdateDelegateStatus(Data.Enums.RegistrationStatus regStatus)
        {
            SetupDatabaseData((int)regStatus);

            mockEmailSendingService.Setup(ess => ess.Send(It.IsAny<EmailObject>())).Returns(Task.FromResult(Result.Ok()));

            using (var db = GetDatabase())
            {
                var sut = new SendEmailsJob(db, mockEmailSendingService.Object, ProjectAdminTenantAccessor.Object);
                await sut.Execute(EMAILAUDITBATCHID, It.IsAny<ProjectTenant>());

                // get updated email and check if changes successful
                var emailAuditResult = db.EmailAudits.SingleOrDefault(ea => ea.EmailAuditBatchId == EMAILAUDITBATCHID);
                var delegateAuditResult = db.DelegateAudits.LastOrDefault();
                emailAuditResult.DelegateUser.RegistrationStatusId.Should().Be((int)regStatus);
            }
        }

        [Fact]
        public async Task EmailTypeConfirmation_EmailStatusSuccess()
        {
            mockEmailSendingService.Setup(ess => ess.Send(It.IsAny<EmailObject>())).Returns(Task.FromResult(Result.Ok()));
            var emailAudit = new EmailAudit
            {
                EmailAuditBatchId = EMAILAUDITBATCHID,
                StatusId = EmailAuditStatus.Review,
                EmailAuditBatch = new EmailAuditBatch { CreatedByUserId = ACTIONEDBYID },
                EmailTemplate = new EmailTemplate { Email = new Email { EmailType = EmailType.Confirmation } },
            };

            using (var db = GetDatabase())
            {
                db.EmailAudits.Add(emailAudit);
                db.SaveChanges();

                var sut = new SendEmailsJob(db, mockEmailSendingService.Object, ProjectAdminTenantAccessor.Object);
                await sut.Execute(EMAILAUDITBATCHID, It.IsAny<ProjectTenant>());

                // get updated email and check if changes successful
                var confirmationEmail = await db.EmailAudits.Include(ea => ea.EmailTemplate).ThenInclude(e => e.Email).SingleOrDefaultAsync(e => e.EmailTemplate.Email.EmailType == EmailType.Confirmation);
                confirmationEmail.StatusId.Should().Be(EmailAuditStatus.Success);
            }
        }

        [Fact]
        public async Task FailedEmail_UpdateEmailStatusFailed()
        {
            SetupDatabaseData(It.IsAny<int>());
            mockEmailSendingService.Setup(ess => ess.Send(It.IsAny<EmailObject>())).Returns(Task.FromResult(Result.Fail()));

            using (var db = GetDatabase())
            {
                var sut = new SendEmailsJob(db, mockEmailSendingService.Object, ProjectAdminTenantAccessor.Object);
                await sut.Execute(EMAILAUDITBATCHID, It.IsAny<ProjectTenant>());

                // get updated email and check if changes failed
                var emailAuditResult = db.EmailAudits.SingleOrDefault(ea => ea.EmailAuditBatchId == EMAILAUDITBATCHID);

                emailAuditResult.StatusId.Should().Be(EmailAuditStatus.Failed);
            }

        }
    }
}
