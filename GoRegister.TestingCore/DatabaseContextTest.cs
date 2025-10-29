using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Settings.Services;
using GoRegister.ApplicationCore.Framework;
using GoRegister.ApplicationCore.Framework.Multitenancy;
using GoRegister.TestingCore.Helpers;
using Moq;
using System;
using System.Collections.Generic;

namespace GoRegister.TestingCore
{
    public class DatabaseContextTest
    {
        protected const int ProjectId = 2;

        protected readonly ConnectionFactory Factory;
        protected readonly ProjectTenant ProjectTenant;
        protected readonly ProjectTenant ProjectAdminTenant;
        protected readonly ProjectTenant AdminTenant;

        protected readonly Mock<ITenantAccessor> ProjectTenantAccessor;
        protected readonly Mock<ITenantAccessor> ProjectAdminTenantAccessor;
        protected readonly Mock<ITenantAccessor> AdminTenantAccessor;

        protected readonly Mock<IProjectSettingsAccessor> ProjectSettingsAccessor;

        protected Project ProjectSettings;

        public DatabaseContextTest()
        {
            Factory = new ConnectionFactory();

            ProjectTenant = new ProjectTenant
            {
                Hostname = "localhost",
                Id = ProjectId,
                Name = "Project"
            };
            ProjectTenantAccessor = new Mock<ITenantAccessor>();
            ProjectTenantAccessor.Setup(e => e.Get).Returns(ProjectTenant);
            ProjectTenantAccessor.Setup(e => e.SetTenant(It.IsAny<ProjectTenant>()));

            ProjectAdminTenant = new ProjectTenant
            {
                Hostname = "localhost",
                Id = ProjectId,
                Name = "Project",
                IsAdmin = true
            };
            ProjectAdminTenantAccessor = new Mock<ITenantAccessor>();
            ProjectAdminTenantAccessor.Setup(e => e.Get).Returns(ProjectAdminTenant);
            ProjectAdminTenantAccessor.Setup(e => e.SetTenant(It.IsAny<ProjectTenant>()));

            AdminTenant = new ProjectTenant
            {
                Hostname = "localhost",
                Id = 0,
                Name = "Admin",
                IsAdmin = true
            };
            AdminTenantAccessor = new Mock<ITenantAccessor>();
            AdminTenantAccessor.Setup(e => e.Get).Returns(AdminTenant);
            AdminTenantAccessor.Setup(e => e.SetTenant(It.IsAny<ProjectTenant>()));

            ProjectSettings = new Project
            {
                Id = ProjectTenant.Id,
                Name = ProjectTenant.Name
            };
            ProjectSettingsAccessor = new Mock<IProjectSettingsAccessor>();
            ProjectSettingsAccessor.Setup(e => e.GetAsync()).ReturnsAsync(ProjectSettings);

            using (var db = Factory.CreateContextForInMemory(AdminTenantAccessor.Object))
            {
                db.Projects.Add(ProjectSettings);
                db.TenantUrls.AddRange(new List<TenantUrl> { 
                    new TenantUrl { Host = "localhost", IsSubdomainHost = true },
                    new TenantUrl { Host = "localhost",  IsSubdomainHost = false },
                });
                db.SaveChanges();
            }
        }

        protected ApplicationDbContext GetDatabase()
        {
            return Factory.CreateContextForInMemory(ProjectTenantAccessor.Object);
        }

        protected ApplicationDbContext GetAdminDatabase()
        {
            return Factory.CreateContextForInMemory(AdminTenantAccessor.Object);
        }

        protected void SetProjectSettings(Action<Project> updater) => updater(ProjectSettings);
    }
}
