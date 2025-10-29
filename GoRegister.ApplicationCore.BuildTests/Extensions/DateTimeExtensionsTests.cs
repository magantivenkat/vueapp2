using FluentAssertions;
using GoRegister.ApplicationCore.Extensions;
using GoRegister.TestingCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace GoRegister.ApplicationCore.BuildTests.Extensions
{
    public class DateTimeExtensionsTests : DatabaseContextTest
    {

        [Fact]
        public void ReturnsUniqueTimezones()
        {
            var sut = new List<SelectListItem>();
            var result = sut.GetTimeZoneList();

            var uniqueCount = result.Distinct().Count();
            result.Count.Should().Be(uniqueCount);
            result.GetType().Should().Be(typeof(List<SelectListItem>));
        }
    }
}
