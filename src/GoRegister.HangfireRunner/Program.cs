using Hangfire;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace GoRegister.HangfireRunner
{
    class Program
    {
        static async Task Main(string[] args)
        {
            GlobalConfiguration.Configuration.UseSqlServerStorage("Server=(localdb)\\mssqllocaldb;Database=aspnet-ReEngage-5809A9CD-E62D-496C-B752-3416A8B9EAEA;Trusted_Connection=True;MultipleActiveResultSets=true");

            var hostBuilder = new HostBuilder()
                // Add configuration, logging, ...
                .ConfigureServices((hostContext, services) =>
                {
                    // Add your services with depedency injection.
                });

            using (var server = new BackgroundJobServer(new BackgroundJobServerOptions()
            {
                WorkerCount = 1
            }))
            {
                await hostBuilder.RunConsoleAsync();
            }
        }
    }
}
