using Fluid;
using MediatR;

namespace GoRegister.ApplicationCore.Domain.Liquid.Events
{
    public class CreatingGoRegisterTemplateContextEvent : INotification
    {
        public TemplateContext TemplateContext { get; set; }
    }
}
