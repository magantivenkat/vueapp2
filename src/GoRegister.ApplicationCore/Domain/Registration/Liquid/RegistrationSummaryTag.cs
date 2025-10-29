using Fluid;
using Fluid.Ast;
using Fluid.Tags;
using GoRegister.ApplicationCore.Domain.Registration.Framework;
using GoRegister.ApplicationCore.Domain.Registration.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Registration.Liquid
{
    public class RegistrationSummaryTag : SimpleTag
    {
        public async override ValueTask<Completion> WriteToAsync(TextWriter writer, TextEncoder encoder, TemplateContext context)
        {
            if (context.GetValue("user").ToObjectValue() is DelegateDataTagAccessor user)
            {

                if (!context.AmbientValues.TryGetValue("Services", out var services))
                {
                    throw new ArgumentException("Services missing");
                }

                var formService = ((IServiceProvider)services).GetRequiredService<IFormService>();

                var summary = await formService.BuildFormSummary(user, Data.Enums.FormType.Registration);

                writer.Write($"<table class='table table-striped'>");
                foreach(var row in summary.Fields)
                {
                    writer.Write($"<tr><td>{row.Name}</td><td>{row.Value}</td></tr>");
                }
                writer.Write($"</table>");
            }

            return Completion.Normal;
        }
    }
}
