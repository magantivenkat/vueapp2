using GoRegister.ApplicationCore.Framework.Domain;
using GoRegister.ApplicationCore.Framework.Domain.Mediatr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GoRegister.Filters
{
    public class ModelStateValidationAttribute : ActionFilterAttribute
    {
        public string ViewName { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var publisher = context.HttpContext.RequestServices.GetService<IPublisher>();                
                object vm = null;

                foreach (var parameter in context.ActionArguments)
                {
                    if (parameter.Value is IValidatableViewModel && parameter.Value != null)
                    {
                        var paramType = parameter.Value.GetType();
                        var invalidResponseType = typeof(PopulateViewModelNotification<>).MakeGenericType(paramType);
                        var invalidResponse =
                            Activator.CreateInstance(invalidResponseType, parameter.Value);

                        publisher.Publish(invalidResponse);
                        vm = parameter.Value;
                    }
                }

                var controller = context.Controller as Controller;

                if (vm != null)
                {
                    context.Result = string.IsNullOrWhiteSpace(ViewName) ?
                        controller.View(vm) :
                        controller.View(ViewName, vm);
                }
                else
                {
                    context.Result = string.IsNullOrWhiteSpace(ViewName) ?
                        controller.View() :
                        controller.View(ViewName);
                }
            }
        }
    }
}
