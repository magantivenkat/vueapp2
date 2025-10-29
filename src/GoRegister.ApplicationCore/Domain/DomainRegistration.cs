using GoRegister.ApplicationCore.Domain.AdminUsers;
using GoRegister.ApplicationCore.Domain.Clients;
using GoRegister.ApplicationCore.Domain.CustomPages.Services;
using GoRegister.ApplicationCore.Domain.ProjectPages.Services;
using GoRegister.ApplicationCore.Domain.Projects.Services;
using GoRegister.ApplicationCore.Domain.ProjectThemes.Services;
using GoRegister.ApplicationCore.Domain.Registration.Framework;
using GoRegister.ApplicationCore.Domain.Registration.Services;
using GoRegister.ApplicationCore.Domain.Settings.Services;
using Microsoft.Extensions.DependencyInjection;
using GoRegister.ApplicationCore.Domain.Liquid;
using GoRegister.ApplicationCore.Domain.Registration;
using GoRegister.ApplicationCore.Domain.Countries;
using GoRegister.ApplicationCore.Domain.Sessions.Services;
using GoRegister.ApplicationCore.Domain.Reports;
using GoRegister.ApplicationCore.Domain.Domains;
using GoRegister.ApplicationCore.Domain.BulkUpload.Services;
using GoRegister.ApplicationCore.Domain.Menus;
using GoRegister.ApplicationCore.Domain.Delegates;
using GoRegister.ApplicationCore.Domain.Menus.Handlers;
using GoRegister.ApplicationCore.Domain.TPNReports;
using GoRegister.ApplicationCore.Domain.ServiceCountryMapping;

namespace GoRegister.ApplicationCore.Domain
{
    public static class DomainRegistration
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddTransient<IProjectService, ProjectService>();

            // registration
            services.AddTransient<IRegistrationService, RegistrationService>();
            services.AddTransient<IFormService, FormService>();
            services.AddTransient<IRegistrationLinkService, RegistrationLinkService>();

            services.AddTransient<IFieldDriverAccessor, FieldDriverAccessor>();
            services.AddTransient<IDataTagMappingCache, DataTagMappingCache>();
            services.AddScoped<IFieldDriverAccessor, FieldDriverAccessor>();

            // delegates
            services.AddTransient<IDelegateUserCacheService, DelegateUserCacheService>();
            services.AddTransient<IRegistrationTypeService, RegistrationTypeService>();
            services.AddTransient<IDelegateService, DelegateService>();
            services.AddTransient<IAttendeeAuthorizationService, AttendeeAuthorizationService>();
            services.AddScoped<ICurrentAttendeeAccessor, CurrentAttendeeAccessor>();
            services.AddTransient<IAttendeeIdentifierService, AttendeeIdentifierService>();

            // bulk upload
            services.AddTransient<IBulkUploadService, BulkUploadService>();

            // sessions
            services.AddTransient<ISessionService, SessionService>();
            services.AddTransient<ISessionAccessor, SessionAccessor>();
            services.AddTransient<ISessionBookingService, SessionBookingService>();

            //pages
            services.AddTransient<IProjectPageService, ProjectPageService>();
            services.AddTransient<ICustomPageService, CustomPageService>();

            // clients
            services.AddTransient<IClientService, ClientService>();

            // admin
            services.AddTransient<IAdminUserService, AdminUserService>();

            // domain
            services.AddTransient<IDomainService, DomainService>();

            // project settings
            services.AddTransient<ISettingsService, SettingsService>();
            services.AddScoped<IProjectSettingsAccessor, ProjectSettingsAccessor>();
            services.AddTransient<IProjectThemeService, ProjectThemeService>();

            // Liquid
            services.AddTransient<ILiquidTemplateManager, LiquidTemplateManager>();

            // Countries
            services.AddScoped<ICountryCacheService, CountryCacheService>();

            // reports
            services.AddScoped<IReportService, ReportService>();
            services.AddReportGroupBys();

            // menus
            services.AddTransient<IMenuFactory, MenuFactory>();

            // menu handlers
            services.AddTransient<IMenuHandler, CustomPageMenuHandler>();
            services.AddTransient<IMenuHandler, LinkMenuHandler>();
            services.AddTransient<IMenuHandler, RegisterMenuHandler>();
            services.AddTransient<IMenuHandler, ViewRegistrationMenuHandler>();
            services.AddTransient<IMenuHandler, SessionsMenuHandler>();

            // user actions
            services.AddTransient<UserActions.IUserActionService, UserActions.UserActionService>();
            
            //TPNReports
            services.AddTransient<ITPNReportService, TPNReportService>();
            services.AddTransient<IServiceCountryMappingService, ServiceCountryMappingService>();

            return services;
        }
    }
}
