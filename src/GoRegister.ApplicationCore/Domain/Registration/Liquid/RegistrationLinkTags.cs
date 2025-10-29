using Fluid;
using Fluid.Ast;
using Fluid.Tags;
using GoRegister.ApplicationCore.Domain.Registration.Framework;
using GoRegister.ApplicationCore.Domain.Registration.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Registration.Liquid
{
    public class InviteUrlTag : SimpleTag
    {
        public override async ValueTask<Completion> WriteToAsync(TextWriter writer, TextEncoder encoder, TemplateContext context)
        {
            if (context.GetValue("user").ToObjectValue() is DelegateDataTagAccessor user)
            {

                if (!context.AmbientValues.TryGetValue("Services", out var services))
                {
                    throw new ArgumentException("Services missing");
                }

                var regLinkService = ((IServiceProvider)services).GetRequiredService<IRegistrationLinkService>();

                writer.Write(await regLinkService.GetInvitationLink(user.UniqueIdentifier));
            }

            return Completion.Normal;
        }
    }

    public class InviteLinkTag : SimpleTag
    {
        public override async ValueTask<Completion> WriteToAsync(TextWriter writer, TextEncoder encoder, TemplateContext context)
        {
            if (context.GetValue("user").ToObjectValue() is DelegateDataTagAccessor user)
            {

                if (!context.AmbientValues.TryGetValue("Services", out var services))
                {
                    throw new ArgumentException("Services missing");
                }

                var regLinkService = ((IServiceProvider)services).GetRequiredService<IRegistrationLinkService>();
                var url = await regLinkService.GetInvitationLink(user.UniqueIdentifier);
                writer.Write($"<a href='{url}' class='gr-invite-link'>Click here to register</a>");
            }

            return Completion.Normal;
        }
    }
}
