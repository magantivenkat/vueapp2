using FluentAssertions;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Delegates.Queries;
using GoRegister.TestingCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GoRegister.ApplicationCore.BuildTests.Domain.Delegates
{
    public class GetTestDelegatesForPreviewShould : DatabaseContextTest
    {
        private readonly List<DelegateUser> _delegates;

        public GetTestDelegatesForPreviewShould()
        {

            using (var db = GetDatabase())
            {
                _delegates = new List<DelegateUser>
                {
                    new DelegateUser{Id = 100, UniqueIdentifier= Guid.NewGuid(), ProjectId = ProjectId, ApplicationUser = new ApplicationUser{ Id=100, FirstName = "Del", LastName = "1" } },
                    new DelegateUser{Id = 101, UniqueIdentifier= Guid.NewGuid(), ProjectId= ProjectId, ApplicationUser = new ApplicationUser{ Id=101, FirstName = "TestDel", LastName = "1" }, IsTest = true },
                    new DelegateUser{Id = 102, UniqueIdentifier= Guid.NewGuid(), ProjectId= ProjectId, ApplicationUser = new ApplicationUser{ Id=102, FirstName = "TestDel", LastName = "2" }, IsTest = true },
                };
                db.Delegates.AddRange(_delegates);
                db.SaveChanges();
            }
        }

        [Fact]
        public async Task Return_ListOfDelegateListItemModel()
        {
            using (var db = GetDatabase())
            {
                var sut = new GetTestDelegatesForPreview.QueryHandler(db);
                var result = await sut.Handle(new GetTestDelegatesForPreview.Query(), new CancellationToken());
                result.Should().BeOfType<GetTestDelegatesForPreview.DelegateModel>();
            }
        }

        [Fact]
        public async Task Return_Only_TestDelegates()
        {
            using (var db = GetDatabase())
            {
                var sut = new GetTestDelegatesForPreview.QueryHandler(db);
                var result = await sut.Handle(new GetTestDelegatesForPreview.Query(), new CancellationToken());

                result.Delegates.Count.Should().Be(2);
            }
        }

        [Fact]
        public async Task Preselect_Cookie_Value()
        {
            using (var db = GetDatabase())
            {
                var sut = new GetTestDelegatesForPreview.QueryHandler(db);
                var result = await sut.Handle(new GetTestDelegatesForPreview.Query { CookieValue = "102" }, new CancellationToken());

                result.Delegates.First(d => d.Value == 102.ToString()).Selected.Should().BeTrue();
            }
        }

    }
}
