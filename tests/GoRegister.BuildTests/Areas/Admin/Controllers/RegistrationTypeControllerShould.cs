using FluentAssertions;
using GoRegister.Areas.Admin.Controllers;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Registration.Models;
using GoRegister.ApplicationCore.Domain.Registration.Services;
using GoRegister.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;


namespace GoRegister.Tests.Areas.Admin.Controllers
{
    public class RegistrationTypeControllerShould
    {
        private readonly RegistrationTypeController _sut; // sut is short for System Under Test

        private readonly Mock<IRegistrationTypeService> _mockRepository;

        private readonly int _projectId = 2;

        private readonly int _id = 1;

        private readonly int _nonExistingId = 2;

        private readonly string _typeName = "Type 1";

        public RegistrationTypeControllerShould()
        {
            _mockRepository = new Mock<IRegistrationTypeService>();
            _sut = new RegistrationTypeController(_mockRepository.Object);
            // Each test runs on it's own instance, they won't affect each other.
        }

        // Test data
        private IEnumerable<RegistrationTypeModel> GetTestRegistrationTypeModels()
        {
            var registrationTypeModel = new List<RegistrationTypeModel>
            {
                new RegistrationTypeModel
                {
                    Id = _id,
                    Name = _typeName,
                    Capacity = 100,
                    DelegateUsers = new List<DelegateUser>()
                },
                new RegistrationTypeModel
                {
                    Id = 2,
                    Name = "Type 2",
                    Capacity = 200,
                    DelegateUsers = new List<DelegateUser>()
                },
                new RegistrationTypeModel
                {
                    Id = 3,
                    Name = "Type 3",
                    Capacity = 300,
                    DelegateUsers = new List<DelegateUser>()
                }
            };

            return registrationTypeModel;
        }

        private async Task MockGetById(int id, string input = "Type 1")
        {
            _mockRepository.Setup(repo => repo.GetById(id))
                .ReturnsAsync(await Task.FromResult(GetTestRegistrationTypeModel(id, input)));
        }

        private RegistrationTypeModel GetTestRegistrationTypeModel(int id, string name)
        {
            return new RegistrationTypeModel
            {
                Id = id,
                ProjectId = _projectId,
                Name = name,
                Capacity = 100
            };
        }

        [Fact]
        public void InheritProjectAdminControllerBase()
        {
            // Assert
            typeof(RegistrationTypeController)
                .Should().BeDerivedFrom<ProjectAdminControllerBase>();
        }

