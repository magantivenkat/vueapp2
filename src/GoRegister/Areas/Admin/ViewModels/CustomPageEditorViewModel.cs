using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.Areas.Admin.ViewModels
{
    public class CustomPageEditorViewModel
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Document { get; set; }
        public int Id { get; set; }
    }
}
