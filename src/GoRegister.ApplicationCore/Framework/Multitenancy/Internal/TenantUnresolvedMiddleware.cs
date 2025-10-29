using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace GoRegister.ApplicationCore.Framework.Multitenancy.Internal
{
    public class TenantUnresolvedMiddleware
    {
        private readonly string redirectLocation;
        private readonly RequestDelegate next;

        public TenantUnresolvedMiddleware(
            RequestDelegate next,
            string redirectLocation)
        {
            _ = next ?? throw new ArgumentNullException(nameof(next));

            this.next = next;
            this.redirectLocation = redirectLocation;
        }

        public async Task Invoke(HttpContext context)
        {
            _ = context ?? throw new ArgumentNullException(nameof(context));

            var tenant = context.GetTenant();

            if (tenant == null)
            {
                if (!string.IsNullOrWhiteSpace(redirectLocation))
                {
                    Redirect(context, redirectLocation);
                }
                else
                {
                    context.Response.StatusCode = 404;
                    var file = new FileInfo(@"wwwroot\tenantnotfound.html");
                    byte[] buffer;
                    if (file.Exists)
                    {
                        context.Response.ContentType = "text/html";
                        buffer = File.ReadAllBytes(file.FullName);
                    }
                    else
                    {
                        context.Response.ContentType = "text/plain";
                        buffer = Encoding.UTF8.GetBytes("Project could not be found, please check the url");
                    }

                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    context.Response.ContentLength = buffer.Length;

                    using (var stream = context.Response.Body)
                    {
                        await stream.WriteAsync(buffer, 0, buffer.Length);
                        await stream.FlushAsync();
                    }
                }
            } else
            {
                // otherwise continue processing
                await next(context);
            }

        }
        private void Redirect(HttpContext context, string redirectLocation)
        {
            context.Response.Redirect(redirectLocation);
            context.Response.StatusCode = StatusCodes.Status302Found;
        }
    }
}
