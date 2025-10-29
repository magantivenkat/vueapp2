using GoRegister.ApplicationCore.Domain.ProjectThemes.Models;
using System.Collections.Generic;

namespace GoRegister.ApplicationCore.Domain.ProjectThemes
{
    public static class ProjectThemeConfiguration
    {
        public static readonly List<LayoutItem> Layouts = new List<LayoutItem>
        {
            new LayoutItem { Name = "Default", View = "_Layout" }
        };
    }
}
