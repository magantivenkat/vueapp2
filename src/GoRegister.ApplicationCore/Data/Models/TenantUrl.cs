using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Data.Models
{
    public class TenantUrl
    {
        public int Id { get; set; }
        public string Host { get; set; }
        public bool IsSubdomainHost { get; set; }
        public string DisallowedSubDomains { get; set; }
        public int? ClientId { get; set; }
        public Client Client { get; set; }
        public string[] DisallowedSubdomainsArray => string.IsNullOrWhiteSpace(DisallowedSubDomains)
            ? new string[] { }
            : DisallowedSubDomains.Split(',');
    }
}
