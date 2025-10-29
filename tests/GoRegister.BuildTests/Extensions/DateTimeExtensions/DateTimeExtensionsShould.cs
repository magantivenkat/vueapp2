using FluentAssertions;
using GoRegister.ApplicationCore.Extensions;
using System;
using Xunit;

namespace GoRegister.Tests.Extensions.DateTimeExtensionsShould
{
    public class DateTimeExtensionsShould
    {
        [Fact]
        public void UtcTime_Plus_Timezone()
        {
            var date = new DateTime(2020, 01, 01, 13, 0, 0);
            var result = date.ConvertToUserProfileTimeZone("New Zealand Standard Time");
            result.Hour.Should().Be(2);
        }
    }
}
