/*  MRF Changes : Added ClientList and DomainList varriable
    Modified Date : 16th September 2022
    Modified By : Mandar.Khade @amexgbt.com
    Team member : Harish.Rame @amexgbt.com
    JIRA Ticket No : GoRegister/GOR-213 

    MRF Changes : Add validation on client dropdown for add template page
    Modified Date :  01st November 2022
    Modified By : Mandar.Khade @amexgbt.com
    Team member : Harish.Rame @amexgbt.com
    JIRA Ticket No : GoRegister/GOR-228
 */

using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Framework.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using GoRegister.ApplicationCore.Domain.Projects.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using GoRegister.ApplicationCore.Framework.Domain.Mediatr;
using GoRegister.ApplicationCore.Domain.Projects.Models;
using GoRegister.ApplicationCore.Data.Enums;

namespace GoRegister.ApplicationCore.Domain.Projects.Commands.CreateProject
{
    public class CreateProjectCommand : ValidatableViewModelRequest<Result<Project>>
    {
        
        [DisplayName("MRF name"), Required]
        public string Name { get; set; }

        public string Timezone { get; set; } = "GMT Standard Time";
        public ProjectTypeEnum ProjectType { get; set; } = ProjectTypeEnum.Project;

        [RegularExpression(@"^[a-zA-Z0-9,-_.]+$", ErrorMessage = "Only alphanumeric characters, dash, underscore and dot are allowed.")]
        public string Subdomain { get; set; }

        [DisplayName("Subdomain Host")]
        public int? SubdomainHost { get; set; }

        [DisplayName("Url Path Host")]
        public int UrlPathHost { get; set; }

        [DisplayName("Path Prefix")]
        [RegularExpression(@"^[a-zA-Z0-9,-_]+$", ErrorMessage = "Only alphanumeric characters, dash and underscore are allowed.")]
        public string PathPrefix { get; set; }

        [DisplayName("Start date")]
        public DateTime StartDate { get; set; }

        [DisplayName("End date")]
        public DateTime EndDate { get; set; }

        [DisplayName("Archive date")]
        public DateTime ArchiveDate { get; set; }

        [DisplayName("Delete data date")]
        public DateTime DeleteDataDate { get; set; }

        [DisplayName("External Reference")]
        public string ExternalReference { get; set; }

        [DisplayName("Client Email address"), Required]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
         @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + 
         @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Enter valid email address.")]
        public string EmailAddress { get; set; }

        [DisplayName("Email display name"), Required]
        public string EmailDisplayFrom { get; set; }

        [DisplayName("Reply to email address")]
        public string EmailReplyTo { get; set; }

        public string ProjectResourceId { get; set; }
        public int ProjectTheme { get; set; }
        public ProjectUrlFormat ProjectUrlFormat { get; set; } = ProjectUrlFormat.UrlPath;

        public List<SelectListItem> ProjectThemes { get; set; }
        public IEnumerable<SelectListItem> SubdomainHosts { get; set; }
        public IEnumerable<SelectListItem> UrlPathHosts { get; set; }
        public List<SelectListItem> TimeZones { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> ProjectTemplates { get; set; } = new List<SelectListItem>();

        [Display(Name = "Client"), Required]
        public int? ClientId { get; set; }
        public List<SelectListItem> ClientList { get; set; }

        public string EmailType { get; set; }

        [DisplayName("Custom Email address")]
        [RegularExpression(@"^[a-zA-Z0-9,-.]+$", ErrorMessage = "Only alphanumeric characters, dash and dot are allowed.")]
        public string CustomEmailAddress { get; set; }
        public CloneProjectModel CloneModel { get; set; } = new CloneProjectModel();

        public int? CloneProjectId { get; set; }
        public int? TemplateId { get; set; }

        public bool IsCloning => CloneProjectId.HasValue;
        public bool IsTemplate => ProjectType == ProjectTypeEnum.Template;
        public string CloneProjectName { get; set; }

        public List<SelectListItem> DomainsListMRF { get; set; }

        public List<SelectListItem> ClientListMRF { get; set; }

    }
}
