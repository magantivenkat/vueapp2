/* MRF Changes: Add new class to save MRD User Response
Modified Date: 18th October 2022
Modified By: Mandar.Khade@amexgbt.com
Team member: Harish.Rane@amexgbt.com
JIRA Ticket No: GoRegister / GOR - 238 - New */



using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Data.Extensions;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Data.Models.Emails;
using GoRegister.ApplicationCore.Data.Models.Fields;
using GoRegister.ApplicationCore.Framework.Multitenancy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Data
{

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, UserRole, int>
    {
        private readonly ITenantAccessor _tenantAccessor;
        private static List<Action<DbContext, int>> _setTenantIdOnSaveCallbacks = new List<Action<DbContext, int>>();

        public ApplicationDbContext(ITenantAccessor tenantAccessor, DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            _tenantAccessor = tenantAccessor;
        }

        //public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        //{
        //    this.options = options;
        //}

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public StoredProcedures StoredProcedures => new StoredProcedures(this);

        public DbSet<Country> Countries { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<RecentProject> RecentProjects { get; set; }
        public DbSet<Form> Forms { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<SessionField> SessionFields { get; set; }
        public DbSet<FieldOption> FieldOptions { get; set; }
        public DbSet<FieldOptionRule> FieldOptionRules { get; set; }
        public DbSet<ProjectPage> ProjectPages { get; set; }
        public DbSet<RegistrationPage> RegistrationPages { get; set; }
        public DbSet<RegistrationPageRegistrationType> RegistrationPageRegistrationTypes { get; set; }
        public DbSet<RegistrationPath> RegistrationPaths { get; set; }
        public DbSet<RegistrationType> RegistrationTypes { get; set; }
        public DbSet<RegistrationTypeField> RegistrationTypeFields { get; set; }
        public DbSet<UserFieldResponse> UserFieldResponses { get; set; }
        public DbSet<CustomPage> CustomPages { get; set; }
        public DbSet<CustomPageVersion> CustomPageVersions { get; set; }
        public DbSet<CustomPageAudit> CustomPageAudits { get; set; }
        public DbSet<CustomPageAuditRegistrationType> CustomPageAuditRegistrationTypes { get; set; }
        public DbSet<DelegateUser> Delegates { get; set; }
        public DbSet<DelegateUserAudit> DelegateAudits { get; set; }
        public DbSet<DelegateSessionBooking> DelegateSessionBookings { get; set; }
        public DbSet<AnonSessionBooking> AnonSessionBookings { get; set; }
        public DbSet<UserFieldResponseAudit> UserFieldResponseAudits { get; set; }
        public DbSet<Models.RegistrationStatus> RegistrationStatuses { get; set; }
        public DbSet<ProjectTheme> ProjectThemes { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientEmail> ClientEmails { get; set; }
        public DbSet<TenantUrl> TenantUrls { get; set; }
        public DbSet<EmailAudit> EmailAudits { get; set; }
        public DbSet<EmailAuditNotification> EmailAuditNotifications { get; set; }
        public DbSet<EmailAuditBatch> EmailAuditBatches { get; set; }
        public DbSet<EmailTemplate> EmailTemplates { get; set; }
        public DbSet<EmailLayout> EmailLayouts { get; set; }
        public DbSet<Email> Emails { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<SessionCategory> SessionCategories { get; set; }
        public DbSet<SessionFieldCategory> SessionFieldCategories { get; set; }
        public DbSet<UserFormResponse> UserFormResponses { get; set; }
        public DbSet<DataQuery> DataQueries { get; set; }
        // menus
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<MenuItemRegistrationType> MenuItemRegistrationTypes { get; set; }
        public DbSet<UserAction> UserActions { get; set; }
        public DbSet<ThemeFont> ThemeFonts { get; set; }

        public DbSet<UserProjectMapping> UserProjectMappings { get; set; }
        public DbSet<BulkUpload> BulkUploads { get; set; }

        public DbSet<MRFClientRequest> MRFClientRequest { get; set; }
        public DbSet<MRFClientRequestCountry> MRFClientRequestCountry { get; set; }

        public DbSet<MRFClientResponseDetails> MRFClientResponseDetails { get; set; }

        public DbSet<TPNCountryClientEmail> TPNCountryClientEmails { get; set; }

        public DbSet<TPNReportRequest> TPNReportRequest { get; set; }
        public DbSet<TPNReportDetails> TPNReportDetails { get; set; }
        public DbSet<TPNCountryAdminMapping> TPNCountryAdminMapping { get; set; }
        public DbSet<TPNReportDataStatus> TPNReportDataStatus { get; set; }
        public DbSet<MRFSysConfig> MRFSysConfig { get; set; }

        public DbSet<MRFServiceCountryMapping> MRFServiceCountryMapping { get; set; }

        public DbSet<MRFApprovalRuleDetails> MRFApprovalRuleDetails { get; set; }
        public DbSet<MRFApprovalActionDetails> MRFApprovalActionDetails { get; set; }

        public DbSet<Language> Language { get; set; }
        public DbSet<MRFLanguage> MRFLanguage { get; set; }
        public DbSet<MRFLanguageResource> MRFLanguageResource { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            Action<DbContext, int> action = (db, tenantId) =>
            {
                SetTenantIdProperty("ProjectId", tenantId, db);
            };
            _setTenantIdOnSaveCallbacks.Add(action);

            base.OnModelCreating(builder);
            builder.Seed();

            builder.RemovePluralizingTableNameConvention();

            builder.Entity<ApplicationUser>().ToTable("User");
            builder.Entity<UserRole>().ToTable("Role");
            builder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaim");
            builder.Entity<IdentityUserClaim<int>>().ToTable("UserClaim");
            builder.Entity<IdentityUserLogin<int>>().ToTable("UserLogin");
            builder.Entity<IdentityUserRole<int>>().ToTable("UserRole");
            builder.Entity<IdentityUserToken<int>>().ToTable("UserToken");

            builder.Entity<Field>()
                .HasDiscriminator(e => e.FieldTypeId)
                .HasValue<TextField>(FieldTypeEnum.Textbox)
                .HasValue<TelephoneField>(FieldTypeEnum.PhoneNumber)
                .HasValue<DateField>(FieldTypeEnum.Date)
                .HasValue<SingleSelectField>(FieldTypeEnum.RadioButton)
                .HasValue<FirstNameField>(FieldTypeEnum.FirstName)
                .HasValue<LastNameField>(FieldTypeEnum.LastName)
                .HasValue<EmailField>(FieldTypeEnum.Email)
                .HasValue<HorizontalRuleField>(FieldTypeEnum.HorizontalRule)
                .HasValue<HeaderField>(FieldTypeEnum.Header)
                .HasValue<SubHeaderField>(FieldTypeEnum.SubHeader)
                .HasValue<HtmlField>(FieldTypeEnum.Html)
                .HasValue<CountryField>(FieldTypeEnum.Country)
                .HasValue<SessionField>(FieldTypeEnum.Session)
                .HasValue<MRFDestinationField>(FieldTypeEnum.MRFDestination)
                .HasValue<MRFServicingCountryField>(FieldTypeEnum.MRFServicingCountry)
                .HasValue<MRFRequestorCountryField>(FieldTypeEnum.MRFRequestorCountry)
                .HasValue<TextAreaField>(FieldTypeEnum.TextSquare);



            builder.Entity<ApplicationUser>(b =>
            {
                var index = b.HasIndex(u => new { u.NormalizedUserName }).Metadata;
                b.Metadata.RemoveIndex(index.Properties);
                b.HasIndex("NormalizedUserName", "ProjectId").HasName("UserNameIndex").IsUnique();
            });

            builder.Entity<ProjectPage>()
                .Property(c => c.Type)
                .HasConversion<int>();

            builder.Entity<Models.RegistrationStatus>()
                .Property(c => c.Id)
                .ValueGeneratedNever();

            builder.Entity<CustomPageRegistrationType>()
                .HasKey(c => new { c.CustomPageId, c.RegistrationTypeId });

            builder.Entity<CustomPageRegistrationType>()
                .HasOne(cr => cr.CustomPage)
                .WithMany(cr => cr.CustomPageRegistrationTypes)
                .HasForeignKey(x => x.CustomPageId);
            builder.Entity<CustomPageRegistrationType>()
                .HasOne(cr => cr.RegistrationType)
                .WithMany(cr => cr.CustomPageRegistrationTypes)
                .HasForeignKey(x => x.RegistrationTypeId);

            builder.Entity<CustomPageRegistrationStatus>()
                .HasKey(c => new { c.CustomPageId, c.RegistrationStatusId });

            builder.Entity<CustomPageRegistrationStatus>()
                .HasOne(cr => cr.CustomPage)
                .WithMany(cr => cr.CustomPageRegistrationStatuses)
                .HasForeignKey(x => x.CustomPageId);
            builder.Entity<CustomPageRegistrationStatus>()
                .HasOne(cr => cr.RegistrationStatus)
                .WithMany(cr => cr.CustomPageRegistrationStatuses)
                .HasForeignKey(x => x.RegistrationStatusId);           

            builder.Entity<UserProjectMapping>().HasKey(upm => new { upm.ProjectId, upm.UserId });


            builder.ApplyGoRegisterConfiguration();            

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                SetEntityPropertiesMethod.MakeGenericMethod(entityType.ClrType).Invoke(this, new object[] { builder });
            }

            foreach (var entityType in builder.Model.GetEntityTypes().Where(t => typeof(IMustHaveProject).IsAssignableFrom(t.ClrType)))
            {
                entityType.FindNavigation("Project").ForeignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }

            builder.Entity<Project>()
               .Property(p => p.IsActive)
               .HasDefaultValue(true);
        }

        public T DetachEntity<T>(T entity) where T : class
        {
            if (entity == null) return entity;

            Entry(entity).State = EntityState.Detached;
            var idProperty = entity.GetType().GetProperty("Id");
            if (idProperty != null)
            {
                if (idProperty.PropertyType == typeof(int))
                    idProperty.SetValue(entity, 0);
                else if (idProperty.PropertyType == typeof(Guid))
                    idProperty.SetValue(entity, Guid.NewGuid());
            }
            return entity;
        }

        public IEnumerable<T> DetachEntities<T>(IEnumerable<T> entities) where T : class
        {
            if (entities == null) return entities;

            foreach (var entity in entities)
            {
                DetachEntity(entity);
            }
            return entities;
        }

        static readonly MethodInfo SetEntityPropertiesMethod = typeof(ApplicationDbContext).GetMethod(nameof(SetEntityProperties), BindingFlags.Instance | BindingFlags.NonPublic);

        private void SetEntityProperties<T>(ModelBuilder builder) where T : class
        {
            if (typeof(IMustHaveProject).IsAssignableFrom(typeof(T)) && !typeof(T).IsSubclassOf(typeof(Field)))
            {
                builder
                    .Entity<T>()
                    .HasQueryFilter(b => EF.Property<int>(b, "ProjectId") == _tenantAccessor.Get.Id);
            }

            if (typeof(IMayHaveProject).IsAssignableFrom(typeof(T)))
            {
                builder
                    .Entity<T>()
                    .HasQueryFilter(b => EF.Property<int?>(b, "ProjectId") == _tenantAccessor.Get.Id || EF.Property<int?>(b, "ProjectId") == null); //TODO: this needs to change to null
            }

            if (typeof(ISoftDelete).IsAssignableFrom(typeof(T)))
            {
                builder
                    .Entity<T>()
                    .HasQueryFilter(b => !EF.Property<bool>(b, "IsDeleted"));
            }
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            //OnBeforeSaveChanges();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override int SaveChanges()
        {
            OnBeforeSaveChanges();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            OnBeforeSaveChanges();
            return await base.SaveChangesAsync(cancellationToken);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            OnBeforeSaveChanges();
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private static void SetTenantIdProperty(string propertyName, int id, DbContext db)
        {
            foreach (var item in db.ChangeTracker.Entries())
            {
                if (item.Entity is IMustHaveProject && item.State == EntityState.Added && id != 0) //TODO: this will break creating users in the admin project
                {
                    item.Property(propertyName).CurrentValue = id;
                }

                if (item.Entity is IMayHaveProject && item.State == EntityState.Added && id != 0) //TODO: this will break creating users in the admin project
                {
                    item.Property(propertyName).CurrentValue = id;
                }

                if (item.Entity is ISoftDelete && item.State == EntityState.Deleted)
                {
                    item.State = EntityState.Modified;
                    item.CurrentValues["IsDeleted"] = true;
                }
            }
        }

        private void SetTenantIdOnSave()
        {
            ChangeTracker.DetectChanges();
            foreach (var item in _setTenantIdOnSaveCallbacks)
            {
                item(this, _tenantAccessor.Get.Id);
            }
        }

        private void OnBeforeSaveChanges()
        {
            SetTenantIdOnSave();
        }

        public DbContext Instance => this;

    }
}
