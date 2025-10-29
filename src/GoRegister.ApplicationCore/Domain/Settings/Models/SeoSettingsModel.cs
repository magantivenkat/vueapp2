using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Settings.Models
{
    public class SeoSettingsModel
    {
        public string PageTitleTag { get; set; }
        public string MetaDescription { get; set; }
        public bool BlockSearchEngineIndexing { get; set; }
    }
}
