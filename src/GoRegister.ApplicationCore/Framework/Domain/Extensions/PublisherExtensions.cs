using MediatR;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Framework.Domain
{
    public static class PublisherExtensions
    {
        public static Task PopulateViewModel<TViewModel>(this IPublisher publisher, TViewModel viewModel)
        {
            return publisher.Publish(new PopulateViewModelNotification<TViewModel>(viewModel));
        }
        public static Task PopulateViewModel<TViewModel>(this IMediator mediator, TViewModel viewModel)
        {
            return mediator.Publish(new PopulateViewModelNotification<TViewModel>(viewModel));
        }
    }
}
