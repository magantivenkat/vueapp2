using FluentAssertions;
using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.Areas.Admin.Features.FormAdmin;
using GoRegister.TestingCore;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GoRegister.BuildTests.Areas.Admin.Features.FormAdmin
{
    public class AddFormTests : DatabaseContextTest
    {
        [Fact]
        public async Task SuccessfullyCreatesCustomForm()
        {
            using (var db = GetDatabase())
            {
                var command = new AddForm.Command { DisplayName = "Custom Form", FormType = FormType.Custom };
                var sut = new AddForm.CommandHandler(db);
                var result = await sut.Handle(command, new CancellationToken());
                result.Id.Should().BeGreaterThan(0);
                result.DisplayName.Should().Be("Custom Form");
            }
        }

        [Theory]
        [InlineData(FormType.Cancel)]
        [InlineData(FormType.Decline)]
        public async Task SuccessfullyCreatesForm(FormType formType)
        {
            using (var db = GetDatabase())
            {
                var command = new AddForm.Command { FormType = formType };
                var sut = new AddForm.CommandHandler(db);
                var result = await sut.Handle(command, new CancellationToken());
                result.Id.Should().BeGreaterThan(0);
                result.DisplayName.Should().Be(null);
            }
        }

        [Theory]
        [InlineData(FormType.Cancel)]
        [InlineData(FormType.Decline)]
        public async Task ShouldNotCreateForm_IfExists(FormType formType)
        {
            var form = new Form() 
            {
                ProjectId = ProjectId,
                FormTypeId = formType
            };

            using (var db = GetDatabase())
            {
                db.Forms.Add(form);
                db.SaveChanges();
                
                var command = new AddForm.Command { FormType = formType };
                var sut = new AddForm.CommandHandler(db);
                var result = await sut.Handle(command, new CancellationToken());
                result.Should().Be(null);
            }
        }

        [Fact]
        public async Task ShouldCreateMultipleCustomForms()
        {
            var form = new Form() 
            {
                ProjectId = ProjectId,
                FormTypeId = FormType.Custom,
                AdminDisplayName = "Test"
            };
            var form2 = new Form() 
            {
                ProjectId = ProjectId,
                FormTypeId = FormType.Custom,
                AdminDisplayName = "Test 2"
            };

            using (var db = GetDatabase())
            {
                db.Forms.Add(form);
                db.Forms.Add(form2);
                db.SaveChanges();
                
                var command = new AddForm.Command { FormType = FormType.Custom, DisplayName = "Test" };
                var sut = new AddForm.CommandHandler(db);
                var result = await sut.Handle(command, new CancellationToken());
                result.Id.Should().BeGreaterThan(0);
            }
        }
    }
}
