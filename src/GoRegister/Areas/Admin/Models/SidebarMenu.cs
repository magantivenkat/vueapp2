using System;
using System.Collections.Generic;

namespace AdminLTE.Models
{
    public class SidebarMenu
    {
        public SidebarMenu() { }

        public SidebarMenu(string name, string url, string faIcon)
        {
            Name = name;
            URLPath = url;
            faIcon = "fa fa-" + faIcon;

            Type = SidebarMenuType.Link;
        }

        public SidebarMenuType Type { get; set; }
        public bool IsActive { get; set; }
        public string Name { get; set; }
        public string IconClassName { get; set; }
        public string URLPath { get; set; }
        public List<SidebarMenu> TreeChild { get; set; }
        public Tuple<int, int, int> LinkCounter { get; set; } = new Tuple<int, int, int>(0, 0, 0);
    }

    public enum SidebarMenuType
    {
        Header,
        Link,
        Tree
    }
}
