//--This is development brach for GoRegister MRF code changes//
using AutoMapper;
using FluentValidation.AspNetCore;
using Fluid;
using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain;
using GoRegister.ApplicationCore.Domain.Registration.Fields;
using GoRegister.ApplicationCore.Domain.Registration.Framework;
using GoRegister.ApplicationCore.Framework;
using GoRegister.ApplicationCore.Framework.Multitenancy;
using GoRegister.ApplicationCore.Framework.Multitenancy.Internal;
using GoRegister.Framework.JSON.Contracts;
using GoRegister.Framework.MVC;
using GoRegister.ApplicationCore.Framework.Notifications;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc.Internal;
//using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Microsoft.Extensions.Hosting;
using AspNetCore.Proxy;
using GoRegister.Hubs;
using Hangfire;
using GoRegister.ApplicationCore.Services;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using GoRegister.ApplicationCore.Data.Extensions;
using GoRegister.Framework.Identity.Claims;
using GoRegister.Filters;
using GoRegister.ApplicationCore.Framework.Multitenancy.Filters;
using GoRegister.ApplicationCore.Framework.Domain.Mediatr;
using GoRegister.ApplicationCore.Framework.Identity;
using GoRegister.ApplicationCore.Hangfire;
using Serilog;
using Microsoft.AspNetCore.Identity.UI.Services;
using GoRegister.Framework.Identity;
using Microsoft.AspNetCore.Authorization;
using GoRegister.Framework.Authorization;
using GoRegister.Security.Middleware;
using GoRegister.ApplicationCore.Framework.Domain;
using Microsoft.AspNetCore.DataProtection;
using GoRegister.Framework.Security.DataProtection.AWS;
using Microsoft.AspNetCore.Mvc;
using System;
using Amazon.S3;
using GoRegister.ApplicationCore.Framework.MVC;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IO;
using Microsoft.AspNetCore.Http;
using GoRegister.ApplicationCore.Domain.Delegates.Models;
using System.Collections;
using System.Text.Encodings.Web;
using Microsoft.EntityFrameworkCore.Design;
using System.Configuration;


