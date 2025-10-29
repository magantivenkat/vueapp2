using GoRegister.ApplicationCore.Data.Enums;
using System.Collections.Generic;

namespace GoRegister.ApplicationCore.Data.Models
{
    public class CustomPage : IMustHaveProject
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Slug { get; set; }

        public bool IsVisible { get; set; }

        public int Position { get; set; }

        public Project Project { get; set; }

        public int ProjectPageId { get; set; }

        public ProjectPage ProjectPage { get; set; }

        public List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
        public List<CustomPageRegistrationType> CustomPageRegistrationTypes { get; set; } = new List<CustomPageRegistrationType>();

        public List<CustomPageRegistrationStatus> CustomPageRegistrationStatuses { get; set; } = new List<CustomPageRegistrationStatus>();

        public ICollection<CustomPageVersion> CustomPageVersions { get; set; } = new List<CustomPageVersion>();

        public ICollection<CustomPageAudit> CustomPageAudits { get; set; } = new List<CustomPageAudit>();
        public PageType PageType { get; set; }
    }
}
