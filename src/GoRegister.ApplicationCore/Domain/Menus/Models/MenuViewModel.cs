using GoRegister.ApplicationCore.Data.Enums;
using System.Collections.Generic;

namespace GoRegister.ApplicationCore.Domain.Menus.Models
{
    public class MenuViewModel
    {
        public IEnumerable<MenuItemListModel> Items { get; set; }
    }

    public class MenuItemListModel
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public MenuItemType MenuItemType { get; set; }
        public string Type { get; set; }
        public string CustomPageLabel { get; set; }
    }
}