namespace GoRegister
{
    public class Startup
    {
        static Startup()
        {
            TemplateContext.GlobalMemberAccessStrategy.IgnoreCasing = true;
            // project
            TemplateContext.GlobalMemberAccessStrategy.Register<GoRegister.ApplicationCore.Domain.Projects.Models.ProjectSettingsLiquidModel>();
            // attendee
            TemplateContext.GlobalMemberAccessStrategy.Register<DelegateDataTagAccessor>();
            TemplateContext.GlobalMemberAccessStrategy.Register<DelegateDataTagAccessor, object>((obj, name) => obj.GetValue(name));
            TemplateContext.GlobalMemberAccessStrategy.Register<AttendeeCategorySessionModel>();
            TemplateContext.GlobalMemberAccessStrategy.Register<AttendeeSessionModel>();
            TemplateContext.GlobalMemberAccessStrategy.Register<IDictionary, object>((obj, name) => obj[name]);
        }

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
            //services.AddSpaStaticFiles(config => config.RootPath = "client-app/js");
            //services.AddDbContext<TenantDbContext>(options =>
            //    options.UseSqlServer(
            //        Configuration.GetConnectionString("DefaultConnection")));
            services
                .AddMultitenancy<ProjectTenant, ProjectTenantResolver>();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                if (Configuration.GetValue<bool>("UseSqlite"))
                {
                    options.UseSqlite(Configuration.GetConnectionString("DefaultConnection"), sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly("GoRegister.Migrations");
                    });
                }
                else
                {
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly("GoRegister.Migrations");
                    });

                }
            });

            if (Env.EnvironmentName != Environments.Development)
            {
                var smConfig = Configuration.GetSection("AwsSM").Get<SystemsManagerSettings>();
                services.AddDataProtection()
                    .PersistKeysToAWSSystemsManager($"/{smConfig.ApplicationName}/DataProtection")
                    .SetApplicationName(smConfig.ApplicationName);
            }

            services.AddIdentity<ApplicationUser, UserRole>(options => {
                options.Password.RequiredLength = 8;
                options.SignIn.RequireConfirmedEmail = true;
            })
                //.AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddClaimsPrincipalFactory<GoRegisterUserClaimsPrincipalFactory>();

            services
                .AddAuthentication();


            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection")));

            // use webapp as hangfire server
            services.AddHangfireServer();

            services.AddAntiforgery();

            services.AddResponseCaching(opts =>
            {

            });
            services
                .AddMvc(setup =>
                {
                    //setup.Filters.Add<TenantFilter>();
                    setup.Filters.Add<SiteAccessFilter>();
                    setup.Filters.Add<NotificationsFilter>();
                    setup.Filters.Add<SitewidePasswordFilter>();
                    setup.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
                    //setup.Filters.Add<ModelStateValidationAttribute>();
                    setup.Conventions.Add(new FeatureFolderConvention());
                })
                .AddRazorOptions(options =>
                {
                    options.ViewLocationFormats.Add("/{0}.cshtml");
                    options.AddFeatureViewLocation();
                    options.ViewLocationExpanders.Add(new FeatureViewExpander());
                })
                .AddRazorPagesOptions(options =>
                {
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.ContractResolver = new NullToEmptyStringContractResolver();
                    options.UseCamelCasing(true);
                })
                .AddFluentValidation(cfg =>
                    cfg.RegisterValidatorsFromAssemblyContaining<Startup>()
                        .RegisterValidatorsFromAssemblyContaining<ApplicationCore.Marker>()
                );

            services.AddMediatR(typeof(Startup), typeof(ApplicationCore.Marker));

            services
                .WithPerTenantOptions<CookieAuthenticationOptions>((options, tenant) =>
                {
                    var cookiePrefix = tenant.IsAdmin ? 1 : tenant.Id;
                    options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;

                    options.Cookie.Name = cookiePrefix + ".Asp.Net.Identity";
                    if (!string.IsNullOrWhiteSpace(tenant.Prefix))
                    {
                        options.Cookie.Path = $"/{tenant.Prefix}";
                    }

                    if (!tenant.IsAdmin)
                    {
                        options.LoginPath = "/App/Login";
                    }
                    else
                    {
                        options.LoginPath = "/Identity/Account/Login";
                        options.SlidingExpiration = true;
                        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                    }
                });

            services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme, options =>
            {
                //options.LoginPath = $"{options.Cookie.Path}{options.LoginPath}";
                //options.LogoutPath = $"{options.Cookie.Path}{options.LogoutPath}";
            });

            services.AddAutoMapper(typeof(AutoMapperServiceCollectionExtension), typeof(Startup));

            services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();

            services.AddDomainFramework();

            // mvc
            services.AddScoped((sp) => new GoRegisterPage());

            // identity
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddDataServices();
            services.AddFormDrivers();
            services.AddGoRegister();
            services.AddServices();
            services.AddDomainServices();

            // add hangfire jobs
            services.AddJobs();

            services.AddMemoryCache();

            services.AddProxies();
            services.AddSignalR();

            services.AddHealthChecks()
                .AddSqlServer(Configuration.GetConnectionString("DefaultConnection"), tags: new[] { "startup" });

            services.AddCors();
            var mvcBuilder = services.AddControllersWithViews();

            services.AddAuthorization(options =>
            {
                options.AddPolicies();
            });

            services.AddAuthorizationHandlers();

            services.AddTransient<IAuthorizationEvaluator, AuthorizationEvaluator>();

            if (Env.IsDevelopment())
            {
                mvcBuilder.AddRazorRuntimeCompilation();
                services
                    .AddMiniProfiler()
                    .AddEntityFramework();
            }

            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});

            services.WithPerTenantOptions<SecurityStampValidatorOptions>((options, tenant) =>
            {
                if (tenant.IsAdmin)
                {
                    options.ValidationInterval = TimeSpan.FromSeconds(30);
                }
            });

            services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
            services.AddAWSService<IAmazonS3>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IRecurringJobManager recurringJobManager, IServiceProvider serviceProvider)
        {

            if (env.EnvironmentName == Environments.Development)
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();

                // REMOVED IN .net core 3.1
                //app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                //{
                //    HotModuleReplacement = true,
                //    ConfigFile = Path.Combine(env.ContentRootPath, @"clientapp\node_modules\@vue\cli-service\webpack.config.js"),
                //    ProjectPath = Path.Combine(env.ContentRootPath, @"ClientApp")
                //});

                //TODO: fix automapper configs. it fails on A LOT of models...
                //mapper.ConfigurationProvider.AssertConfigurationIsValid();

                //Running CSP headers in DEV environment
                app.UseSecurityHeadersMiddleware(
                   new SecurityHeadersBuilder()
                       .AddContentSecurityPolicy());

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

                app.UseSecurityHeadersMiddleware(
                    new SecurityHeadersBuilder()
                        .AddDefaultSecurePolicy());

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

            app.Map("/health", healthApp =>
            {
                healthApp.UseRouting();
                healthApp.UseEndpoints(endpoints =>
                {
                    var endpointConfig = Configuration.GetSection("HealthCheck");
                    endpoints.MapHealthChecks(endpointConfig["Startup"]);
                    endpoints.MapHealthChecks(endpointConfig["Live"], new HealthCheckOptions { Predicate = _ => false });
                    endpoints.MapHealthChecks(endpointConfig["Readiness"], new HealthCheckOptions
                    {
                        Predicate = (hcr) =>
                        {
                            return false;
                        }
                    });
                });
            });



            //app.Use(async (context, next) =>
            //{
            //    if (context.Request.Path.StartsWithSegments("/robots.txt"))
            //    {
            //        var robotsTxtPath = Path.Combine(env.ContentRootPath, "robots.txt");
            //        var environmentRobotsTxt = Path.Combine(env.ContentRootPath, $"robots.{env.EnvironmentName}.txt");
            //        string output = "User-agent: *  \nDisallow: /";

            //        if (File.Exists(environmentRobotsTxt))
            //        {
            //            output = await File.ReadAllTextAsync(environmentRobotsTxt);
            //        }
            //        else if (File.Exists(robotsTxtPath))
            //        {
            //            output = await File.ReadAllTextAsync(robotsTxtPath);
            //        }
            //        context.Response.ContentType = "text/plain";
            //        await context.Response.WriteAsync(output);
            //    }
            //    else await next();
            //});

            app.Use(async (contextMRF, next) =>
            {
                if (contextMRF.Request.Path.StartsWithSegments("/robots.txt"))
                {
                    var robotsTxtPath = Path.Combine(env.ContentRootPath, "robots.txt");
                    var environmentRobotsTxt = Path.Combine(env.ContentRootPath, $"robots.{env.EnvironmentName}.txt");
                    string output = "User-agent: *  \nDisallow: /";

                    if (File.Exists(environmentRobotsTxt))
                    {
                        output = await File.ReadAllTextAsync(environmentRobotsTxt);
                    }
                    else if (File.Exists(robotsTxtPath))
                    {
                        output = await File.ReadAllTextAsync(robotsTxtPath);
                    }
                    var finaloutput = HtmlEncoder.Default.Encode(output); //GOR-371
                    contextMRF.Response.ContentType = "text/plain";
                    await contextMRF.Response.WriteAsync(finaloutput); //GOR-371
                }
                else await next();
            });

            
            //app.UseHttpsRedirection();
            app.UseCookiePolicy();
            //app.UseResponseCaching();



            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = responseContext =>
                {
                    const int durationInSeconds = 60 * 60 * 24; // 24 hours
                    responseContext.Context.Response.Headers[HeaderNames.CacheControl] = "public,max-age=" + durationInSeconds;
                }
            });


            app.UseMultitenancy<ProjectTenant>(Configuration);

            app.UseSerilogRequestLogging();
            app.UseRouting();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials()); // allow credentials

            app.UseAuthentication();
            app.UseAuthorization();



            if (env.EnvironmentName == Environments.Development)
            {
                app.UseMiniProfiler();
            }

            if (env.EnvironmentName == Environments.Development)
            {
                app.Map(
                    "/client-app",
                    ctx => ctx.UseSpa(spa =>
                    {
                        spa.Options.SourcePath = "ClientApp";
                        spa.UseProxyToSpaDevelopmentServer("http://localhost:8081/");
                    })
                );
            }

            app.UseHangfireDashboard("/tasks-hangfire-dashboard", new DashboardOptions()
            {
                Authorization = new[] { new HangfireAuthorizationFilter() },
                IgnoreAntiforgeryToken = true
            });

            app.BuildRoutes();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<QuestionsHub>("/questionshub", configuration =>
                {

                });
            });

            var tpnReportConfig = Configuration.GetSection("TPNReportsJobSchedule");
            recurringJobManager.AddOrUpdate(tpnReportConfig["CronStringDescription"], () => serviceProvider.GetService<GoRegister.ApplicationCore.Domain.TPNReports.ITPNReportService>().GenrateAutoTPNReport(), tpnReportConfig["CronString"]);
        }
    }

    //public class DesignTimeBMDbContext : IDesignTimeDbContextFactory<ApplicationDbContext>
    //{
    //    //public IConfiguration Configuration { get; }
    //    public ApplicationDbContext CreateDbContext(string[] args)
    //    {
    //        //var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
    //        //// pass your design time connection string here
    //        //optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
    //        //return new ApplicationDbContext(optionsBuilder.Options);

    //        IConfiguration configuration = new ConfigurationBuilder()
    //            .SetBasePath(Directory.GetCurrentDirectory())
    //            .AddJsonFile("appsettings.json")
    //            .Build();

    //        var builder = new DbContextOptionsBuilder<ApplicationDbContext>();

    //        var connectionString = configuration.GetConnectionString("DefaultConnection");

    //        builder.UseSqlServer(connectionString);

    //        return new ApplicationDbContext(builder.Options);
    //    }
    //}
}
