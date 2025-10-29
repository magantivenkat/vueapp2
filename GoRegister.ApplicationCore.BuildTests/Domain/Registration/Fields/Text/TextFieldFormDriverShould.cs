using FluentAssertions;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Registration.Fields.Text;
using GoRegister.TestingCore.Forms;
using Xunit;

namespace GoRegister.ApplicationCore.BuildTests.Domain.Registration.Fields.Text
{
    public class TextFieldFormDriverShould : FieldTest<TextField>
    {
        public TextFieldFormDriverShould() : base() {}

        [Theory]
        [InlineData("test", false)]
        [InlineData("test@", false)]
        [InlineData("test.", false)]
        [InlineData("test@test", false)]
        [InlineData("test@test.com", true)]
        public async void ValidateTypeEmail(string email, bool isValid)
        {
            var sut = new TextFieldFormDriver();
            Field.InputType = "email";
            AddFormResponse(email);

            await sut.Process(Field, ResponseContext, ValidationContext, FormData);

            ValidationContext.IsValid.Should().Be(isValid);
        }
    }
}
