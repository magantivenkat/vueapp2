using System;
using System.Collections.Generic;
using System.Text;

namespace GoRegister.ApplicationCore.Framework.Domain
{
    public abstract class Error
    {
        private Error() { }

        public abstract TResult Accept<TVisitor, TResult>(TVisitor visitor)
            where TVisitor : IErrorVisitor<TResult>;

        public interface IErrorVisitor<out TVisitResult>
        {
            TVisitResult Visit(NotFound result);

            TVisitResult Visit(Invalid result);

            TVisitResult Visit(Unauthorized result);

            TVisitResult Visit(Unknown result);
        }

        public sealed class NotFound : Error
        {
            public NotFound(string message)
            {
                Message = message;
            }

            public string Message { get; }

            public override TResult Accept<TVisitor, TResult>(TVisitor visitor)
                => visitor.Visit(this);
        }

        public sealed class Invalid : Error
        {
            public Invalid(string message)
            {
                Message = message;
                Errors.Add("", new List<string> { message });
            }

            public string Message { get; }

            public Dictionary<string, List<string>> Errors = new Dictionary<string, List<string>>();

            public Invalid WithError(string message)
            {
                return WithError("", message);
            }

            public Invalid WithError(string key, string message)
            {
                if(Errors.ContainsKey(key))
                {
                    Errors[key].Add(message);
                } else
                {
                    Errors.Add("", new List<string> { message });
                }

                return this;
            }

            public override TResult Accept<TVisitor, TResult>(TVisitor visitor)
                => visitor.Visit(this);
        }

        public sealed class Unauthorized : Error
        {
            public Unauthorized(string message)
            {
                Message = message;
            }

            public string Message { get; }

            public override TResult Accept<TVisitor, TResult>(TVisitor visitor)
                => visitor.Visit(this);
        }

        public sealed class Unknown : Error
        {
            public override TResult Accept<TVisitor, TResult>(TVisitor visitor)
                => visitor.Visit(this);
        }
    }
}
