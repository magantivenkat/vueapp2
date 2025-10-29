using FluentAssertions;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Projects.Queries;
using GoRegister.TestingCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using GoRegister.ApplicationCore.Domain.Projects.Models;

namespace GoRegister.ApplicationCore.BuildTests.Domain.Projects
{
    public class ProjectListQueryShould : DatabaseContextTest
    {
        private readonly int ADMIN_USER_ID = 100;

        public ProjectListQueryShould()
        {
            using (var db = GetDatabase())
            {
                var user = new ApplicationUser { Id = ADMIN_USER_ID };
                var recentProjects = new List<RecentProject> {
                    new RecentProject { Project = new Project { }, User = user },
                    new RecentProject { Project = new Project { }, User = user },
                    new RecentProject { Project = new Project { }, User = user },
                    new RecentProject { Project = new Project { }, User = user },
                    new RecentProject { Project = new Project { }, User = user },
                    new RecentProject { Project = new Project { }, User = user },
                };

                db.Users.Add(user);
                db.RecentProjects.AddRange(recentProjects);
                db.SaveChanges();

            }
        }

        [Fact]
        public async Task ReturnListProjectModel()
        {
            using (var db = GetDatabase())
            {
                var sut = new ProjectListQuery.QueryHandler(db);
                var result = await sut.Handle(new ProjectListQuery.Query(), new CancellationToken());
                result.Should().BeOfType<Response>();
            }
        }

        [Fact]
        public async Task OnlyReturn_MaximumFive_RecentProjects()
        {
            using (var db = GetDatabase())
            {
                var sut = new ProjectListQuery.QueryHandler(db);

                var result = await sut.Handle(new ProjectListQuery.Query { UserId = ADMIN_USER_ID }, new CancellationToken());
                result.RecentProjects.Count.Should().Be(5);
            }
        }

    }
}
