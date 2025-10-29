using GoRegister.ApplicationCore.Domain.Reports.Framework;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoRegister.ApplicationCore.Domain.Reports
{
    public static class ReportRegistration
    {
        public static IServiceCollection AddReportGroupBys(this IServiceCollection services)
        {
            return services
                .AddTransient<IGroupBy, SingleSelectGroupBy>()
                .AddTransient<IGroupBy, RegistrationStatusGroupBy>();
        }
    }
}
