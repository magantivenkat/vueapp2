using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Models;

[assembly: HostingStartup(typeof(GoRegister.Areas.Identity.IdentityHostingStartup))]
namespace GoRegister.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}