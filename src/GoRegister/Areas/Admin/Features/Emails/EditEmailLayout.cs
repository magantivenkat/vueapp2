using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Framework.Domain;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GoRegister.Areas.Admin.Features.Emails
{
    public static class EditEmailLayout
    {
        public class Query : IRequest<Result<CreateEditEmailLayout.Response>>
        {
            public int Id { get; set; }
        }

        public class QueryHandler :  IRequestHandler<Query, Result<CreateEditEmailLayout.Response>>
        {
            private readonly ApplicationDbContext _db;

            public QueryHandler(ApplicationDbContext db)
            {
                _db = db;
            }

            public async Task<Result<CreateEditEmailLayout.Response>> Handle(Query message, CancellationToken cancellationToken)
            {
                return await
                    _db.EmailLayouts.FindResultAsync(el => el.Id == message.Id)
                .MapAsync(async email => {
                     var result = new CreateEditEmailLayout.Response();
                     var model = new CreateEditEmailLayout.Command
                     {
                         Id = email.Id,                         
                         Name = email.Name,
                         Templates = new List<CreateEditEmailLayout.EmailTemplateModel> { new CreateEditEmailLayout.EmailTemplateModel { BodyHtml = email.Html } }
                     };

                     result.Model = model;

                     return Result.Ok(result);
                 });

            }
        }
    }
}
