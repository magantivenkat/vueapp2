using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Domain.Projects.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoRegister.ApplicationCore.Data.Models
{
    public class Project
    {
        public int Id { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UniqueId { get; set; }
        public string Name { get; set; }
        public string Host { get; set; }
        public string Prefix { get; set; }
        public string PageTitleTag { get; set; }
        public string MetaDescription { get; set; }
        public string Code { get; set; }
        public bool BlockSearchEngineIndexing { get; set; }
        public bool IsSitewidePasswordEnabled { get; set; }
        public string SitewidePasswordPlainText { get; set; }
        public string SitewidePasswordHashed { get; set; }
        public bool AllowDuplicateEmails { get; set; }
        public string Timezone { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime ArchiveDate { get; set; }
        public DateTime DeleteDataDate { get; set; }
        public string ExternalReference { get; set; }
        public string EmailAddress { get; set; }
        public string EmailDisplayFrom { get; set; }
        public string EmailReplyTo { get; set; }
        public string ProjectResourceId { get; set; }
        public ProjectStatus StatusId { get; set; }
        public bool AllowAnonymousAccess { get; set; }
        public ProjectTypeEnum ProjectType { get; set; }

        public virtual ICollection<ProjectTheme> ProjectThemes { get; set; } = new HashSet<ProjectTheme>();
        public ICollection<CustomPage> CustomPages { get; set; } = new HashSet<CustomPage>();
        public ICollection<Form> Forms { get; set; } = new HashSet<Form>();
        public ICollection<RegistrationType> RegistrationTypes { get; set; } = new HashSet<RegistrationType>();
        public ICollection<RegistrationPath> RegistrationPaths { get; set; } = new HashSet<RegistrationPath>();
        public ICollection<SessionCategory> SessionCategories { get; set; } = new HashSet<SessionCategory>();
        public ICollection<RecentProject> RecentProjects { get; set; } = new HashSet<RecentProject>();
        public ICollection<MenuItem> MenuItems { get; set; } = new HashSet<MenuItem>();
        public ICollection<UserProjectMapping> UserProjectMapping { get; set; } = new HashSet<UserProjectMapping>();
        public Client Client { get; set; }
        public List<Session> Sessions { get; set; }
        public int? CreatedByUserId { get; set; }        
        public ApplicationUser CreatedByUser { get; set; }
        public bool IsActive { get; set; }
    }

    public class ProjectMap : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder
                .HasMany(e => e.RegistrationPaths)
                .WithOne(e => e.Project)
                .IsRequired();

            builder
                .HasMany(e => e.RegistrationTypes)
                .WithOne(e => e.Project)
                .IsRequired();

            builder
                .HasOne(e => e.CreatedByUser)
                .WithMany(e => e.CreatedProjects)
                .IsRequired()
                .HasForeignKey(e => e.CreatedByUserId);

        }
    }
}
