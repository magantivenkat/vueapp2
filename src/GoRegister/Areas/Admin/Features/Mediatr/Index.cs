using GoRegister.ApplicationCore.Framework.Domain;
using MediatR;
using MediatR.Pipeline;
using System.Threading;
using System.Threading.Tasks;

namespace GoRegister.Areas.Admin.Features.Mediatr
{
    public static class Index
    {
        public class Query : IRequest<Result<Command>>
        {
        }

        public class QueryHandler : PagesCommandHandler<Query, Command, int>
        {
            public override async Task<Result<Command>> OnGetAsync(Query request)
            {
                var command = new Command();
                await PopulateViewModel(command);
                return Result.Ok(command);
            }

            public override Task<Result<int>> OnPostAsync(Command request)
            {
                return Task.FromResult(Result.Ok(1));
            }

            protected override Task PopulateViewModel(Command command)
            {
                command.Messages = new[] { "Hey" };
                return Task.CompletedTask;
            }
        }

        public class Command : PopulateQueryBase<int>
        {
            public string Message { get; set; }
            public string[] Messages { get; set; }
        }

        public abstract class PagesCommandHandler<TGet, TPost, TPostResult> : PopulateCommandHandler<TPost, TPostResult>, IRequestHandler<TGet, Result<TPost>>
            where TGet : IRequest<Result<TPost>>
            where TPost : PopulateQueryBase<TPostResult> 
        {

            public abstract Task<Result<TPost>> OnGetAsync(TGet request);


            public Task<Result<TPost>> Handle(TGet request, CancellationToken cancellationToken) => OnGetAsync(request);
        }

        public abstract class PopulateCommandHandler<TPost, TPostResult> : PopulateCommandHandlerBase<TPost, TPostResult>, IRequestHandler<TPost, Result<TPostResult>>
            where TPost : PopulateQueryBase<TPostResult>
            //where TCommand : PopulateCommandBase
        {

            public abstract Task<Result<TPostResult>> OnPostAsync(TPost request);

            public Task<Result<TPostResult>> Handle(TPost request, CancellationToken cancellationToken)
            {
                return OnPostAsync(request);
            }
        }

        public abstract class PopulateCommandHandlerBase<TPost, TPostResult> : INotificationHandler<TPost>
            where TPost : PopulateQueryBase<TPostResult>
            //where TCommand : PopulateCommandBase
        {

            protected abstract Task PopulateViewModel(TPost command);

            async Task INotificationHandler<TPost>.Handle(TPost notification, CancellationToken cancellationToken)
            {
                await PopulateViewModel(notification);
            }
        }

        public class PopulateViewModelContainer<TViewModel> where TViewModel : class
        {
            public TViewModel ViewModel { get; set; }
        }

        public abstract class PopulateQueryBase<TCommand> : IRequest<Result<TCommand>>, INotification
            //where TCommand : PopulateCommandBase
        {

        }
    }

    //public class ConstrainedRequestPostProcessor<TRequest, TResponse>
    //    : IRequestPostProcessor<TRequest, TResponse>
    //    where TRequest : Ping
    //{

    //    public ConstrainedRequestPostProcessor(TextWriter writer)
    //    {
    //        _writer = writer;
    //    }

    //    public Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
    //    {
    //        return _writer.WriteLineAsync("- All Done with Ping");
    //    }
    //}
}
