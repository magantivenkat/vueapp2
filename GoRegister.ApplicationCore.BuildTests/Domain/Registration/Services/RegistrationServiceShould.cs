using AutoMapper;
using FluentAssertions;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Registration.Framework;
using GoRegister.ApplicationCore.Domain.Registration.Services;
using GoRegister.ApplicationCore.Framework;
using GoRegister.TestingCore;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace GoRegister.ApplicationCore.BuildTests.Domain.Registration.Services
{
    public class RegistrationServiceShould : DatabaseContextTest
    {
        private readonly RegistrationService _sut;
        private readonly Mock<IEnumerable<IFormDriver>> mockFormDrivers = new Mock<IEnumerable<IFormDriver>>();


        public RegistrationServiceShould()
        {
            using (var db = GetDatabase())
            {
                var config = new MapperConfiguration(cfg =>
                {
                    //cfg.CreateMap<Client, ClientModel>().ReverseMap();
                });

                IMapper _mapper = config.CreateMapper();


                _sut = new RegistrationService(mockFormDrivers.Object, db, _mapper, ProjectSettingsAccessor.Object);
            }
        }

        #region CanCancel Tests
        [Fact]
        public void RegPath_CanCancel_IsFalse_ReturnFalse()
        {
            var registrationType = new RegistrationType
            {
                RegistrationPath = new RegistrationPath { CanCancel = false }
            };

            var result = _sut.CanCancel(registrationType);
            result.Should().BeFalse();
        }

        [Fact]
        public void RegPath_CanCancel_ButTodaysDate_AfterDateCancelTo_ReturnFalse()
        {
            SystemTime.Set(() => new System.DateTime(2020, 06, 01));
            var registrationType = new RegistrationType
            {
                RegistrationPath = new RegistrationPath
                {
                    CanCancel = true,
                    DateCancelTo = new System.DateTime(2020, 05, 29)
                }
            };

            var result = _sut.CanCancel(registrationType);
            result.Should().BeFalse();
        }

        [Fact]
        public void RegPath_CanCancel_BeforeDateCancelTo()
        {
            SystemTime.Set(() => new System.DateTime(2020, 06, 01));

            var registrationType = new RegistrationType
            {
                RegistrationPath = new RegistrationPath
                {
                    CanCancel = true,
                    DateCancelTo = new System.DateTime(2020, 06, 10)
                }
            };

            var result = _sut.CanCancel(registrationType);
            result.Should().BeTrue();
        }

        [Fact]
        public void RegPath_CanCancel()
        {
            var registrationType = new RegistrationType
            {
                RegistrationPath = new RegistrationPath { CanCancel = true }
            };

            var result = _sut.CanCancel(registrationType);
            result.Should().BeTrue();
        }

        [Fact]
        public void RegPath_CanCancelFalse_ButHasDate_StillReturnFalse()
        {
            SystemTime.Set(() => new System.DateTime(2020, 06, 01));

            var registrationType = new RegistrationType
            {
                RegistrationPath = new RegistrationPath { 
                    CanCancel = false, 
                    DateCancelTo = new System.DateTime(2020, 06, 10)
                }
            };

            var result = _sut.CanCancel(registrationType);
            result.Should().BeFalse();
        }
        #endregion

        #region CanDecline Tests
        [Fact]
        public void RegPath_CanDecline_IsFalse_ReturnFalse()
        {
            var registrationType = new RegistrationType
            {
                RegistrationPath = new RegistrationPath { CanDecline = false }
            };

            var result = _sut.CanDecline(registrationType);
            result.Should().BeFalse();
        }

        [Fact]
        public void RegPath_CanDecline_ButTodaysDate_AfterDateDeclineTo_ReturnFalse()
        {
            SystemTime.Set(() => new System.DateTime(2020, 06, 01));
            var registrationType = new RegistrationType
            {
                RegistrationPath = new RegistrationPath
                {
                    CanDecline = true,
                    DateDeclineTo = new System.DateTime(2020, 05, 29)
                }
            };

            var result = _sut.CanDecline(registrationType);
            result.Should().BeFalse();
        }

        [Fact]
        public void RegPath_CanDecline_BeforeDateDeclineTo()
        {
            SystemTime.Set(() => new System.DateTime(2020, 06, 01));

            var registrationType = new RegistrationType
            {
                RegistrationPath = new RegistrationPath
                {
                    CanDecline = true,
                    DateDeclineTo = new System.DateTime(2020, 06, 10)
                }
            };

            var result = _sut.CanDecline(registrationType);
            result.Should().BeTrue();
        }

        [Fact]
        public void RegPath_CanDecline()
        {
            var registrationType = new RegistrationType
            {
                RegistrationPath = new RegistrationPath { CanDecline = true }
            };

            var result = _sut.CanDecline(registrationType);
            result.Should().BeTrue();
        }

        [Fact]
        public void RegPath_CanDeclineFalse_ButHasDate_StillReturnFalse()
        {
            SystemTime.Set(() => new System.DateTime(2020, 06, 01));

            var registrationType = new RegistrationType
            {
                RegistrationPath = new RegistrationPath
                {
                    CanDecline = false,
                    DateDeclineTo = new System.DateTime(2020, 06, 10)
                }
            };

            var result = _sut.CanDecline(registrationType);
            result.Should().BeFalse();
        }
        #endregion
    }
}
