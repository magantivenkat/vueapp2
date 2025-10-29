using FluentAssertions;
using GoRegister.TestingCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using GoRegister.ApplicationCore.Domain.Templates.Queries;
using GoRegister.ApplicationCore.Data.Models;
using System;

namespace GoRegister.ApplicationCore.BuildTests.Domain.Templates.Queries
{
    public class GetTemplatesShould : DatabaseContextTest
    {

        public GetTemplatesShould()
        {
            using (var db = GetDatabase())
            {
                var projs = new List<Project> {
                    new Project { ProjectType = Data.Enums.ProjectTypeEnum.Project, DateCreated = new DateTime(2020,1,1), Name = "SHOULD NOT BE INCLUDED" },
                    new Project { ProjectType = Data.Enums.ProjectTypeEnum.Template, DateCreated = new DateTime(2020,1,1), Name = "Second" },
                    new Project { ProjectType = Data.Enums.ProjectTypeEnum.Template, DateCreated = new DateTime(2021,1,1), Name = "Third" },
                    new Project { ProjectType = Data.Enums.ProjectTypeEnum.Template, DateCreated = new DateTime(2019,1,1), Name = "First" },
                };
                db.Projects.AddRange(projs);
                db.SaveChanges();
            }
        }

        [Fact]
        public async Task Return_ListOfProjects()
        {
            using (var db = GetDatabase())
            {
                var sut = new GetTemplates.QueryHandler(db);
                var result = await sut.Handle(new GetTemplates.Query(), new CancellationToken());

                result.Should().BeOfType<List<GetTemplates.ProjectModel>>();
            }
        }

        [Fact]
        public async Task Returns_ProjectType_Template()
        {
            using (var db = GetDatabase())
            {
                var sut = new GetTemplates.QueryHandler(db);
                var result = await sut.Handle(new GetTemplates.Query(), new CancellationToken());

                result.Count.Should().Be(3);
            }
        }

        [Fact]
        public async Task Returns_Templates_OrderedBy_DateCreated()
        {
            using (var db = GetDatabase())
            {
                var sut = new GetTemplates.QueryHandler(db);
                var result = await sut.Handle(new GetTemplates.Query(), new CancellationToken());

                result[0].Name.Should().Be("First");
            }
        }


    }
}
