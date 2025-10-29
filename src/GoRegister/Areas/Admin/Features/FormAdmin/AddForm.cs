using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Data.Models;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GoRegister.Areas.Admin.Features.FormAdmin
{
    public class AddForm
    {
        public class Command : IRequest<Result>
        {
            public FormType FormType { get; set; }
            public string DisplayName { get; set; }
        }

        public class Result
        {
            public int Id { get; set; }
            public string DisplayName { get; set; }
        }


        public class CommandHandler : IRequestHandler<Command, Result>
        {
            private readonly ApplicationDbContext _db;

            public CommandHandler(ApplicationDbContext db)
            {
                _db = db;
            }

            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                Form form;

                if (request.FormType != FormType.Custom)
                {
                    form = _db.Forms.SingleOrDefault(f => f.FormTypeId == request.FormType);

                    if (form != null && request.FormType != FormType.Custom) return null;
                }

                form = new Form
                {
                    AdminDisplayName = request.DisplayName,
                    FormTypeId = request.FormType,
                };

                _db.Forms.Add(form);
                await _db.SaveChangesAsync();

                var result = new Result { Id = form.Id, DisplayName = form.AdminDisplayName };
                return result;
            }
        }
    }
}
