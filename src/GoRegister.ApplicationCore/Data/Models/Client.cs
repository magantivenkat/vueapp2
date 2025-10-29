using System;
using System.Collections.Generic;

namespace GoRegister.ApplicationCore.Data.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }

        public string ClientUuid {get; set;}

        public List<ClientEmail> ClientEmails { get; set; }

        public List<TPNCountryClientEmail> TPNCountryClientEmails { get; set; }

        public List<TPNCountryAdminMapping> TPNCountryAdminMappings { get; set; }



    }
}
