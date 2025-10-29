using AutoMapper;
using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Framework.Domain;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GoRegister.Areas.Admin.Features.Emails
{
    public static class Preview
    {
        public class Query : IRequest<Result<string>>
        {
            public int Id { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, Result<string>>
        {
            private readonly ApplicationDbContext _db;
            private readonly IMapper _mapper;

            public QueryHandler(ApplicationDbContext db, IMapper mapper)
            {
                _db = db;
                _mapper = mapper;
            }

            public async Task<Result<string>> Handle(Query message, CancellationToken cancellationToken)
            {
                return await _db.EmailAudits.FindResultAsync(e => e.Id == message.Id)
                        .MapAsync(email =>
                        {
                            return Task.FromResult(Result.Ok(email.Body));
                        });
            }
        }
    }
}
