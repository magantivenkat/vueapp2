using GoRegister.ApplicationCore.Domain.Registration.Commands;
using GoRegister.ApplicationCore.Domain.Registration.Models;
using GoRegister.ApplicationCore.Domain.Registration.Services;
using GoRegister.ApplicationCore.Framework.Domain;
using GoRegister.ApplicationCore.Framework.Identity;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Models;
using GoRegister.TestingCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Registration;
using FluentAssertions;
using GoRegister.ApplicationCore.Domain.Registration.EventHandlers;
using System.Threading;
using System.Linq;
using GoRegister.ApplicationCore.Domain.Registration.Events;
using GoRegister.ApplicationCore.Data.Enums;

namespace GoRegister.ApplicationCore.BuildTests.Domain.Registration.Commands
{
    public class DeclineRegistrationShould : DatabaseContextTest
    {
        private readonly Mock<ICurrentUserAccessor> mockCurrentUserAccessor = new Mock<ICurrentUserAccessor>();
        private readonly Mock<IRegistrationService> mockRegistrationService = new Mock<IRegistrationService>();
        private readonly Mock<IFormService> mockFormService = new Mock<IFormService>();
        private readonly Mock<IPublisher> mockPublisher = new Mock<IPublisher>();
        const int FORM_ID = 19;
        const int USER_ID = 101;
        const string SERIALIZED_FORM = "SERIALIZED";
        private static DateTime UTC_NOW = new DateTime(1991, 7, 13);

        public DeclineRegistrationShould()
        {
            var delegateUser = new DelegateUser
            {
                Id = USER_ID,
                RegistrationStatusId = (int)Data.Enums.RegistrationStatus.Invited,
                ApplicationUser = new ApplicationUser { Id = USER_ID, Email = "coel.drysdale@amexgbt.com" },
                IsTest = false,

            };

            SystemTime.Set(() => UTC_NOW);

            mockFormService
                .Setup(fs => fs.GetForm(FormType.Decline))
                .ReturnsAsync(new FormBuilderModel { Form = new Form { Id = FORM_ID } });

            mockFormService
                .Setup(fs => fs.SerializeForm(It.IsAny<UserFormResponse>(), It.IsAny<FormBuilderModel>()))
                .Returns(SERIALIZED_FORM);

            mockCurrentUserAccessor
                .Setup(us => us.GetUserId())
                .Returns(USER_ID);

            mockFormService
                .Setup(fs => fs.GetUserResponseModel(FORM_ID, USER_ID))
                .ReturnsAsync(Result.Ok(new UserFormResponseModel(new UserFormResponse { DelegateUser = delegateUser })));

            mockRegistrationService
                .Setup(rs => rs.CanDecline(It.IsAny<RegistrationType>()))
                .Returns(true);

            mockFormService
                .Setup(fs => fs.ProcessFormResponse(It.IsAny<FormBuilderModel>(), It.IsAny<UserFormResponseModel>(), It.IsAny<FormModel>()));
        }

        [Fact]
        public async Task RegistrationDeclinedSuccessfully()
        {
            using (var db = GetDatabase())
            {
                var command = new DeclineRegistration.Command() { Model = new FormModel { } };
                var sut = new DeclineRegistration.Handler(db, mockCurrentUserAccessor.Object, mockRegistrationService.Object, mockFormService.Object, mockPublisher.Object);
                await sut.Handle(command, new CancellationToken());
            }

            using (var db = GetDatabase())
            {
                var del = db.Delegates.Find(USER_ID);
                del.RegistrationStatusId.Should().Be((int)Data.Enums.RegistrationStatus.Declined);
                del.DeclineDocument.Should().Be(SERIALIZED_FORM);
                del.DeclinedUtc.Should().Be(UTC_NOW);
                del.ModifiedUtc.Should().Be(UTC_NOW);
            }
        }

        [Fact]
        public async Task RegistrationDeclinedSuccessfullyAddsAudit()
        {
            using (var db = GetDatabase())
            {
                var command = new DeclineRegistration.Command() { Model = new FormModel { } };
                var sut = new DeclineRegistration.Handler(db, mockCurrentUserAccessor.Object, mockRegistrationService.Object, mockFormService.Object, mockPublisher.Object);
                await sut.Handle(command, new CancellationToken());
            }

            using (var db = GetDatabase())
            {
                db.DelegateAudits.ToList().Count.Should().Be(1);
            }
        }

        [Fact]
        public async Task RegistrationDeclinedSuccessfullyPublishesDeclinedEvent()
        {
            using (var db = GetDatabase())
            {
                var command = new DeclineRegistration.Command() { Model = new FormModel { } };
                var sut = new DeclineRegistration.Handler(db, mockCurrentUserAccessor.Object, mockRegistrationService.Object, mockFormService.Object, mockPublisher.Object);
                await sut.Handle(command, new CancellationToken());
            }

            mockPublisher.Verify(p => p.Publish(It.Is<DelegateDeclinedRegistrationEvent>(
                e => e.DelegateUser.Id == USER_ID
            ), default), Times.Once);
        }

        [Theory]
        [InlineData((int)Data.Enums.RegistrationStatus.NotInvited)]
        [InlineData((int)Data.Enums.RegistrationStatus.Confirmed)]
        public async Task DelegateNot_Invited_Returns_Error(int regStatus)
        {
            var userFormResponse = new UserFormResponse { DelegateUser = new DelegateUser { RegistrationStatusId = regStatus } };

            mockFormService.Setup(fs => fs.GetUserResponseModel(FORM_ID, USER_ID))
                .ReturnsAsync(Result.Ok(new UserFormResponseModel(userFormResponse)));

            using (var db = GetDatabase())
            {
                var command = new DeclineRegistration.Command() { Model = new FormModel { } };
                var sut = new DeclineRegistration.Handler(db, mockCurrentUserAccessor.Object, mockRegistrationService.Object, mockFormService.Object, mockPublisher.Object);
                var result = await sut.Handle(command, new CancellationToken());

                result.Failed.Should().BeTrue();
            }
        }

        [Fact]
        public async Task DelegateInvited_ButNotAllowedToCancel()
        {
            var userFormResponse = new UserFormResponse
            {
                DelegateUser = new DelegateUser
                {
                    RegistrationStatusId = (int)Data.Enums.RegistrationStatus.Invited,
                    IsTest = false
                }
            };

            mockFormService.Setup(fs => fs.GetUserResponseModel(FORM_ID, USER_ID))
                .ReturnsAsync(Result.Ok(new UserFormResponseModel(userFormResponse)));

            using (var db = GetDatabase())
            {
                mockRegistrationService
                    .Setup(rs => rs.CanDecline(It.IsAny<RegistrationType>()))
                    .Returns(false);

                var command = new DeclineRegistration.Command() { Model = new FormModel { } };
                var sut = new DeclineRegistration.Handler(db, mockCurrentUserAccessor.Object, mockRegistrationService.Object, mockFormService.Object, mockPublisher.Object);
                var result = await sut.Handle(command, new CancellationToken());

                result.Failed.Should().BeTrue();
            }
        }
    }
}
