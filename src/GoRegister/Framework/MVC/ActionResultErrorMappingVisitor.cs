using GoRegister.ApplicationCore.Framework.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.Framework.MVC
{
    public readonly struct ActionResultErrorMappingVisitor<TModel> : Error.IErrorVisitor<ActionResult<TModel>>
    {
        public ActionResult<TModel> Visit(Error.NotFound result)
            => new NotFoundObjectResult(result.Message);

        public ActionResult<TModel> Visit(Error.Invalid result)
            => new BadRequestObjectResult(result.Message);

        public ActionResult<TModel> Visit(Error.Unauthorized result)
            => new StatusCodeResult(StatusCodes.Status403Forbidden);

        public ActionResult<TModel> Visit(Error.Unknown result)
            => new StatusCodeResult(StatusCodes.Status500InternalServerError);
    }

    public readonly struct ActionResultErrorMappingVisitor : Error.IErrorVisitor<IActionResult>
    {
        public IActionResult Visit(Error.NotFound result)
            => new NotFoundObjectResult(result.Message);

        public IActionResult Visit(Error.Invalid result)
        {
            var response = new JsonResult(new { Errors = result.Errors });
            response.StatusCode = StatusCodes.Status400BadRequest;
            return response;
        }

        public IActionResult Visit(Error.Unauthorized result)
            => new StatusCodeResult(StatusCodes.Status403Forbidden);

        public IActionResult Visit(Error.Unknown result)
            => new StatusCodeResult(StatusCodes.Status500InternalServerError);
    }
}
