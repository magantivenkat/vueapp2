using AutoMapper;
using FluentAssertions;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.CustomPages.Services;
using GoRegister.TestingCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GoRegister.ApplicationCore.BuildTests.Domain.CustomPages.Services
{
    public class CustomPagesServicesTests : DatabaseContextTest
    {
        //private readonly IMapper _mapper;
        private readonly IMapper _mapper;

        public CustomPagesServicesTests()
        {
            //Configure mapping just for this test
            var config = new MapperConfiguration(cfg =>
            {
                //cfg.CreateMap<Client, ClientModel>().ReverseMap();
            });

            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task ReturnCustomPage()
        {
            using (var db = GetDatabase())
            {
                //Arrange
                var delegateUser = new DelegateUser { Id = 1, RegistrationTypeId = 1, RegistrationStatusId = 1 };
                var custPage = new CustomPage
                {
                    IsVisible = true,
                    CustomPageRegistrationStatuses = new List<CustomPageRegistrationStatus>() { new CustomPageRegistrationStatus() { RegistrationStatusId = 1 } },
                    CustomPageRegistrationTypes = new List<CustomPageRegistrationType>() { new CustomPageRegistrationType() { RegistrationTypeId = 1 }, new CustomPageRegistrationType() { RegistrationTypeId = 2 } }
                };

                db.Delegates.Add(delegateUser);
                db.CustomPages.Add(custPage);
                db.SaveChanges();

                //Act
                var sut = new CustomPageService(db, _mapper, null);
                var result = await sut.GetNavBar(1);

                //Assert
                result.Count().Should().Be(1);
            }
        }

        [Fact]
        public async Task GetNavBar_IsVisibleOnly_Null()
        {
            using (var db = GetDatabase())
            {
                var delegateUser = new DelegateUser { Id = 1, RegistrationTypeId = 1, RegistrationStatusId = 1 };
                var custPage = new CustomPage
                {
                    IsVisible = false,
                    CustomPageRegistrationStatuses = new List<CustomPageRegistrationStatus>() { new CustomPageRegistrationStatus() { RegistrationStatusId = 1 } },
                    CustomPageRegistrationTypes = new List<CustomPageRegistrationType>() { new CustomPageRegistrationType() { RegistrationTypeId = 1 }, new CustomPageRegistrationType() { RegistrationTypeId = 2 } }
                };

                db.Delegates.Add(delegateUser);
                db.CustomPages.Add(custPage);
                db.SaveChanges();

                //Act
                var sut = new CustomPageService(db, _mapper, null);
                var result = await sut.GetNavBar(1);

                //Assert
                result.Count().Should().Be(0);
            }
        }

        //- order of pages returned is correct
        [Fact]
        public async Task GetNavBar_IsInOrder()
        {
            using (var db = GetDatabase())
            {
                var delegateUser = new DelegateUser { Id = 1, RegistrationTypeId = 1, RegistrationStatusId = 1 };
                var custPage = new CustomPage
                {
                    IsVisible = true,
                    CustomPageRegistrationStatuses = new List<CustomPageRegistrationStatus>() { new CustomPageRegistrationStatus() { RegistrationStatusId = 1 } },
                    CustomPageRegistrationTypes = new List<CustomPageRegistrationType>() { new CustomPageRegistrationType() { RegistrationTypeId = 1 }, new CustomPageRegistrationType() { RegistrationTypeId = 2 } },
                    Position = 1

                };

                var custPage1 = new CustomPage
                {
                    IsVisible = true,
                    CustomPageRegistrationStatuses = new List<CustomPageRegistrationStatus>() { new CustomPageRegistrationStatus() { RegistrationStatusId = 1 } },
                    CustomPageRegistrationTypes = new List<CustomPageRegistrationType>() { new CustomPageRegistrationType() { RegistrationTypeId = 1 }, new CustomPageRegistrationType() { RegistrationTypeId = 2 } },
                    Position = 2

                };
                db.Delegates.Add(delegateUser);
                db.CustomPages.Add(custPage);
                db.CustomPages.Add(custPage1);
                db.SaveChanges();

                //Act
                var sut = new CustomPageService(db, _mapper, null);
                var result = await sut.GetNavBar(1);

                //Assert
                result.Should().BeInAscendingOrder(x => x.Position);
            }
        }

        //- only pages matching the users reg type are returned(or ones with no reg types)
        [Fact]
        public async Task GetNavBar_WithRegTypes_Matching()
        {
            using (var db = GetDatabase())
            {
                var delegateUser = new DelegateUser { Id = 1, RegistrationTypeId = 1 };
                var custPage = new CustomPage
                {
                    IsVisible = true,
                    CustomPageRegistrationTypes = new List<CustomPageRegistrationType>() { new CustomPageRegistrationType() { RegistrationTypeId = 1 }, new CustomPageRegistrationType() { RegistrationTypeId = 2 } },
                    Position = 1

                };

                var custPage1 = new CustomPage
                {
                    IsVisible = true,
                    CustomPageRegistrationTypes = new List<CustomPageRegistrationType>() { new CustomPageRegistrationType() { RegistrationTypeId = 1 }, new CustomPageRegistrationType() { RegistrationTypeId = 2 } },
                    Position = 2

                };
                db.Delegates.Add(delegateUser);
                db.CustomPages.Add(custPage);
                db.CustomPages.Add(custPage1);
                db.SaveChanges();

                //Act
                var sut = new CustomPageService(db, _mapper, null);
                var result = await sut.GetNavBar(1);

                //Assert
                result.Count().Should().Be(2);
            }
        }

        [Fact]
        public async Task GetNavBar_WithRegTypes_NotMatching()
        {
            using (var db = GetDatabase())
            {
                var delegateUser = new DelegateUser { Id = 2, RegistrationTypeId = 2 };
                var custPage = new CustomPage
                {
                    IsVisible = true,
                    CustomPageRegistrationTypes = new List<CustomPageRegistrationType>() { new CustomPageRegistrationType() { RegistrationTypeId = 1 } },
                    Position = 1

                };

                var custPage1 = new CustomPage
                {
                    IsVisible = true,
                    CustomPageRegistrationTypes = new List<CustomPageRegistrationType>() { new CustomPageRegistrationType() { RegistrationTypeId = 1 } },
                    Position = 2

                };

                db.Delegates.Add(delegateUser);
                db.CustomPages.Add(custPage);
                db.CustomPages.Add(custPage1);
                db.SaveChanges();

                //Act
                var sut = new CustomPageService(db, _mapper, null);
                var result = await sut.GetNavBar(2);

                //Assert
                result.Count().Should().Be(0);
            }
        }

        //- only pages matching the users reg status are returned(or ones with no reg status)
        [Fact]
        public async Task GetNavBar_WithRegStatus_Matching()
        {
            using (var db = GetDatabase())
            {
                var delegateUser = new DelegateUser { Id = 1, RegistrationStatusId = 1 };
                var custPage = new CustomPage
                {
                    IsVisible = true,
                    CustomPageRegistrationStatuses = new List<CustomPageRegistrationStatus>() { new CustomPageRegistrationStatus() { RegistrationStatusId = 1 }, new CustomPageRegistrationStatus { RegistrationStatusId = 2 } },
                    Position = 1

                };

                var custPage1 = new CustomPage
                {
                    IsVisible = true,
                    CustomPageRegistrationStatuses = new List<CustomPageRegistrationStatus>() { new CustomPageRegistrationStatus() { RegistrationStatusId = 1 }, new CustomPageRegistrationStatus { RegistrationStatusId = 2 } },
                    Position = 2

                };
                db.Delegates.Add(delegateUser);
                db.CustomPages.Add(custPage);
                db.CustomPages.Add(custPage1);
                db.SaveChanges();

                //Act
                var sut = new CustomPageService(db, _mapper, null);
                var result = await sut.GetNavBar(1);

                //Assert
                result.Count().Should().Be(2);
            }
        }

        [Fact]
        public async Task GetNavBar_WithRegStatus_NotMatching()
        {
            using (var db = GetDatabase())
            {
                var delegateUser = new DelegateUser { Id = 12 };
                var custPage = new CustomPage
                {
                    IsVisible = true,
                    //CustomPageRegistrationTypes = new List<CustomPageRegistrationType>() { new CustomPageRegistrationType() { RegistrationTypeId = 1 } },
                    Position = 1

                };

                var custPage1 = new CustomPage
                {
                    IsVisible = true,
                    //CustomPageRegistrationTypes = new List<CustomPageRegistrationType>() { new CustomPageRegistrationType() { RegistrationTypeId = 1 } },
                    Position = 2

                };
                db.Delegates.Add(delegateUser);
                db.CustomPages.Add(custPage);
                db.CustomPages.Add(custPage1);
                db.SaveChanges();

                //Act
                var sut = new CustomPageService(db, _mapper, null);
                var result = await sut.GetNavBar(12);

                //Assert
                result.Count().Should().Be(2);
            }
        }
        //- if it matches the users reg type but not status then it shouldn't be returned
        [Fact]
        public async Task GetNavBar_RegType_Matching_RegStatus_NotMatching()
        {
            using (var db = GetDatabase())
            {
                var delegateUser = new DelegateUser { Id = 1, RegistrationTypeId = 1, RegistrationStatusId = 1 };
                var custPage = new CustomPage
                {
                    IsVisible = true,
                    CustomPageRegistrationTypes = new List<CustomPageRegistrationType>() { new CustomPageRegistrationType() { RegistrationTypeId = 1 } },
                    CustomPageRegistrationStatuses = new List<CustomPageRegistrationStatus>() { new CustomPageRegistrationStatus() { RegistrationStatusId = 11 } },
                    Position = 1

                };

                var custPage1 = new CustomPage
                {
                    IsVisible = true,
                    CustomPageRegistrationTypes = new List<CustomPageRegistrationType>() { new CustomPageRegistrationType() { RegistrationTypeId = 1 } },
                    CustomPageRegistrationStatuses = new List<CustomPageRegistrationStatus>() { new CustomPageRegistrationStatus() { RegistrationStatusId = 11 } },
                    Position = 2

                };
                db.Delegates.Add(delegateUser);
                db.CustomPages.Add(custPage);
                db.CustomPages.Add(custPage1);
                db.SaveChanges();

                //Act
                var sut = new CustomPageService(db, _mapper, null);
                var result = await sut.GetNavBar(1);

                //Assert
                result.Count().Should().Be(0);
            }
        }

        [Fact]
        //- hidden pages are not returned
        public async Task GetNavBar_HiddenPage_NotReturned()
        {
            using (var db = GetDatabase())
            {
                var delegateUser = new DelegateUser { Id = 1, RegistrationTypeId = 1, RegistrationStatusId = 1 };
                var custPage = new CustomPage
                {
                    IsVisible = true,
                    Position = 1

                };

                var custPage1 = new CustomPage
                {
                    IsVisible = false,
                    Position = 2

                };
                db.Delegates.Add(delegateUser);
                db.CustomPages.Add(custPage);
                db.CustomPages.Add(custPage1);
                db.SaveChanges();

                //Act
                var sut = new CustomPageService(db, _mapper, null);
                var result = await sut.GetNavBar(1);

                //Assert
                result.Count().Should().Be(1);
            }
        }
    }
}
