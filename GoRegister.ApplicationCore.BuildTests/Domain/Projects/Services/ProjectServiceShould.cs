using AutoMapper;
using FluentAssertions;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Clients;
using GoRegister.ApplicationCore.Domain.Projects.Enums;
using GoRegister.ApplicationCore.Domain.Projects.Models;
using GoRegister.ApplicationCore.Domain.Projects.Services;
using GoRegister.ApplicationCore.Domain.ProjectThemes.Models;
using GoRegister.ApplicationCore.Domain.ProjectThemes.Services;
using GoRegister.TestingCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GoRegister.ApplicationCore.BuildTests.Domain.Projects.Services
{
    public class ProjectServiceShould : DatabaseContextTest
    {
        private readonly Mock<IConfiguration> mockConfiguration = new Mock<IConfiguration>();

        private readonly int CLIENT_ID = 1;
        private readonly int RECENT_PROJECT_EXISTS_ID = 12;
        private readonly int USER_ID = 98;

        public ProjectServiceShould()
        {
            using (var db = GetDatabase())
            {
                // create an existing project to database
                var project = new RecentProject { Project = new Project { Id = RECENT_PROJECT_EXISTS_ID }, User = new ApplicationUser { Id = USER_ID } };
                db.RecentProjects.Add(project);

                var client = new Client { Id = CLIENT_ID, Name = "Test Client" };
                db.Clients.Add(client);
                db.SaveChanges();
            }

            //Configure mapping just for this test
            var config = new MapperConfiguration(cfg =>
            {
            });
        }

        [Fact]
        public async Task CreateUrlPathProject() { }

        [Fact]
        public async Task CreateSubDomainProject() { }

        [Fact]
        public async Task GenerateProjectCode_ShouldBe6CharaterString()
        {
            using (var db = GetDatabase())
            {
                var sut = new ProjectService(db, mockConfiguration.Object);
                var result = await sut.GenerateProjectCodeAsync();
                result.Should().BeOfType<string>();
                result.Length.Should().Be(6);
            }
        }


        #region AddProjectToRecentListShould

        [Fact]
        public async Task AddProjectToRecent()
        {
            using (var db = GetDatabase())
            {
                var sut = new ProjectService(db, mockConfiguration.Object);
                await sut.AddProjectToRecentList(12, 21);
            }

            // get database result
            using (var db = GetDatabase())
            {
                var result = await db.RecentProjects
                    .Include(p => p.Project)
                    .LastOrDefaultAsync();
                result.Project.Id.Should().Be(12);
            }
        }

        [Fact]
        public async Task DoNotAddProjectToRecent_IfExists()
        {
            using (var db = GetDatabase())
            {
                // try adding same project again
                var sut = new ProjectService(db, mockConfiguration.Object);
                await sut.AddProjectToRecentList(RECENT_PROJECT_EXISTS_ID, USER_ID);
            }

            using (var db = GetDatabase())
            {
                var result = await db.RecentProjects
                    .Include(p => p.Project).CountAsync(p => p.Project.Id == RECENT_PROJECT_EXISTS_ID);
                result.Should().Be(1);
            }
        }

        [Fact]
        public async Task RemoveOldestProjectFromList_IfCountOver5()
        {
            var projectToAdd = new Project { Id = 9823 };
            using (var db = GetDatabase())
            {
                // add another 4 unique projects to users recent projects list
                var user = await db.Users.SingleOrDefaultAsync(u => u.Id == USER_ID);
                var projects = new List<RecentProject> {
                    new RecentProject{Project = new Project{ Id = 9004 }, User = user },
                    new RecentProject{Project = new Project{ Id = 8005 }, User = user },
                    new RecentProject{Project = new Project{ Id = 2006 }, User = user },
                    new RecentProject{Project = new Project{ Id = 4007 }, User = user },
                };

                // project to add
                db.Projects.Add(projectToAdd);
                db.RecentProjects.AddRange(projects);
                db.SaveChanges();
            }

            using (var db = GetDatabase())
            {
                // add new project again
                var sut = new ProjectService(db, mockConfiguration.Object);
                await sut.AddProjectToRecentList(projectToAdd.Id, USER_ID);
            }

            using (var db = GetDatabase())
            {
                var result = await db.RecentProjects
                    .CountAsync(p => p.User.Id == USER_ID);
                result.Should().Be(5);
            }
        }

        #endregion
    }
}
