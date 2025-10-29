using Moq;
using GoRegister.Areas.Admin.Features.Search;
using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;
using GoRegister.ApplicationCore.Framework.Dapper;

namespace GoRegister.Tests.Areas.Admin.Features.Search
{
    public class SearchQueryHandlerTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async void Handle_NullEmptyOrWhitespaceInput_EmptyList(string input)
        {
            //Arange
            var slickConnection = new Mock<ISlickConnection>();

            var sut = new SearchQuery.QueryHandler(slickConnection.Object);
            var query = new SearchQuery.Query()
            {
                Value = input
            };

            //Act
            var results = await sut.Handle(query, new System.Threading.CancellationToken());

            //Assert
            results.Should().BeEmpty();
            slickConnection.Verify(e => e.Get(), Times.Never);
        }

        [Theory]
        [InlineData("foo", "%foo%")]
        [InlineData(" foo", "%foo%")]
        [InlineData("foo ", "%foo%")]
        [InlineData("foo bar", "%foo%bar%")]
        [InlineData("foo  bar", "%foo%bar%")] // multiple spaces
        [InlineData(" foo  bar ", "%foo%bar%")] // multiple words with leading and trailing space
        public async void Handle_ValidInput_GenerateSqlLikeExpression(string input, string result)
        {
            //Arrange
            var slick = new Mock<Slick>();
            slick.CallBase = true;
            slick.Setup(e => e.QueryAsync<SearchQuery.Result>(It.IsAny<string>(), It.IsAny<object>(), null, null, null)).ReturnsAsync(new List<SearchQuery.Result>());

            var slickConnection = new Mock<ISlickConnection>();
            slickConnection.Setup(e => e.Get()).Returns(slick.Object);

            var sut = new SearchQuery.QueryHandler(slickConnection.Object);
            var query = new SearchQuery.Query()
            {
                Value = input
            };

            //Act
            var results = await sut.Handle(query, new System.Threading.CancellationToken());

            //Assert
            object PARAM_RESULT = new { likeQuery = result };
            Func<object, bool> validate = actual =>
            {
                actual.Should().BeEquivalentTo(PARAM_RESULT, options => options.Including(e => e.SelectedMemberPath == "likeQuery"));
                return true;
            };

            slick.Verify(e => e.QueryAsync<SearchQuery.Result>(It.IsAny<string>(), It.Is<object>(actual => validate(actual)), null, null, null));
        }
    }
}
