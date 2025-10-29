using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static GoRegister.ApplicationCore.Framework.Domain.Error;

namespace GoRegister.ApplicationCore.Framework.Domain.Mediatr
{
    public class ValidationPipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        //where TResponse : class
        //TODO waiting for .net 5 to do this type of constraint in DI (where TRequest : IValidatable)
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        private readonly ILogger<TRequest> _logger;
        private readonly IPublisher _publisher;

        public ValidationPipelineBehaviour(IEnumerable<IValidator<TRequest>> validators, ILogger<TRequest> logger, IPublisher publisher)
        {
            _validators = validators;
            _logger = logger;
            _publisher = publisher;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if(request is IValidatable)
            {
                var failures = _validators
                    .Select(v => v.Validate(request))
                    .SelectMany(result => result.Errors)
                    .Where(f => f != null)
                    .ToList();

                if (failures.Any())
                {
                    var responseType = typeof(TResponse);

                    if (responseType.IsGenericType)
                    {
                        var resultType = responseType.GetGenericArguments()[0];
                        var invalidResponseType = typeof(Result<>).MakeGenericType(resultType);

                        var error = new Invalid(string.Join(',', failures.Select(e => e.ToString())));

                        var invalidResponse =
                            Activator.CreateInstance(invalidResponseType, resultType.IsValueType ? default : Activator.CreateInstance(resultType), error);

                        if(request is IValidatableViewModel)
                        {
                            await _publisher.PopulateViewModel(request);
                        }

                        return (TResponse)invalidResponse;
                    }
                }
            }
            

            var response = await next();

            return response;
        }
    }
}
