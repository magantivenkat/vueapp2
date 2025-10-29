using MediatR.Pipeline;
using System.Threading;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Framework.Domain.Mediatr
{
    public class PopulateViewModelPostProcessor<TRequest, TResponse> : IRequestPostProcessor<TRequest, TResponse>
    {
        private readonly IPublisher _publisher;

        public PopulateViewModelPostProcessor(IPublisher publisher)
        {
            _publisher = publisher;
        }

        public async Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
        {
            if (response is IResult && request is IValidatableViewModel)
            {
                var iresult = response as IResult;
                if (iresult.Failed)
                {
                    await _publisher.PopulateViewModel(request);
                }
            }
        }
    }
}
