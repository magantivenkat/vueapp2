using FluentAssertions;
using GoRegister.ApplicationCore.Extensions;
using Xunit;

namespace GoRegister.Tests.Extensions.StringExtensionsShould
{
    public class StringExtensionsShould
    {
        [Fact]
        public void Generate_Random_String()
        {
            var result = "".GenerateRandomString(12);

            result.Length.Should().Be(12);            
        }

        [Fact]
        public void Generate_Random_String_With_Prefix()
        {
            var result = "test".GenerateRandomString(12);

            result.Length.Should().Be(16);            
        }

        [Fact]
        public void Generate_Random_String_Using_StaticMethod()
        {
            var result = StringExtensions.GenerateRandomString(12);
            result.Length.Should().Be(12);            
        }


    }
}
