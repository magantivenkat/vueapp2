using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoRegister.ApplicationCore.Framework.Domain.Mediatr
{
    public abstract class ValidatableRequest<TResponse> : IValidatable, IRequest<TResponse>
    {
        public bool IsValidated { get; set; }
    }

    public abstract class ValidatableViewModelRequest<TResponse> : ValidatableRequest<TResponse>, IValidatableViewModel
    {

    }

    public interface IValidatable
    {
        bool IsValidated { get; set; }
    }

    public interface IValidatableViewModel : IValidatable
    {

    }
}
