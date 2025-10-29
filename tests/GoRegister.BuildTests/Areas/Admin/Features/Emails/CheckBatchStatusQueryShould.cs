using FluentAssertions;
using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Framework;
using GoRegister.ApplicationCore.Framework.Multitenancy;
using GoRegister.Areas.Admin.Features.Emails;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GoRegister.BuildTests.Areas.Admin.Features.Emails
{
    public class CheckBatchStatusShould
    {
        protected int ExistingProjectId = 2;
        protected Guid BatchId = new Guid("2CC3ADF6-10ED-4543-94E0-65C1EA644992");

        public ApplicationDbContext GetContextWithData()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var tenant = new ProjectTenant { Id = ExistingProjectId, IsAdmin = true };
            var tenantAccessorMock = new Mock<ITenantAccessor>();
            tenantAccessorMock.Setup(e => e.Get).Returns(tenant);

            var context = new ApplicationDbContext(tenantAccessorMock.Object, options);

            var project = new Project { Id = ExistingProjectId, Name = "Project 1" };

            var emailAuditBatch = new EmailAuditBatch { BatchId = BatchId, ProjectId = ExistingProjectId };
            emailAuditBatch.EmailAudits = new List<EmailAudit> {
                new EmailAudit {StatusId = EmailAuditStatus.Success },
                new EmailAudit {StatusId = EmailAuditStatus.Failed },
                new EmailAudit {StatusId = EmailAuditStatus.Send },
            };
            context.Projects.Add(project);
            context.EmailAuditBatches.Add(emailAuditBatch);
            context.SaveChanges();

            return context;
        }

        [Fact]
        public async Task Test()
        {
            using (var db = GetContextWithData())
            {
                var sut = new CheckBatchStatus.QueryHandler(db);
                var request = new CheckBatchStatus.Query { BatchId = BatchId };
                var result = await sut.Handle(request, new System.Threading.CancellationToken());

                result.Sent.Should().Be(1);
                result.ToSend.Should().Be(1);
                result.Failed.Should().Be(1);
                result.Total.Should().Be(3);
            }

        }
    }
}
