using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Countries
{
    public class CountryModel
    {
        public string ISO { get; set; }
        public string Name { get; set; }
        public string ISO3 { get; set; }
        public int Numeric { get; set; }
    }
}