        [Fact]
        public async Task ReturnViewForIndex()
        {
            var result = await _sut.Index(_projectId);

            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public async Task ReturnIndexViewWithModel()
        {
            // Arrange 
            _mockRepository.Setup(x => x.GetAll(_projectId))
                .ReturnsAsync(await Task.FromResult(GetTestRegistrationTypeModels()));

            // Act
            var result = await _sut.Index(_projectId);

            // Assert
            var viewResult = result.Should().BeOfType<ViewResult>().Subject;

            var types = viewResult.ViewData.Model.Should().BeAssignableTo<IEnumerable<RegistrationTypeModel>>().Subject.ToList();

            types.Should().HaveCount(3)
                .And.SatisfyRespectively(first =>
                    {
                        first.Id.Should().Be(_id);
                        first.Name.Should().Be(_typeName);
                        first.Capacity.Should().Be(100);
                    },
                    second =>
                    {
                        second.Id.Should().Be(2);
                        second.Name.Should().Be("Type 2");
                        second.Capacity.Should().Be(200);
                    },
                    third =>
                    {
                        third.Id.Should().Be(3);
                        third.Name.Should().Be("Type 3");
                        third.Capacity.Should().Be(300);
                    });
        }

        [Fact]
        public void ReturnViewForCreate()
        {
            var result = _sut.Create();

            result.Should().BeOfType<ViewResult>();
        }

        [Theory]
        [InlineData("  test")]
        [InlineData("test  ")]
        [InlineData("  test  ")]
        public async Task TrimNameForCreateAction(string input)
        {
            // Arrange 
            var registrationTypeModel = GetTestRegistrationTypeModel(_id, input);

            // Act
            await _sut.Create(_projectId, registrationTypeModel);

            // Assert
            registrationTypeModel.Name.Should().Be("test");
        }

        [Fact]
        public async Task ReturnViewWhenCreateActionHasDuplicateName()
        {
            // Arrange 
            var registrationTypeModel = GetTestRegistrationTypeModel(_id, _typeName);

            _mockRepository.Setup(x => x.IsDuplicateName(registrationTypeModel.ProjectId, registrationTypeModel.Name))
                .ReturnsAsync(await Task.FromResult(true));

            // Act
            var result = await _sut.Create(_projectId, registrationTypeModel);

            // Assert
            result.Should().BeOfType<ViewResult>();

            result.As<ViewResult>()
                .ViewData
                .ModelState["Name"]
                .Errors
                .Should().HaveCount(1);
        }

        // Testing the ActionFilterAttribute (GoRegister.Filters.ModelStateValidationAttribute)
        // The Create action method will never be called if ModelState is invalid
        [Fact]
        public void ReturnViewResultWhenCreateActionModelStateIsInvalid()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            var context = new ActionExecutingContext(
                new ActionContext
                {
                    HttpContext = httpContext,
                    RouteData = new RouteData(),
                    ActionDescriptor = new ActionDescriptor(),
                },
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                new Mock<Controller>().Object);

            var sut = new GoRegister.Filters.ModelStateValidationAttribute();

            //Act
            context.ModelState.AddModelError("Name", "Required");
            sut.OnActionExecuting(context);

            //Assert
            context.Result.Should().NotBeNull().And.BeOfType<ViewResult>();
        }

        [Fact]
        public async Task ReturnRedirectToIndexActionWhenCreateActionModelStateIsValid()
        {
            // Arrange
            var registrationTypeModel = GetTestRegistrationTypeModel(_id, _typeName);
            registrationTypeModel.RegistrationPathId = 1;

            _mockRepository.Setup(repo => repo.CreateAsync(_projectId, registrationTypeModel, false))
                .ReturnsAsync(await Task.FromResult(5))
                .Verifiable();

            _mockRepository.Setup(x => x.GetRegistrationPathId(_projectId))
                .Returns(1);

            _mockRepository.Setup(x => x.IsDuplicateName(registrationTypeModel.ProjectId, registrationTypeModel.Name))
                .ReturnsAsync(await Task.FromResult(false));

            // Act
            var result = await _sut.Create(_projectId, registrationTypeModel);

            // Assert
            var redirectToActionResult = result.Should().BeOfType<RedirectToActionResult>().Subject;
            redirectToActionResult.ControllerName.Should().BeNull();
            redirectToActionResult.ActionName.Should().BeEquivalentTo("Index");
            _mockRepository.Verify();
        }

        [Fact]
        public async Task ReturnViewForEditWithModel()
        {
            // Arrange
            await MockGetById(_id);

            // Act
            var result = await _sut.Edit(_id);

            // Assert
            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            var type = viewResult.Model.Should().BeAssignableTo<RegistrationTypeModel>().Subject;
            type.Id.Should().Be(_id);
            type.Name.Should().Be(_typeName);
            type.Capacity.Should().Be(100);
        }

