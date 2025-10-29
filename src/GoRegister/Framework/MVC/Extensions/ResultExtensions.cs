using GoRegister.ApplicationCore.Framework.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;

namespace GoRegister.Framework.MVC
{
    public static class ResultExtensions
    {
        public static IActionResult ToJsonResult<T>(this Result<T> result)
        {
            if (result.Failed)
            {
                return result.Error.Accept<ActionResultErrorMappingVisitor, IActionResult>(new ActionResultErrorMappingVisitor());
            }

            return new OkObjectResult(result.Value);
        }

        public static ActionResult<TValue> ToActionResult<TValue>(this Result<TValue> result)
        {
            if (result.Failed) return result.Error.ToErrorResult<TValue>();

            return ToSuccessResult(result.Value, value => value);
        }

        public static ActionResult<TModel> ToSuccessResult<TValue, TModel>(
            TValue result,
            Func<TValue, TModel> valueMapper)
            => result is Unit
                ? (ActionResult<TModel>)new NoContentResult()
                : valueMapper(result);

        public static ActionResult<TModel> ToErrorResult<TModel>(this Error error)
            => error.Accept<ActionResultErrorMappingVisitor<TModel>, ActionResult<TModel>>(new ActionResultErrorMappingVisitor<TModel>());
    }
}
