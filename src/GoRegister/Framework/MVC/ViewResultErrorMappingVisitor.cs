using GoRegister.ApplicationCore.Framework.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.Framework.MVC
{
    public readonly struct ViewResultErrorMappingVisitor<TModel> : Error.IErrorVisitor<ActionResult<TModel>>
    {
        public ActionResult<TModel> Visit(Error.NotFound result)
            => new NotFoundObjectResult(result.Message);

        public ActionResult<TModel> Visit(Error.Invalid result)
            => new ViewResult();

        public ActionResult<TModel> Visit(Error.Unauthorized result)
            => new StatusCodeResult(StatusCodes.Status403Forbidden);

        public ActionResult<TModel> Visit(Error.Unknown result)
            => new StatusCodeResult(StatusCodes.Status500InternalServerError);
    }
}
