using GoRegister.ApplicationCore.Domain.Registration.Fields.Date;
using GoRegister.ApplicationCore.Domain.Registration.Fields.TelephoneNumber;
using GoRegister.ApplicationCore.Domain.Registration.Fields.Text;
using GoRegister.ApplicationCore.Domain.Registration.Framework;
using Microsoft.Extensions.DependencyInjection;
using GoRegister.ApplicationCore.Domain.Registration.Fields.SingleSelect;
using GoRegister.ApplicationCore.Domain.Registration.Fields.FirstName;
using GoRegister.ApplicationCore.Domain.Registration.Fields.LastName;
using GoRegister.ApplicationCore.Domain.Registration.Fields.Email;
using GoRegister.ApplicationCore.Domain.Registration.Fields.HorizontalRule;
using GoRegister.ApplicationCore.Domain.Registration.Fields.Header;
using GoRegister.ApplicationCore.Domain.Registration.Fields.MRFDestination;
using GoRegister.ApplicationCore.Domain.Registration.Fields.MRFServicingCountry;
using GoRegister.ApplicationCore.Domain.Registration.Fields.MRFRequestorCountry;

using GoRegister.ApplicationCore.Domain.Registration.Fields.TextArea;

namespace GoRegister.ApplicationCore.Domain.Registration.Fields
{
    public static class FormDriverRegistration
    {
        public static IServiceCollection AddFormDrivers(this IServiceCollection services)
        {
            services.AddTransient<IFormDriver, TextFieldFormDriver>();
            services.AddTransient<IFormDriver, TelephoneFieldFormDriver>();
            services.AddTransient<IFormDriver, DateFieldFormDriver>();
            services.AddTransient<IFormDriver, SingleSelectFormDriver>();
            services.AddTransient<IFormDriver, FirstNameFieldFormDriver>();
            services.AddTransient<IFormDriver, LastNameFieldFormDriver>();
            services.AddTransient<IFormDriver, EmailFieldFormDriver>();
            services.AddTransient<IFormDriver, HorizontalRuleFormDriver>();
            services.AddTransient<IFormDriver, HeaderFormDriver>();
            services.AddTransient<IFormDriver, SubHeaderFormDriver>();
            services.AddTransient<IFormDriver, HtmlFormDriver>();
            services.AddTransient<IFormDriver, Country.CountryFormDriver>();
            services.AddTransient<IFormDriver, Session.SessionFieldFormDriver>();
            services.AddTransient<IFormDriver, MRFDestinationFieldFormDriver>();
            services.AddTransient<IFormDriver, MRFServicingCountryFormDriver>();
            services.AddTransient<IFormDriver, MRFRequestorCountryFormDriver>();
            services.AddTransient<IFormDriver, TextAreaFieldFormDriver>();
            // services
            services.AddScoped<IFieldOptionCache, FieldOptionCache>();

            return services;
        }
    }
}
