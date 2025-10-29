using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoRegister.MRFAPI.Middleware;
using GoRegister.MRFAPI.Services;
using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Models;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using GoRegister.ApplicationCore.Framework;
using GoRegister.ApplicationCore.Framework.Multitenancy;
using Microsoft.AspNetCore.Identity;
using Amazon.S3;
using AspNetCore.Proxy;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using AutoMapper;
using GoRegister.ApplicationCore.Domain;
using GoRegister.ApplicationCore.Framework.Identity;
using GoRegister.ApplicationCore.Framework.Domain;
using Serilog;

namespace GoRegister.MRFAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
               .AddMultitenancy<ProjectTenant, ProjectTenantResolver>();
            services.AddCors();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ASPNetCoreWebAPiDemo", Version = "v1" });
            });

            services.WithPerTenantOptions<SecurityStampValidatorOptions>((options, tenant) =>
            {
                if (tenant.IsAdmin)
                {
                    options.ValidationInterval = TimeSpan.FromSeconds(30);
                }
            });



            services.AddAutoMapper(typeof(AutoMapperServiceCollectionExtension), typeof(Startup));


            services.AddDbContext<ApplicationDbContext>(x => x.UseSqlServer(Configuration.GetConnectionString("ApiConnection")));
            services.AddScoped<IMRFClientRequestService, MRFClientRequestService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ASPNetCoreWebAPiDemo v1"));


                using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                    if (context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
                    {
                        if (Configuration.GetValue<bool>("UseSqlite"))
                        {
                            context.Database.EnsureCreated();
                        }
                        else
                        {
                            context.Database.Migrate();
                        }
                    }
                }

                app.UseProxies(proxies =>
                {
                    proxies.Map("__hmr/{**path}", proxy => proxy.UseHttp((_, args) => $"http://localhost:8081/__hmr/{args["path"]}"));
                    proxies.Map("sockjs-node/{**path}", proxy => proxy.UseHttp((_, args) => $"http://localhost:8081/sockjs-node/{args["path"]}"));
                });
            }
            else
            {
                app.UseStatusCodePagesWithReExecute("/Errors/{0}");

                app.UseExceptionHandler("/Errors/{0}");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();


                if (Configuration.GetValue<bool>("UseSqlite"))
                {
                    using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                    {
                        var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                        if (context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
                        {
                            context.Database.EnsureCreated();
                        }
                    }
                }

            }
            app.UseSerilogRequestLogging();

            app.UseMiddleware<ApiKeyMiddleware>();
            app.UseHttpsRedirection();
            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials());
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
