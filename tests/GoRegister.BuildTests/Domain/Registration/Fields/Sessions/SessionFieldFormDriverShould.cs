using AutoMapper;
using FluentAssertions;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Registration.Fields.Session;
using GoRegister.ApplicationCore.Domain.Registration.Framework;
using GoRegister.ApplicationCore.Domain.Sessions.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace GoRegister.Tests.Domain.Registration.Fields.Sessions
{
    public class SessionFieldFormDriverShould
    {
        private readonly Mock<IUrlHelper> _urlHelper = new Mock<IUrlHelper>();
        private readonly Mock<ISessionAccessor> _sessionAccessor = new Mock<ISessionAccessor>();
        private readonly Mock<ISessionBookingService> _sessionBookingService = new Mock<ISessionBookingService>();
        private readonly Mock<SessionField> _sessionField = new Mock<SessionField>();


        //private readonly Mock<FormDisplayContext> _formDisplayContext = new Mock<FormDisplayContext>(It.IsAny<IMapper>(), It.IsAny<UserFormResponse>(), true);
        //private readonly Mock<FieldDisplayContext> _fieldDisplayContext = new Mock<FieldDisplayContext>(It.IsAny<int>(), _formDisplayContext.Object);

        [Fact]
        public async Task BeTypeOf_SessionFieldDisplayModel()
        {
            // Arrange
            var sut = new SessionFieldFormDriver(_urlHelper.Object, _sessionAccessor.Object, _sessionBookingService.Object);
            var _formDisplayContext = new Mock<FormDisplayContext>(It.IsAny<IMapper>(), It.IsAny<UserFormResponse>(), true);
            var _fieldDisplayContext = new Mock<FieldDisplayContext>(It.IsAny<int>(), _formDisplayContext.Object);

            var sessionList = new List<Session> {
            new Session{ Name = "Sesh1" } };

            _sessionAccessor.Setup(sa => sa.GetSessionsForCategories(It.IsAny<IEnumerable<int>>())).ReturnsAsync(sessionList);

            // Act
            var result = await sut.Display(_sessionField.Object, _fieldDisplayContext.Object);


            // Assert
            result.Model.Should().BeOfType<SessionFieldDisplayModel>();
        }

    }
}
