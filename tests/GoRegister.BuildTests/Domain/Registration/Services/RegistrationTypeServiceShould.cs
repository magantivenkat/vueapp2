using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Registration;
using GoRegister.ApplicationCore.Domain.Registration.Models;
using GoRegister.ApplicationCore.Domain.Registration.Services;
using GoRegister.ApplicationCore.Framework;
using GoRegister.ApplicationCore.Framework.Multitenancy;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace GoRegister.Tests.Domain.Registration.Services
{
    public class RegistrationTypeServiceShould
    {
        private readonly RegistrationTypeService _sut; // sut is short for System Under Test

        private const int ExistingProjectId = 1;
        private const int NonExistingProjectId = 99;

        private const int ExistingId = 1;
        private const int NonExistingId = 99;

        public ApplicationDbContext GetContextWithData()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var tenant = new ProjectTenant { Id = 2, IsAdmin = true };
            var tenantAccessorMock = new Mock<ITenantAccessor>();
            tenantAccessorMock.Setup(e => e.Get).Returns(tenant);

            var context = new ApplicationDbContext(tenantAccessorMock.Object, options);

            var project = new Project { Id = ExistingProjectId, Name = "Project 1" };

            var registrationPath = new RegistrationPath { Id = 1, ProjectId = project.Id, Name = "Reg Path" };

            context.RegistrationTypes.Add(new RegistrationType { Id = 1, ProjectId = project.Id, RegistrationPathId = registrationPath.Id, Name = "Type 1", Capacity = 100, IsDeleted = false });
            context.RegistrationTypes.Add(new RegistrationType { Id = 2, ProjectId = project.Id, RegistrationPathId = registrationPath.Id, Name = "Type 2", Capacity = 200, IsDeleted = false });
            // IsDeleted
            context.RegistrationTypes.Add(new RegistrationType { Id = 3, ProjectId = project.Id, RegistrationPathId = registrationPath.Id, Name = "Type 3", Capacity = 300, IsDeleted = true });
            context.RegistrationTypes.Add(new RegistrationType { Id = 4, ProjectId = project.Id, RegistrationPathId = registrationPath.Id, Name = "Type 4", Capacity = 400, IsDeleted = true });

            context.SaveChanges();

            return context;
        }

        public RegistrationTypeServiceShould()
        {
            IMapper mapper = new Mapper(new MapperConfiguration(mc => mc.AddProfile(new RegistrationProfile())));

            _sut = new RegistrationTypeService(mapper, GetContextWithData());
        }

        [Fact]
        public void ImplementInterfaceIRegistrationTypeService()
        {
            // Assert
            typeof(RegistrationTypeService)
                .Should().Implement<IRegistrationTypeService>();
        }

        [Fact]
        public async Task CallGetAllWithValidModel()
        {
            var result = await _sut.GetAll(ExistingProjectId);

            result.Should().BeOfType<List<RegistrationTypeModel>>()
                .And.HaveCount(2)
                .And.SatisfyRespectively(
                first =>
                {
                    first.Id.Should().Be(1);
                    first.Name.Should().Be("Type 1");
                    first.Capacity.Should().Be(100);
                },
                second =>
                {
                    second.Id.Should().Be(2);
                    second.Name.Should().Be("Type 2");
                    second.Capacity.Should().Be(200);
                });
        }

        [Fact]
        public async Task CallGetAllWithEmptyModel()
        {
            var result = await _sut.GetAll(NonExistingProjectId);

            result.Should().BeOfType<List<RegistrationTypeModel>>()
                .And.BeEmpty();
        }

        [Fact]
        public async Task CallGetByIdWithValidModel()
        {
            var result = await _sut.GetById(ExistingId);

            result.Should().BeOfType<RegistrationTypeModel>()
                .And.NotBeNull();

            result.Id.Should().Be(ExistingId);
            result.Name.Should().Be("Type 1");
            result.Capacity.Should().Be(100);

        }

        [Fact]
        public async Task CallGetByIdWithEmptyModel()
        {
            var result = await _sut.GetById(NonExistingId);

            result.Should().BeNull();
        }

        [Fact]
        public async Task CallExistsAndReturnTrue()
        {
            var result = await _sut.Exists(ExistingId);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task CallExistAndReturnFalse()
        {
            var result = await _sut.Exists(NonExistingId);

            result.Should().BeFalse();
        }

        [Theory]
        [InlineData(ExistingProjectId, "Type 1")]
        [InlineData(ExistingProjectId, "Type 2")]
        public async Task CallIsDuplicateNameAndReturnTrue(int id, string name)
        {
            var result = await _sut.IsDuplicateName(id, name);

            result.Should().BeTrue();
        }

        [Theory]
        [InlineData(ExistingProjectId, "X")]
        [InlineData(ExistingProjectId, "Type 4")]
        [InlineData(NonExistingProjectId, "Type 1")]
        public async Task CallIsDuplicateNameAndReturnFalse(int id, string name)
        {
            var result = await _sut.IsDuplicateName(id, name);

            result.Should().BeFalse();
        }

        [Fact]
        public async Task CallDeleteWithSuccess()
        {
            await _sut.DeleteAsync(ExistingId);

            var result = await _sut.GetAll(ExistingProjectId);

            result.Should().BeOfType<List<RegistrationTypeModel>>()
                .And.HaveCount(1)
                .And.SatisfyRespectively(
                    first =>
                    {
                        first.Id.Should().Be(2);
                        first.Name.Should().Be("Type 2");
                        first.Capacity.Should().Be(200);
                    });
        }

        [Fact]
        public async Task CallDeleteWithNoSuccess()
        {
            await _sut.DeleteAsync(4);

            var result = await _sut.GetAll(ExistingProjectId);

            result.Should().BeOfType<List<RegistrationTypeModel>>()
                .And.HaveCount(2)
                .And.SatisfyRespectively(
                    first =>
                    {
                        first.Id.Should().Be(1);
                        first.Name.Should().Be("Type 1");
                        first.Capacity.Should().Be(100);
                    },
                    second =>
                    {
                        second.Id.Should().Be(2);
                        second.Name.Should().Be("Type 2");
                        second.Capacity.Should().Be(200);
                    });
        }

        [Fact]
        public async Task CallUpdateWithSuccess()
        {
            await _sut.UpdateAsync(new RegistrationTypeModel
            { Id = ExistingId, ProjectId = ExistingProjectId, Name = "Type X", Capacity = 999 });

            var result = await _sut.GetById(ExistingId);

            result.Should().BeOfType<RegistrationTypeModel>()
                .And.NotBeNull();

            result.Id.Should().Be(ExistingId);
            result.Name.Should().Be("Type X");
            result.Capacity.Should().Be(999);
        }

        [Fact]
        public async Task CallCreateWithSuccess()
        {
            var createModel = new RegistrationTypeModel
            {
                Id = 5,
                ProjectId = ExistingProjectId,
                RegistrationPathId = 1,
                Name = "Type X",
                Capacity = 999
            };

            var createId = await _sut.CreateAsync(ExistingProjectId, createModel, false);

            var result = await _sut.GetById(createId);

            result.Should().BeOfType<RegistrationTypeModel>()
                .And.NotBeNull();

            result.Id.Should().Be(createId);
            result.Name.Should().Be("Type X");
            result.Capacity.Should().Be(999);

            var resultAll = await _sut.GetAll(ExistingProjectId);

            resultAll.Should().BeOfType<List<RegistrationTypeModel>>()
                .And.HaveCount(3)
                .And.SatisfyRespectively(
                    first =>
                    {
                        first.Id.Should().Be(1);
                        first.Name.Should().Be("Type 1");
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
                        third.Id.Should().Be(createId);
                        third.Name.Should().Be("Type X");
                        third.Capacity.Should().Be(999);
                    });

        }

    }
}
