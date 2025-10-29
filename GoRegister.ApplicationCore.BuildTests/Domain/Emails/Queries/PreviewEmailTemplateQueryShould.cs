using FluentAssertions;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Delegates.Queries;
using GoRegister.ApplicationCore.Domain.Emails.Queries;
using GoRegister.ApplicationCore.Framework.Domain;
using GoRegister.TestingCore;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GoRegister.ApplicationCore.BuildTests.Domain.Delegates
{
    public class PreviewEmailTemplateQueryShould : DatabaseContextTest
    {
        private string _emailBody = "<p>Hi</p>";

        public PreviewEmailTemplateQueryShould()
        {
            using (var db = GetDatabase())
            {
                var emailTemplate = new EmailTemplate
                {
                    Id = 1,
                    BodyHtml = "<div>{{emailBody}}</div>"
                };

                db.Add(emailTemplate);
                db.SaveChanges();
            }
        }

        [Fact]
        public async Task Return_String()
        {
            using (var db = GetDatabase())
            {
                var sut = new PreviewEmailTemplate.QueryHandler(db);
                var query = new PreviewEmailTemplate.Query { EmailBody = _emailBody };
                var result = await sut.Handle(query, new CancellationToken());
                result.Should().BeOfType<string>();
            }
        }

        [Fact]
        public async Task OnlyReturn_EmailBody_WhenTemplate_NULL()
        {
            using (var db = GetDatabase())
            {
                var sut = new PreviewEmailTemplate.QueryHandler(db);
                var query = new PreviewEmailTemplate.Query { EmailTemplateId = 0, EmailBody = _emailBody };
                var result = await sut.Handle(query, new CancellationToken());
                result.Should().Be(_emailBody);
            }
        }

        [Fact]
        public async Task Return_MergedTemplateANDContent_WhenTemplate_IsNotNULL()
        {
            using (var db = GetDatabase())
            {
                var sut = new PreviewEmailTemplate.QueryHandler(db);
                var query = new PreviewEmailTemplate.Query { EmailTemplateId = 1, EmailBody = _emailBody };
                var result = await sut.Handle(query, new CancellationToken());
                result.Should().Be($"<div>{_emailBody}</div>");
            }
        }
    }
}
