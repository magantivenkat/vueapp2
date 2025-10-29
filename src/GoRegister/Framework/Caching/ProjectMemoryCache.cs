using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace GoRegister.Framework.Caching
{
    public class ProjectMemoryCache : MemoryCache
    {
        public ProjectMemoryCache(IOptions<MemoryCacheOptions> optionsAccessor) : base(optionsAccessor)
        {
        }

        public new ICacheEntry CreateEntry(object key)
        {
            
            return base.CreateEntry(key);
        }
    }
}
