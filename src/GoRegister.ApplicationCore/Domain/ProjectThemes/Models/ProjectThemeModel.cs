using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GoRegister.ApplicationCore.Domain.ProjectThemes.Models
{
    public class ProjectThemeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Guid ThemeGuid { get; set; }
        public int? ProjectId { get; set; }
        public DateTime DateUpdated { get; set; }
        public List<SelectListItem> LayoutOptions { get; set; }
        public string LayoutName { get; set; }
        [Display(Name="Specific To Client")]
        public int? ClientId { get; set; }
        public List<SelectListItem> ClientList { get; set; }
        public string ThemeVariables { get; set; }
        public string ThemeVariableObject { get; set; }
        public string ThemeCss { get; set; }
        public string OverrideCss { get; set; }
        public string HeaderHtml { get; set; }
        public string FooterHtml { get; set; }
        public string HomepageHtml { get; set; }
        public string HeadScripts { get; set; }
        public string FooterScripts { get; set; }
        public bool IsArchived { get; set; }
        public DateTime? DateArchived { get; set; }

        public Guid ThemeUniqueId { get; set; }
        public string LogoUrl { get; set; }
        public IFormFile File { get; set; }
    }
}