        [Fact]
        public async Task ReturnNotFoundForEdit()
        {
            // Arrange
            await MockGetById(_id);

            // Act
            var result = await _sut.Edit(_nonExistingId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task ReturnNotFoundForEditPostAction()
        {
            // Arrange
            await MockGetById(_id);

            var registrationTypeModel = GetTestRegistrationTypeModel(_id, _typeName);

            // Act
            var result = await _sut.Edit(_nonExistingId, registrationTypeModel);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Theory]
        [InlineData("  test")]
        [InlineData("test  ")]
        [InlineData("  test  ")]
        public async Task TrimNameForEditAction(string input)
        {
            // Arrange 
            await MockGetById(_id, input);

            var registrationTypeModel = GetTestRegistrationTypeModel(_id, input);

            // Act
            await _sut.Edit(_id, registrationTypeModel);

            // Assert
            registrationTypeModel.Name.Should().Be("test");
        }

        [Fact]
        public async Task ReturnViewWhenEditActionHasDuplicateName()
        {
            // Arrange 
            await MockGetById(_id);

            var registrationTypeModel = GetTestRegistrationTypeModel(_id, _typeName);

            var notTheSameNameRegistrationTypeModel = GetTestRegistrationTypeModel(2, "Type 2");

            _mockRepository.Setup(repo => repo.GetById(_id))
                .ReturnsAsync(await Task.FromResult(notTheSameNameRegistrationTypeModel));

            _mockRepository.Setup(x => x.IsDuplicateName(registrationTypeModel.ProjectId, registrationTypeModel.Name))
                .ReturnsAsync(await Task.FromResult(true));

            // Act
            var result = await _sut.Edit(_id, registrationTypeModel);

            // Assert
            result.Should().BeOfType<ViewResult>();

            result.As<ViewResult>()
                .ViewData
                .ModelState["Name"]
                .Errors
                .Should().HaveCount(1);
        }

        [Fact]
        public async Task ReturnRedirectToIndexActionWhenEditActionModelStateIsValid()
        {
            // Arrange
            await MockGetById(_id);

            var registrationTypeModel = GetTestRegistrationTypeModel(_id, _typeName);

            _mockRepository.Setup(repo => repo.UpdateAsync(registrationTypeModel))
                .Returns(Task.FromResult(5))
                .Verifiable();

            _mockRepository.Setup(x => x.IsDuplicateName(registrationTypeModel.ProjectId, registrationTypeModel.Name))
                .Returns(Task.FromResult(false));

            // Act
            var result = await _sut.Edit(_id, registrationTypeModel);

            // Assert
            var redirectToActionResult = result.Should().BeOfType<RedirectToActionResult>().Subject;
            redirectToActionResult.ControllerName.Should().BeNull();
            redirectToActionResult.ActionName.Should().BeEquivalentTo("Index");
            _mockRepository.Verify();
        }

        [Fact]
        public async Task ReturnViewForDetailsWithModel()
        {
            // Arrange
            await MockGetById(_id);

            // Act
            var result = await _sut.Details(_id);

            // Assert
            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            var type = viewResult.Model.Should().BeAssignableTo<RegistrationTypeModel>().Subject;
            type.Id.Should().Be(1);
            type.Name.Should().Be(_typeName);
            type.Capacity.Should().Be(100);
        }

        [Fact]
        public async Task ReturnNotFoundForDetails()
        {
            // Arrange
            await MockGetById(_id);

            // Act
            var result = await _sut.Details(_nonExistingId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task ReturnNoContentWhenDelete()
        {
            // Arrange
            await MockGetById(_id);

            var registrationTypeModel = GetTestRegistrationTypeModel(_id, _typeName);

            _mockRepository.Setup(repo => repo.DeleteAsync(registrationTypeModel.Id))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _sut.Delete(_id);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task ReturnNotFoundForDelete()
        {
            // Arrange
            await MockGetById(_id);

            // Act
            var result = await _sut.Delete(_nonExistingId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void HaveValidateAntiForgeryTokenAttributeOnHttpPostActions()
        {
            // assert
            typeof(RegistrationTypeController).Methods()
                .ThatReturn<Task<IActionResult>>()
                .ThatAreDecoratedWith<HttpPostAttribute>()
                .Should()
                .BeDecoratedWith<ValidateAntiForgeryTokenAttribute>(
                    "because all Actions with HttpPost require ValidateAntiForgeryToken");
        }

        [Fact]
        public void HaveModelStateValidationAttributeOnHttpPostActions()
        {
            // assert
            typeof(RegistrationTypeController).Methods()
                .ThatReturn<Task<IActionResult>>()
                .ThatAreDecoratedWith<HttpPostAttribute>()
                .Should()
                .BeDecoratedWith<ModelStateValidationAttribute>(
                    "because all Actions with HttpPost require ModelStateValidationAttribute filter");
        }

    }
}
