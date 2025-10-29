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

namespace GoRegister.ApplicationCore.BuildTests.Domain.Registration.Commands
{
    public class CancelRegistrationShould : DatabaseContextTest
    {
        private readonly Mock<ICurrentUserAccessor> mockCurrentUserAccessor = new Mock<ICurrentUserAccessor>();
        private readonly Mock<IRegistrationService> mockRegistrationService = new Mock<IRegistrationService>();
        private readonly Mock<IFormService> mockFormService = new Mock<IFormService>();
        private readonly Mock<IPublisher> mockPublisher = new Mock<IPublisher>();
        const int FORM_ID = 19;
        const int USER_ID = 101;
        const string SERIALIZED_FORM = "SERIALIZED";
        private static DateTime UTC_NOW = new DateTime(1991, 7, 13);

        public CancelRegistrationShould()
        {
            var delegateUser = new DelegateUser
            {
                Id = USER_ID,
                RegistrationStatusId = (int)Data.Enums.RegistrationStatus.Confirmed,
                ApplicationUser = new ApplicationUser { Id = USER_ID, Email = "coel.drysdale@amexgbt.com" },
                IsTest = false,

            };

            SystemTime.Set(() => UTC_NOW);

            mockFormService
                .Setup(fs => fs.GetCancellationForm())
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
                .Setup(rs => rs.CanCancel(It.IsAny<RegistrationType>()))
                .Returns(true);

            mockFormService
                .Setup(fs => fs.ProcessFormResponse(It.IsAny<FormBuilderModel>(), It.IsAny<UserFormResponseModel>(), It.IsAny<FormModel>()));
        }

        [Fact]
        public async Task RegistrationCancelledSuccessfully()
        {
            using (var db = GetDatabase())
            {
                var command = new CancelRegistration.Command() { Model = new FormModel { } };
                var sut = new CancelRegistration.Handler(db, mockCurrentUserAccessor.Object, mockRegistrationService.Object, mockFormService.Object, mockPublisher.Object);
                await sut.Handle(command, new CancellationToken());
            }

            using (var db = GetDatabase())
            {
                var del = db.Delegates.Find(USER_ID);
                del.RegistrationStatusId.Should().Be((int)Data.Enums.RegistrationStatus.Cancelled);
                del.CancellationDocument.Should().Be(SERIALIZED_FORM);
                del.CancelledUtc.Should().Be(UTC_NOW);
                del.ModifiedUtc.Should().Be(UTC_NOW);
            }
        }

        [Fact]
        public async Task RegistrationCancelledSuccessfullyAddsAudit()
        {
            using (var db = GetDatabase())
            {
                var command = new CancelRegistration.Command() { Model = new FormModel { } };
                var sut = new CancelRegistration.Handler(db, mockCurrentUserAccessor.Object, mockRegistrationService.Object, mockFormService.Object, mockPublisher.Object);
                await sut.Handle(command, new CancellationToken());
            }

            using (var db = GetDatabase())
            {
                db.DelegateAudits.ToList().Count.Should().Be(1);
            }
        }

        [Fact]
        public async Task RegistrationCancelledSuccessfullyPublishesCancelledEvent()
        {
            using (var db = GetDatabase())
            {
                var command = new CancelRegistration.Command() { Model = new FormModel { } };
                var sut = new CancelRegistration.Handler(db, mockCurrentUserAccessor.Object, mockRegistrationService.Object, mockFormService.Object, mockPublisher.Object);
                await sut.Handle(command, new CancellationToken());
            }

            mockPublisher.Verify(p => p.Publish(It.Is<DelegateCancelledRegistrationEvent>(
                e => e.DelegateUser.Id == USER_ID
            ), default), Times.Once);
        }

        [Theory]
        [InlineData((int)Data.Enums.RegistrationStatus.Invited)]
        [InlineData((int)Data.Enums.RegistrationStatus.Declined)]
        public async Task DelegateNotConfirmed_Returns_Error(int regStatus)
        {
            var userFormResponse = new UserFormResponse { DelegateUser = new DelegateUser { RegistrationStatusId = regStatus } };

            mockFormService.Setup(fs => fs.GetUserResponseModel(FORM_ID, USER_ID))
                .ReturnsAsync(Result.Ok(new UserFormResponseModel(userFormResponse)));

            using (var db = GetDatabase())
            {
                var command = new CancelRegistration.Command() { Model = new FormModel { } };
                var sut = new CancelRegistration.Handler(db, mockCurrentUserAccessor.Object, mockRegistrationService.Object, mockFormService.Object, mockPublisher.Object);
                var result = await sut.Handle(command, new CancellationToken());

                result.Failed.Should().BeTrue();
            }
        }

        [Fact]
        public async Task DelegateConfirmed_ButNotAllowedToCancel()
        {
            var userFormResponse = new UserFormResponse
            {
                DelegateUser = new DelegateUser
                {
                    RegistrationStatusId = (int)Data.Enums.RegistrationStatus.Confirmed,
                    IsTest = false
                }
            };

            mockFormService.Setup(fs => fs.GetUserResponseModel(FORM_ID, USER_ID))
                .ReturnsAsync(Result.Ok(new UserFormResponseModel(userFormResponse)));

            using (var db = GetDatabase())
            {
                mockRegistrationService
                    .Setup(rs => rs.CanCancel(It.IsAny<RegistrationType>()))
                    .Returns(false);

                var command = new CancelRegistration.Command() { Model = new FormModel { } };
                var sut = new CancelRegistration.Handler(db, mockCurrentUserAccessor.Object, mockRegistrationService.Object, mockFormService.Object, mockPublisher.Object);
                var result = await sut.Handle(command, new CancellationToken());

                result.Failed.Should().BeTrue();
            }
        }
    }
}
