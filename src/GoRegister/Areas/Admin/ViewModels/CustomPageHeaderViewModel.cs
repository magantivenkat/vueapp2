using GoRegister.ApplicationCore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.Areas.Admin.ViewModels
{
    public class CustomPageHeaderViewModel
    {
        public IEnumerable<CustomPage> CustomPages { get; set; }

        public string Prefix { get; set; }
    }
}
