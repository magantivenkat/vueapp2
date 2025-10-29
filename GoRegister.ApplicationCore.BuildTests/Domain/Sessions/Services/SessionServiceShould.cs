using AutoMapper;
using FluentAssertions;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Sessions;
using GoRegister.ApplicationCore.Domain.Sessions.Services;
using GoRegister.ApplicationCore.Domain.Sessions.ViewModels;
using GoRegister.TestingCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace GoRegister.Tests.Domain.Sessions.Services
{
    public class SessionServiceShould : DatabaseContextTest
    {
        IMapper mapper = new Mapper(new MapperConfiguration(mc => mc.AddProfile(new SessionMappingProfile())));

        public SessionServiceShould()
        {
            using (var db = GetDatabase())
            {                
                db.Sessions.Add(new Session { Id = 1, Name = "Test Session 1", Capacity = 2, DateCreatedUtc = DateTime.Now, DateEndUtc = DateTime.Now, DateCloseRegistrationUtc = DateTime.Now });
                db.Sessions.Add(
                    new Session
                    {
                        Id = 2,
                        Name = "Test Session 2",
                        Capacity = 1,
                        DateCreatedUtc = DateTime.Now,
                        DateEndUtc = DateTime.Now,
                        DateCloseRegistrationUtc = DateTime.Now
                    });

                db.SaveChanges();
            }
        }

        [Fact]
        public void ImplementInterfaceISessionService()
        {
            // Assert
            typeof(SessionService)
                .Should().Implement<ISessionService>();
        }

        [Fact]
        public async Task CreateSessionCategory()
        {
            // Arrange
            var category = new SessionCategoryModel { Name = "", Description = "", IsSingleSession = true };

            // Act
            using (var db = GetDatabase())
            {

                var sut = new SessionService(db, mapper);
                var result = await sut.CreateCategoryAsync(category);
                result.Id.Should().Be(1);
            }
        }

        [Fact]
        public async Task Get_Delegates_For_Session()
        {
            using (var db = GetDatabase())
            {
                db.DelegateSessionBookings.Add(new DelegateSessionBooking
                {
                    SessionId = 101,
                    DelegateUser = new DelegateUser { Id = 201, ApplicationUser = new ApplicationUser { Id = 201, FirstName = "Test", LastName = "Test" } }
                });

                db.SaveChanges();
            }

            // Act
            using (var db = GetDatabase())
            {
                var sut = new SessionService(db, mapper);
                var result = await sut.GetDelegatesForSession(101);

                // Assert
                result.Count.Should().Be(1);
            }
        }

        [Fact]
        public async Task NewSession_WithoutACategory_StaysWithoutACategory()
        {
            // BUG FIX: Previously, new Session with empty catergory would still create a category.
            var session = new SessionCreateEditViewModel { ProjectId = ProjectId };
            using (var db = GetDatabase())
            {

                var sut = new SessionService(db, mapper);
                var result = await sut.CreateAsync(session);

                result.SessionCategory.Should().Be(null);
            }
        }

    }
}
