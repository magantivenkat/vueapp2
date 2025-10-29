using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace GoRegister.ApplicationCore.Framework.Multitenancy
{
    internal class MultiTenantOptionsManager<TOptions> : IOptions<TOptions>, IOptionsSnapshot<TOptions> where TOptions : class, new()
    {
        private readonly IOptionsFactory<TOptions> _factory;
        private readonly IOptionsMonitorCache<TOptions> _cache; // Note: this is a private cache

        /// <summary>
        /// Initializes a new instance with the specified options configurations.
        /// </summary>
        /// <param name="factory">The factory to use to create options.</param>
        public MultiTenantOptionsManager(IOptionsFactory<TOptions> factory, IOptionsMonitorCache<TOptions> cache)
        {
            _factory = factory;
            _cache = cache;
        }

        public TOptions Value
        {
            get
            {
                return Get(Options.DefaultName);
            }
        }

        public virtual TOptions Get(string name)
        {
            name = name ?? Options.DefaultName;

            // Store the options in our instance cache.
            return _cache.GetOrAdd(name, () => _factory.Create(name));
        }

        public void Reset()
        {
            _cache.Clear();
        }
    }
}
