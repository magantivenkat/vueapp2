using GoRegister.ApplicationCore.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoRegister.ApplicationCore.Data.Models
{
    public class MenuItem : MustHaveProjectEntity
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public MenuItemType MenuItemType { get; set; }
        public int Order { get; set; }
        public bool OpenInNewTab { get; set; }
        public string CssClass { get; set; }
        public string AnchorLink { get; set; }
        public int? CustomPageId { get; set; }
        public CustomPage CustomPage { get; set; }
        public FormType? FormType { get; set; }
        public int? FormId { get; set; }
        public Form Form { get; set; }
        public string Fragment { get; set; }
        public ICollection<MenuItemRegistrationType> MenuItemRegistrationTypes { get; set; } = new HashSet<MenuItemRegistrationType>();
    }
}
