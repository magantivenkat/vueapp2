using FluentValidation;
using System;

namespace GoRegister.ApplicationCore.Framework.Validators
{
    public static class URIValidatorExtension
    {
        public static IRuleBuilderInitial<T, string> IsAbsoluteUri<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Custom((uri, context) => {
                if (string.IsNullOrWhiteSpace(uri)) return;

                if(!Uri.TryCreate(uri, UriKind.Absolute, out _))
                {
                    context.AddFailure("Uri is not valid");
                }
            });
        }
    }
}
