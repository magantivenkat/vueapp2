using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GoRegister.Areas.Admin.Features.Emails
{
    public static class ListEmails
    {
        public class Query : IRequest<Result>
        {
        }

        public class Result
        {
            public List<RegistrationEmailItem> RegistrationEmails { get; set; }
            public IEnumerable<EmailItem> CustomEmails { get; set; }
            public List<EmailItem> EmailLayouts { get; set; }
        }

        public class EmailItem
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class RegistrationEmailItem
        {
            public RegistrationEmailItem(EmailType type, int? id)
            {
                EmailTypeId = type;
                Id = id;
                Name = type.ToString();
            }

            public int? Id { get; set; }
            public string Name { get; set; }
            public EmailType EmailTypeId { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, Result>
        {
            private readonly ApplicationDbContext _db;
            private readonly EmailType[] registrationEmailTypes = new EmailType[] { EmailType.Invitation, EmailType.Confirmation, EmailType.Decline, EmailType.Cancellation };

            public QueryHandler(ApplicationDbContext db)
            {
                _db = db;
            }

            public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                var emails = await _db.Emails.ToListAsync();
       
                var emailTemplates = await _db.EmailLayouts.Include(et => et.Project).Where(p => p.ProjectId == emails.FirstOrDefault().ProjectId).Select(et => new EmailItem
                {
                    Id = et.Id,
                    Name = et.Name
                }).ToListAsync();

                var regEmails = new List<RegistrationEmailItem>();
                foreach (var type in registrationEmailTypes)
                {
                    var existingEmail = emails.FirstOrDefault(e => e.EmailType == type);
                    regEmails.Add(new RegistrationEmailItem(type, existingEmail?.Id));
                }

                var customEmails = emails.Where(e => e.EmailType == EmailType.Custom).Select(e => new EmailItem
                {
                    Id = e.Id,
                    Name = e.Name
                });

                return new Result
                {
                    RegistrationEmails = regEmails,
                    CustomEmails = customEmails,
                    EmailLayouts = emailTemplates
                };
            }
        }
    }
}
