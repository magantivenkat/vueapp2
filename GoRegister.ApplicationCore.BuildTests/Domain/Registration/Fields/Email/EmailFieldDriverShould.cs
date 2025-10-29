using FluentAssertions;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Registration.Fields.Email;
using GoRegister.TestingCore.Forms;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GoRegister.ApplicationCore.BuildTests.Domain.Registration.Fields.Email
{
    public class EmailFieldDriverShould : FieldTest<EmailField>
    {
        public EmailFieldDriverShould() : base()
        {
            
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public async Task Process_NullOrWhiteSpaceEmail_ValidationFails(string email)
        {
            AddFormResponse(email);

            using (var db = GetDatabase())
            {
                var sut = new EmailFieldFormDriver(db, ProjectSettingsAccessor.Object);
                await sut.Process(Field, ResponseContext, ValidationContext, FormData);
            }

            ValidationContext.IsValid.Should().BeFalse();
        }

        [Fact]
        public async Task Process_NoEmail_ValidationFails()
        {
            using (var db = GetDatabase())
            {
                var sut = new EmailFieldFormDriver(db, ProjectSettingsAccessor.Object);
                await sut.Process(Field, ResponseContext, ValidationContext, FormData);
            }

            ValidationContext.IsValid.Should().BeFalse();
        }

        [Fact]
        public async Task Process_NullEmail_ValidationFails()
        {
            FormData.Add(Field.GetRenderId(), JValue.CreateNull());

            using (var db = GetDatabase())
            {
                var sut = new EmailFieldFormDriver(db, ProjectSettingsAccessor.Object);
                await sut.Process(Field, ResponseContext, ValidationContext, FormData);
            }

            ValidationContext.IsValid.Should().BeFalse();
        }

        [Fact]
        public async Task Process_InvalidEmail_ValidationFails()
        {
            AddFormResponse("invalidemail");

            using (var db = GetDatabase())
            {
                var sut = new EmailFieldFormDriver(db, ProjectSettingsAccessor.Object);
                await sut.Process(Field, ResponseContext, ValidationContext, FormData);
            }

            ValidationContext.IsValid.Should().BeFalse();
        }

        [Fact]
        public async Task Process_DuplicateEmail_ValidationFails()
        {
            var email = "duplicate@email.com";
            var normalizedEmail = "DUPLICATE@EMAIL.COM";
            AddFormResponse(email);

            using(var db = GetDatabase())
            {
                db.Users.Add(new ApplicationUser
                {
                    Email = email,
                    NormalizedEmail = normalizedEmail
                });
                db.SaveChanges();
            }

            using (var db = GetDatabase())
            {
                var sut = new EmailFieldFormDriver(db, ProjectSettingsAccessor.Object);
                await sut.Process(Field, ResponseContext, ValidationContext, FormData);
            }

            ValidationContext.IsValid.Should().BeFalse();
        }

        [Fact]
        public async Task Process_DuplicateEmailWithAllowDuplicates_UpdateEmail()
        {
            var email = "duplicate@email.com";
            var normalizedEmail = "DUPLICATE@EMAIL.COM";
            AddFormResponse(email);

            SetProjectSettings(ps => ps.AllowDuplicateEmails = true);

            using (var db = GetDatabase())
            {
                db.Users.Add(new ApplicationUser
                {
                    Email = email,
                    NormalizedEmail = normalizedEmail
                });
                db.SaveChanges();
            }

            using (var db = GetDatabase())
            {
                var sut = new EmailFieldFormDriver(db, ProjectSettingsAccessor.Object);
                await sut.Process(Field, ResponseContext, ValidationContext, FormData);
            }

            ValidationContext.IsValid.Should().BeTrue();
            DelegateUser.ApplicationUser.Email.Should().Be(email);
            DelegateUser.ApplicationUser.NormalizedEmail.Should().Be(normalizedEmail);
        }

        [Fact]
        public async Task Process_TestDelegateWithDuplicateEmail_UpdateEmail()
        {
            var email = "duplicate@email.com";
            var normalizedEmail = "DUPLICATE@EMAIL.COM";
            AddFormResponse(email);

            DelegateUser.IsTest = true;

            using (var db = GetDatabase())
            {
                db.Users.Add(new ApplicationUser
                {
                    Email = email,
                    NormalizedEmail = normalizedEmail
                });
                db.SaveChanges();
            }

            using (var db = GetDatabase())
            {
                var sut = new EmailFieldFormDriver(db, ProjectSettingsAccessor.Object);
                await sut.Process(Field, ResponseContext, ValidationContext, FormData);
            }

            ValidationContext.IsValid.Should().BeTrue();
            DelegateUser.ApplicationUser.Email.Should().Be(email);
            DelegateUser.ApplicationUser.NormalizedEmail.Should().Be(normalizedEmail);
        }
    }
}
