using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.Features.Errors
{
    //[AllowAnonymous]
    //[Area("Admin")]
    public class ErrorsTenantController : Controller
    {
        [Route("Errors/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

                    //ViewBag.OriginalPath = statusCodeResult.OriginalPath;
                    //ViewBag.OriginalPathBase = statusCodeResult.OriginalPathBase;
                    //ViewBag.OriginalQueryString = statusCodeResult.OriginalQueryString;

                    Log.ForContext("OriginalPath", statusCodeResult.OriginalPath)
                        .ForContext("OriginalPathBase", statusCodeResult.OriginalPathBase)
                        .ForContext("OriginalQueryString", statusCodeResult.OriginalQueryString)
                        .Warning("Invalid Url Accessed");

                    return View("NotFound");
                case 500:
                case 0:
                    var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
                    //ViewBag.ErrorDescription = exceptionDetails.Error;
                    //ViewBag.ErrorPath = exceptionDetails.Path;

                    Log.ForContext("ErrorDescription", exceptionDetails.Error)
                            .ForContext("ErrorPath", exceptionDetails.Path)
                            .Error("Exception occured");

                    return View("ErrorPage");
                default:
                    return View("NotFound");
            }
        }
    }
}
