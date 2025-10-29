using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Framework.Domain.Mediatr
{
    public class Publisher : IPublisher
    {
        private readonly IMediator _mediator;

        public Publisher(IMediator mediator)
        {
            this._mediator = mediator;
        }

        public Task Publish(object notification, CancellationToken cancellationToken = default)
        {
            return _mediator.Publish(notification, cancellationToken);
        }

        public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification
        {
            return _mediator.Publish(notification, cancellationToken);
        }
    }
}
