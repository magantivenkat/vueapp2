using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Fluid;
using GoRegister.ApplicationCore.Domain.Liquid.Events;
using GoRegister.ApplicationCore.Framework.Domain;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace GoRegister.ApplicationCore.Domain.Liquid
{
    public class LiquidTemplateManager : ILiquidTemplateManager
    {
        private readonly IMemoryCache memoryCache;
        private readonly IServiceProvider serviceProvider;
        private readonly IPublisher _publisher;

        public LiquidTemplateManager(IMemoryCache memoryCache, IServiceProvider serviceProvider, IPublisher publisher)
        {
            this.memoryCache = memoryCache;
            this.serviceProvider = serviceProvider;
            _publisher = publisher;
        }

        public async Task<TemplateContext> GetGoRegisterTemplateContext()
        {
            var context = new TemplateContext();
            var creatingEvent = new CreatingGoRegisterTemplateContextEvent() { TemplateContext = context };
            await _publisher.Publish(creatingEvent);
            return context;
        }

        public async Task<string> RenderAsync(string source, TemplateContext context, TextEncoder encoder)
        {
            if (string.IsNullOrWhiteSpace(source))
                return default;

            var result = GetCachedTemplate(source);
            context.AmbientValues.TryAdd("Services", serviceProvider);

            return await result.RenderAsync(context, encoder);
        }

        private LiquidViewTemplate GetCachedTemplate(string source)
        {
            IEnumerable<string> errors;

            var result = memoryCache.GetOrCreate(
                source,
                e =>
                {
                    if (!TryParse(source, out var parsed, out errors))
                        TryParse(string.Join(Environment.NewLine, errors), out parsed, out errors);

                    e.SetSlidingExpiration(TimeSpan.FromSeconds(30));
                    return parsed;
                });
            return result;
        }

        public bool Validate(string template, out IEnumerable<string> errors) => TryParse(template, out _, out errors);
        private bool TryParse(string template, out LiquidViewTemplate result, out IEnumerable<string> errors) => LiquidViewTemplate.TryParse(template, true, out result, out errors);
    }
}