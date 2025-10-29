using AutoMapper;
using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GoRegister.Areas.Admin.Features.Emails
{
    public static class CreateEmail
    {
        public class Query : IRequest<CreateEditEmail.Response>
        {
            public EmailType EmailType { get; set; }
        }

        public class QueryHandler : CreateEditEmail.QueryHandlerBase, IRequestHandler<Query, CreateEditEmail.Response>
        {
            private readonly ApplicationDbContext _db;
            private readonly IMapper _mapper;

            public QueryHandler(ApplicationDbContext db, IMapper mapper) : base(db)
            {
                _db = db;
                _mapper = mapper;
            }

            public async Task<CreateEditEmail.Response> Handle(Query message, CancellationToken cancellationToken)
            {
                var result = new CreateEditEmail.Response();

                var model = new CreateEditEmail.Command();
                model.EmailType = message.EmailType;
                model.DefaultTemplateIndex = 0;
                model.Templates.Add(new CreateEditEmail.EmailTemplateModel());


                result.Model = model;
                result.ViewModel = await GetViewModel();

                return result;
            }
        }
    }
}
