using FluentAssertions;
using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Domain.Delegates;
using GoRegister.ApplicationCore.Domain.Delegates.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace GoRegister.ApplicationCore.BuildTests.Domain.Delegates
{
    public class AttendeeAuthorizationServiceAuthorizeTests
    {
        public AttendeeAuthorizationServiceAuthorizeTests()
        {

        }

        [Fact]
        public void Should_BeTrue_When_NullRegTypeList()
        {
            var sut = GetService();
            var attendee = new AttendeeAuthorizationModel { RegistrationTypeId = 1, RegistrationStatus = RegistrationStatus.Invited };
            var content = new ContentAuthorizationModel { RegistrationTypes = null };

            sut.Authorize(attendee, content).Should().BeTrue();
        }

        [Fact]
        public void Should_BeTrue_When_EmptyRegTypeList()
        {
            var sut = GetService();
            var attendee = new AttendeeAuthorizationModel { RegistrationTypeId = 1, RegistrationStatus = RegistrationStatus.Invited };
            var content = new ContentAuthorizationModel { RegistrationTypes = new List<int>() };

            sut.Authorize(attendee, content).Should().BeTrue();
        }

        [Fact]
        public void Should_BeTrue_When_NullRegStatusList()
        {
            var sut = GetService();
            var attendee = new AttendeeAuthorizationModel { RegistrationTypeId = 1, RegistrationStatus = RegistrationStatus.Invited };
            var content = new ContentAuthorizationModel { RegistrationStatuses = null };

            sut.Authorize(attendee, content).Should().BeTrue();
        }

        [Fact]
        public void Should_BeTrue_When_EmptyRegStatusList()
        {
            var sut = GetService();
            var attendee = new AttendeeAuthorizationModel { RegistrationTypeId = 1, RegistrationStatus = RegistrationStatus.Invited };
            var content = new ContentAuthorizationModel { RegistrationStatuses = new List<RegistrationStatus>() };

            sut.Authorize(attendee, content).Should().BeTrue();
        }

        [Fact]
        public void Should_BeTrue_When_RegStatusListContainsAttendeeRegStatus()
        {
            var sut = GetService();
            var attendee = new AttendeeAuthorizationModel { RegistrationTypeId = 1, RegistrationStatus = RegistrationStatus.Invited };
            var content = new ContentAuthorizationModel { RegistrationStatuses = new List<RegistrationStatus>() { RegistrationStatus.Invited, RegistrationStatus.Confirmed } };

            sut.Authorize(attendee, content).Should().BeTrue();
        }

        [Fact]
        public void Should_BeFalse_When_RegStatusListDoesNotContainAttendeeRegStatus()
        {
            var sut = GetService();
            var attendee = new AttendeeAuthorizationModel { RegistrationTypeId = 1, RegistrationStatus = RegistrationStatus.Invited };
            var content = new ContentAuthorizationModel { RegistrationStatuses = new List<RegistrationStatus>() { RegistrationStatus.Confirmed, RegistrationStatus.Declined } };

            sut.Authorize(attendee, content).Should().BeFalse();
        }

        [Fact]
        public void Should_BeTrue_When_RegStatusListContainsAttendeeRegType()
        {
            var sut = GetService();
            var attendee = new AttendeeAuthorizationModel { RegistrationTypeId = 1, RegistrationStatus = RegistrationStatus.Invited };
            var content = new ContentAuthorizationModel { RegistrationTypes = new List<int> { 1, 2 } };

            sut.Authorize(attendee, content).Should().BeTrue();
        }

        [Fact]
        public void Should_BeFalse_When_RegStatusListDoesNotContainAttendeeRegType()
        {
            var sut = GetService();
            var attendee = new AttendeeAuthorizationModel { RegistrationTypeId = 1, RegistrationStatus = RegistrationStatus.Invited };
            var content = new ContentAuthorizationModel { RegistrationTypes = new List<int> { 2, 3 } };

            sut.Authorize(attendee, content).Should().BeFalse();
        }

        [Fact]
        public void Should_BeFalse_When_NoMatches()
        {
            var sut = GetService();
            var attendee = new AttendeeAuthorizationModel { RegistrationTypeId = 1, RegistrationStatus = RegistrationStatus.Invited };
            var content = new ContentAuthorizationModel
            {
                RegistrationTypes = new List<int> { 2, 3 },
                RegistrationStatuses = new List<RegistrationStatus>
                {
                    RegistrationStatus.Declined,
                    RegistrationStatus.Confirmed
                }
            };

            sut.Authorize(attendee, content).Should().BeFalse();
        }

        [Fact]
        public void Should_BeTrue_When_AnonymousAndAllowsAnonymous()
        {
            var sut = GetService();
            var attendee = new AttendeeAuthorizationModel { IsAnonymous = true };
            var content = new ContentAuthorizationModel
            {
                AllowAnonymous = true,
                RegistrationTypes = new List<int> { 2, 3 },
                RegistrationStatuses = new List<RegistrationStatus>
                {
                    RegistrationStatus.Declined,
                    RegistrationStatus.Confirmed
                }
            };

            sut.Authorize(attendee, content).Should().BeTrue();
        }

        [Fact]
        public void Should_BeFalse_When_AnonymousAndDoesNotAllowAnonymous()
        {
            var sut = GetService();
            var attendee = new AttendeeAuthorizationModel { IsAnonymous = true };
            var content = new ContentAuthorizationModel
            {
                AllowAnonymous = false,
                RegistrationTypes = new List<int> { 2, 3 },
                RegistrationStatuses = new List<RegistrationStatus>
                {
                    RegistrationStatus.Declined,
                    RegistrationStatus.Confirmed
                }
            };

            sut.Authorize(attendee, content).Should().BeFalse();
        }

        [Fact]
        public void Should_BeFalse_When_AuthenticatedAndNoMatchesButAllowsAnonymous()
        {
            var sut = GetService();
            var attendee = new AttendeeAuthorizationModel { RegistrationTypeId = 1, RegistrationStatus = RegistrationStatus.Invited };
            var content = new ContentAuthorizationModel
            {
                AllowAnonymous = true,
                RegistrationTypes = new List<int> { 2, 3 },
                RegistrationStatuses = new List<RegistrationStatus>
                {
                    RegistrationStatus.Declined,
                    RegistrationStatus.Confirmed
                }
            };

            sut.Authorize(attendee, content).Should().BeFalse();
        }

        private IAttendeeAuthorizationService GetService()
        {
            return new AttendeeAuthorizationService();
        }
    }
}
