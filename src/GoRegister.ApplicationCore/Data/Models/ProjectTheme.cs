using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoRegister.ApplicationCore.Data.Models
{
    public class ProjectTheme
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public Guid ThemeGuid { get; set; }
        public int? ProjectId { get; set; }
        public int? ClientId { get; set; }
        public DateTime DateUpdated { get; set; }
        public string LayoutName { get; set; }
        public string ThemeVariables { get; set; }
        public string ThemeVariableObject { get; set; }
        public string ThemeCss { get; set; }
        public string OverrideCss { get; set; }
        public string HeaderHtml { get; set; }
        public string FooterHtml { get; set; }
        public string HeadScripts { get; set; }
        public string FooterScripts { get; set; }        
        public string HomepageHtml { get; set; }
        public bool IsArchived { get; set; }
        public DateTime? DateArchived { get; set; }

        public Project Project { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ThemeUniqueId { get; set; }
        public string LogoUrl { get; set; }
        public ICollection<ThemeFont> Fonts { get; set; } = new HashSet<ThemeFont>();

    }
}
