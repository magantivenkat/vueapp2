using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GoRegister.ApplicationCore.Framework.Notifications
{
    public class NotificationsFilter : IActionFilter
    {
        private const string Cookie = "notifications";

        private readonly INotifier _notifier;

        public NotificationsFilter(INotifier notifier)
        {
            _notifier = notifier;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var json = Convert.ToString(context.HttpContext.Request.Cookies[Cookie]);
            if (String.IsNullOrWhiteSpace(json))
            {
                return;
            }

            _notifier.AddJson(json);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (!_notifier.Rendered && _notifier.List().Any() && (context.Result is RedirectResult || context.Result is RedirectToActionResult || context.Result is RedirectToPageResult || context.Result is RedirectToRouteResult))
            {
                context.HttpContext.Response.Cookies.Append(Cookie, _notifier.GetJson());
            }
            else if(!string.IsNullOrWhiteSpace(context.HttpContext.Request.Cookies[Cookie]))
            {
                context.HttpContext.Response.Cookies.Delete(Cookie);
            }
        }
    }
}
