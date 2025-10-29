using GoRegister.ApplicationCore.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoRegister.ApplicationCore.Data.Models
{
    public class ThemeFont : MustHaveProjectEntity
    {
        public int Id { get; set; }
        public string Link { get; set; }
        public FontType FontType { get; set; }
        public string Name { get; set; }
        public string Variants { get; set; }
        public int ProjectThemeId { get; set; }
        public ProjectTheme ProjectTheme { get; set; }
    }
}
