using GoRegister.ApplicationCore.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoRegister.ApplicationCore.Domain.Menus.Models
{
    public class BuildMenuViewModel
    {
        public List<MenuLinkModel> Links { get; set; }
    }

    public class BuildMenuItemModel
    {
        public int Id { get; set; }
        public MenuItemType MenuItemType { get; set; }
        public string Label { get; set; }
        public bool OpenInNewTab { get; set; }
        public string CssClass { get; set; }
        public string Fragment { get; set; }
        public string AnchorLink { get; set; }
        public string CustomPageSlug { get; set; }
        public string CustomPageTitle { get; set; }
        public IEnumerable<int> RegistrationTypeIds { get; set; }
        public IEnumerable<RegistrationStatus> RegistrationStatuses { get; set; } = new List<RegistrationStatus>();
        public IEnumerable<int> CustomPageRegistrationTypes { get; set; }
        public IEnumerable<RegistrationStatus> CustomPageRegistrationStatuses { get; set; }
        public PageType? CustomPageType { get; set; }
    }

    public class MenuLinkModel
    {
        public string Link { get; set; }
        public bool OpenInNewTab { get; set; }
        public string CssClass { get; set; }
        public string Label { get; set; }
    }
}
