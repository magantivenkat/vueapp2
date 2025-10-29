using FluentAssertions;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Liquid.Queries;
using GoRegister.TestingCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static GoRegister.ApplicationCore.Domain.Liquid.Queries.ListDataTagsQuery;

namespace GoRegister.ApplicationCore.BuildTests.Domain.DataTags.Services
{
    public class ValidateDataTagQueryTests : DatabaseContextTest
    {
        protected int ExistingProjectId = 2;


        public ValidateDataTagQueryTests()
        {

        }

        [Fact]
        public async void Handle_Result_NotEmpty()
        {
            using (var db = GetDatabase())
            {
                var sut = new ListDataTagsQuery.QueryHandler(db);
                var query = new ListDataTagsQuery.Query();

                var result = await sut.Handle(query, new System.Threading.CancellationToken());

                result.Should().NotBeEmpty();
            }
        }

        [Fact]
        public async void Handle_Result_OfType()
        {
            using (var db = GetDatabase())
            {
                var sut = new ListDataTagsQuery.QueryHandler(db);
                var query = new ListDataTagsQuery.Query();

                var result = await sut.Handle(query, new System.Threading.CancellationToken());

                result.Should().BeOfType<List<ListDataTagsQuery.DataTagCategory>>();
            }
        }

        [Fact]
        public async void Handle_Result_Count()
        {
            using (var db = GetDatabase())
            {
                var sut = new ListDataTagsQuery.QueryHandler(db);
                var query = new ListDataTagsQuery.Query();

                var result = await sut.Handle(query, new System.Threading.CancellationToken());

                result.Should().HaveCount(3);
            }
        }

        [Fact]
        public async void Handle_Result_Has_Registration()
        {
            using (var db = GetDatabase())
            {
                var sut = new ListDataTagsQuery.QueryHandler(db);
                var query = new ListDataTagsQuery.Query();

                var result = await sut.Handle(query, new System.Threading.CancellationToken());

                result.Contains(new ListDataTagsQuery.DataTagCategory { Name = "Registration" });
            }
        }

        [Fact]
        public async void Handle_Result_Has_Sessions()
        {
            using (var db = GetDatabase())
            {
                var sut = new ListDataTagsQuery.QueryHandler(db);
                var query = new ListDataTagsQuery.Query();

                var result = await sut.Handle(query, new System.Threading.CancellationToken());

                result.Contains(new ListDataTagsQuery.DataTagCategory { Name = "Sessions" });
            }
        }

        [Fact]
        public async void Handle_Result_Has_Project()
        {
            using (var db = GetDatabase())
            {
                var sut = new ListDataTagsQuery.QueryHandler(db);
                var query = new ListDataTagsQuery.Query();

                var result = await sut.Handle(query, new System.Threading.CancellationToken());

                result.Contains(new ListDataTagsQuery.DataTagCategory { Name = "Project" });
            }
        }

        [Fact]
        public async void Handle_Result_Has_SessionsDataTags()
        {
            using (var db = GetDatabase())
            {
                var fieldsSessions = await db.SessionCategories.ToListAsync();
                var sessionsCategory = new DataTagCategory { Name = "Sessions" };

                var cat = new SessionCategory { Name = "Test" };

                db.SessionCategories.Add(cat);
                db.SaveChanges();
            }

            using (var db = GetDatabase())
            {
                var sut = new ListDataTagsQuery.QueryHandler(db);
                var query = new ListDataTagsQuery.Query();

                var result = await sut.Handle(query, new System.Threading.CancellationToken());

                var session = result.Where(f => f.Name == "Sessions").SelectMany(f => f.Tags).ToList();
                session = session.Where(s => s.DataTag.Contains("Test")).ToList();

                session.Should().HaveCount(1);
            }
        }

        [Fact]
        public async void Handle_Result_Has_ProjectsDataTags()
        {
            using (var db = GetDatabase())
            {
                var sut = new ListDataTagsQuery.QueryHandler(db);
                var query = new ListDataTagsQuery.Query();

                var result = await sut.Handle(query, new System.Threading.CancellationToken());

                var project = result.Where(f => f.Name == "Project").SelectMany(f => f.Tags).ToList();
                project = project.Where(s => s.DataTag.Contains("name")).ToList();

                project.Should().HaveCount(1);
            }
        }
    }
}
