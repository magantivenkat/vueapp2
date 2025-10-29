using GoRegister.ApplicationCore.Framework.MVC;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace GoRegister.Filters
{
    public class ThemedResultFilter : TypeFilterAttribute
    {
        public ThemedResultFilter() : base(typeof(InternalThemedResultFilter)) { }

        private class InternalThemedResultFilter : IAsyncResultFilter
        {
            private readonly GoRegisterPage _page;

            public InternalThemedResultFilter(GoRegisterPage page)
            {
                _page = page;
            }

            public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
            {
                if(context.Result is ViewResult)
                {

                }

                await next();
            }
        }
    }
}
