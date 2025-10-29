using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Clients;
using GoRegister.ApplicationCore.Framework;
using GoRegister.ApplicationCore.Framework.DataTables;
using GoRegister.ApplicationCore.Framework.Multitenancy;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace GoRegister.Tests.Domain.Clients.Services
{
    public class ClientServiceShould
    {
        private readonly ClientService _sut; // sut is short for System Under Test

        public ApplicationDbContext GetContextWithData()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var tenant = new ProjectTenant { Id = 1 };
            var tenantAccessorMock = new Mock<ITenantAccessor>();
            tenantAccessorMock.Setup(e => e.Get).Returns(tenant);

            var context = new ApplicationDbContext(tenantAccessorMock.Object, options);
            context.Clients.Add(new Client { Id = 1000, Name = "Test1", DateCreated = DateTime.Now });
            context.Clients.Add(new Client { Id = 1001, Name = "Test2", DateCreated = DateTime.Now });

            context.SaveChanges();

            return context;
        }

        public ClientServiceShould()
        {
            IMapper mapper = new Mapper(new MapperConfiguration(mc => mc.AddProfile(new ClientMappingProfile())));

            _sut = new ClientService(GetContextWithData(), mapper);
        }

        [Fact]
        public void ImplementInterfaceIClientService()
        {
            // Assert
            typeof(ClientService)
                .Should().Implement<IClientService>();
        }

        [Fact]
        public async Task GetList_TypeOf_ListClientModel()
        {
            var result = await _sut.GetList();

            result.Should().BeOfType<List<ClientModel>>();
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task SutSave_When_NewClient_Then_IdNotZero()
        {
            var newClient = new ClientModel
            {
                Name = "New Client"
            };
            var result = await _sut.Save(newClient);

            result.Id.Should().NotBe(0);
        }

        [Fact]
        public async Task SutSave_When_UpdateClient_Then_UpdatedNameIsOther()
        {
            var updateClient = new ClientModel
            {
                Id = 1000,
                Name = "Other"
            };
            var result = await _sut.Save(updateClient);

            result.Name.Should().Be("Other");
        }

        [Fact]
        public async void CreateNewClientWithEmail()
        {
            var model = new ClientModel { Id = 0, Name = "New Client", Email = "newclient@test.com" };
            var result = await _sut.Save(model);

            result.Id.Should().NotBe(0);
            result.ClientEmails.Count.Should().NotBe(0);
        }

        [Fact]
        public async Task SutFilterClientTable_When_SearchValueIs1_Then_CountIs1()
        {
            var fixture = new Fixture();
            var dtParameters = new DataTables.DtParameters { Search = new DataTables.DtSearch { Value = "1" } };
            var clients = await _sut.GetList();

            var result = _sut.FilterClientTable(dtParameters, clients);

            result.Count().Should().Be(1);
        }

        [Fact]
        public async Task CreateClientEmail()
        {
            // Todo
            CreateEditClientEmailModel model = new CreateEditClientEmailModel { ClientId = 1000, Email = "test@test.com" };
            var result = await _sut.SaveEmailAsync(model);
            result.Id.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task UpdateClientEmail()
        {
            // Todo
            CreateEditClientEmailModel model = new CreateEditClientEmailModel { ClientId = 1000, Email = "test@test.com" };
            var newEmail = await _sut.SaveEmailAsync(model);
            model.Id = newEmail.Id;
            model.Email = "testUpdate@test.com";

            var result = await _sut.SaveEmailAsync(model);
            result.Id.Should().Be(newEmail.Id);
        }

        [Fact]
        public async Task DeleteClientEmail()
        {
            // Todo
            CreateEditClientEmailModel model = new CreateEditClientEmailModel { ClientId = 1000, Email = "test@test.com" };
            var email = await _sut.SaveEmailAsync(model);

            bool result = await _sut.DeleteEmailAsync(email.Id);


            result.Should().BeTrue();

        }

        [Fact]
        public async Task GetById_BeNull()
        {
            var result = await _sut.GetById(1);

            result.Should().BeNull();            
        }

        [Fact]
        public async Task GetById_TypeOf_ClientModel()
        {
            var result = await _sut.GetById(1000);

            result.Should().NotBeNull();
            result.Should().BeOfType<ClientModel>();
        }

        [Fact]
        public async Task  GetClientDropdownList_NotNull()
        {
            var result = await _sut.GetClientDropdownList();

            result.Should().NotBeNull();
            result.Should().BeOfType<List<SelectListItem>>();

        }       
    }
}
