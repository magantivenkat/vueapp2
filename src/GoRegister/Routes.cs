using Microsoft.AspNetCore.Builder;

namespace GoRegister
{
    public static class Routes
    {
        public static IApplicationBuilder BuildRoutes(this IApplicationBuilder app)
        {
            app.UseEndpoints(e =>
            {
                e.MapRazorPages();
                e.MapControllers();
                e.MapDefaultControllerRoute();

                e.MapControllerRoute(
                    "SlugRoute",
                    "{slug=}",
                    new { controller = "Content", action = "Index" }
                );


            });

            return app;
        }
    }
}
