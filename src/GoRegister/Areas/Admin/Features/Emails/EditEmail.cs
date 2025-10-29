using AutoMapper;
using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Framework.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GoRegister.Areas.Admin.Features.Emails
{
    public static class EditEmail
    {
        public class Query : IRequest<Result<CreateEditEmail.Response>>
        {
            public int Id { get; set; }
        }

        public class QueryHandler : CreateEditEmail.QueryHandlerBase, IRequestHandler<Query, Result<CreateEditEmail.Response>>
        {
            private readonly ApplicationDbContext _db;
            private readonly IMapper _mapper;

            public QueryHandler(ApplicationDbContext db, IMapper mapper) : base(db)
            {
                _db = db;
                _mapper = mapper;
            }

            public async Task<Result<CreateEditEmail.Response>> Handle(Query message, CancellationToken cancellationToken)
            {
                return await
                    _db.Emails
                        .Include(e => e.EmailTemplates)
                            .ThenInclude(e => e.RegistrationTypes)
                        .FindResultAsync(e => e.Id == message.Id)
                    .MapAsync(async email =>
                    {
                        var result = new CreateEditEmail.Response();
                        var model = _mapper.Map<CreateEditEmail.Command>(email);
                        var defaultTemplate = email.EmailTemplates.Single(e => e.IsDefault);
                        model.DefaultTemplateIndex = model.Templates.IndexOf(model.Templates.Single(tmp => tmp.Id == defaultTemplate.Id));

                        result.Model = model;
                        result.ViewModel = await GetViewModel();

                        return Result.Ok(result);
                    });
            }
        }
    }
}
