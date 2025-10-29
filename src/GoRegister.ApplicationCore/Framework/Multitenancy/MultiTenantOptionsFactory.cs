using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Framework.Multitenancy
{
    internal class MultiTenantOptionsFactory<TOptions> : IOptionsFactory<TOptions> where TOptions : class, new()
    {
        private readonly IEnumerable<IConfigureOptions<TOptions>> _setups;
        private readonly Action<TOptions, ProjectTenant> tenantConfig;
        private readonly ISingletonTenantAccessor _tenantContextAccessor;
        private readonly IEnumerable<IPostConfigureOptions<TOptions>> _postConfigures;

        /// <summary>
        /// Initializes a new instance with the specified options configurations.
        /// </summary>
        /// <param name="setups">The configuration actions to run.</param>
        /// <param name="postConfigures">The initialization actions to run.</param>
        public MultiTenantOptionsFactory(IEnumerable<IConfigureOptions<TOptions>> setups, IEnumerable<IPostConfigureOptions<TOptions>> postConfigures, Action<TOptions, ProjectTenant> tenantConfig, ISingletonTenantAccessor tenantContextAccessor)
        {
            _setups = setups;
            this.tenantConfig = tenantConfig;
            this._tenantContextAccessor = tenantContextAccessor;
            _postConfigures = postConfigures;
        }

        public TOptions Create(string name)
        {
            var options = new TOptions();
            foreach (var setup in _setups)
            {
                if (setup is IConfigureNamedOptions<TOptions> namedSetup)
                {
                    namedSetup.Configure(name, options);
                }
                else if (name == Options.DefaultName)
                {
                    setup.Configure(options);
                }
            }

            // Configure tenant options.
            if (_tenantContextAccessor.Get != null)
            {
                tenantConfig(options, _tenantContextAccessor.Get);
            }

            foreach (var post in _postConfigures)
            {
                post.PostConfigure(name, options);
            }
            return options;
        }
    }
}
