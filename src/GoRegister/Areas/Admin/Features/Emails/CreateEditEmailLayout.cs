using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Models.Emails;
using GoRegister.ApplicationCore.Framework.Domain;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GoRegister.Areas.Admin.Features.Emails
{
    public static class CreateEditEmailLayout
    {
        public class Response : IRequest<int>
        {
            public Command Model { get; set; }
        }

        public class Command : IRequest<Result<int>>
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public List<EmailTemplateModel> Templates { get; set; } = new List<EmailTemplateModel>();
        }

        public class EmailTemplateModel
        {
            public string BodyHtml { get; set; }
        }


        public class CommandHandler : IRequestHandler<Command, Result<int>>
        {
            private readonly ApplicationDbContext _db;

            public CommandHandler(ApplicationDbContext db)
            {
                _db = db;
            }

            public async Task<Result<int>> Handle(Command request, CancellationToken cancellationToken)
            {
                var emailLayout = new EmailLayout();
                if (request.Id == 0)
                {
                    // Create new EmailLayout
                    emailLayout = new EmailLayout
                    {
                        Name = request.Name,
                        Html = request.Templates.First().BodyHtml
                    };

                    _db.Add(emailLayout);
                }
                else
                {
                    emailLayout = await _db.EmailLayouts.FindAsync(request.Id);
                    emailLayout.Name = request.Name;
                    emailLayout.Html = request.Templates.First().BodyHtml;
                }

                await _db.SaveChangesAsync();
                return Result.Ok(emailLayout.Id);
            }
        }
    }
}

