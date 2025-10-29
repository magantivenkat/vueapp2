using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Framework.Domain
{
    public abstract class PopulateViewModelHandlerBase<TViewModel> : INotificationHandler<PopulateViewModelNotification<TViewModel>>
        where TViewModel : class
    {
        public Task Handle(PopulateViewModelNotification<TViewModel> notification, CancellationToken cancellationToken)
        {
            return PopulateViewModel(notification.ViewModel);
        }

        public abstract Task PopulateViewModel(TViewModel model);
    }

    public class PopulateViewModelNotification<T> : INotification
    {
        public T ViewModel { get; set; }

        public PopulateViewModelNotification(T viewModel)
        {
            ViewModel = viewModel;
        }
    }
}
