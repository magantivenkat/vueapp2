using Fluid;
using GoRegister.ApplicationCore.Domain.Registration.Liquid;

namespace GoRegister.ApplicationCore.Domain.Liquid
{
    public class LiquidViewTemplate : BaseFluidTemplate<LiquidViewTemplate>
    {
        static LiquidViewTemplate()
        {
            Factory.RegisterTag<InviteUrlTag>("invite_url");
            Factory.RegisterTag<InviteLinkTag>("invite_link");
            Factory.RegisterTag<DeclineUrlTag>("decline_url");
            Factory.RegisterTag<DeclineLinkTag>("decline_link");
            Factory.RegisterTag<RegistrationSummaryTag>("registration_summary");
        }
    }
}
