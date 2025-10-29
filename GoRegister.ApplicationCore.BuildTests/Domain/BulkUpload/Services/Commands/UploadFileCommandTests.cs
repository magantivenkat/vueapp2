using FluentAssertions;
using GoRegister.ApplicationCore.Domain.BulkUpload.Commands;
using GoRegister.ApplicationCore.Domain.BulkUpload.Models;
using GoRegister.ApplicationCore.Domain.BulkUpload.Services;
using GoRegister.ApplicationCore.Domain.Settings.Services;
using GoRegister.ApplicationCore.Framework.Domain;
using GoRegister.ApplicationCore.Services.FileStorage;
using GoRegister.TestingCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace GoRegister.ApplicationCore.BuildTests.Domain.BulkUpload.Services.Commands
{
    public class UploadFileCommandTests : DatabaseContextTest
    {
        private readonly Mock<IBulkUploadService> _bulkUploadService = new Mock<IBulkUploadService>();
        private readonly Mock<IProjectSettingsAccessor> _projectSettingsAccessor = new Mock<IProjectSettingsAccessor>();
        private readonly Mock<IFileStorage> _fileStorage = new Mock<IFileStorage>();
        private readonly Mock<IConfiguration> _configuration = new Mock<IConfiguration>();
  
        private Mock<IFormFile> _file = new Mock<IFormFile>();

        private readonly BulkUploadMappingModel _validMappingModel = new BulkUploadMappingModel();

        Dictionary<string, string> awsConfiguration;

        public UploadFileCommandTests()
        {
            _file.Setup(f => f.FileName);
            _file.Setup(_ => _.OpenReadStream()).Returns(new MemoryStream());
            _file.Setup(_ => _.FileName).Returns("test.pdf");
            _file.Setup(_ => _.Length).Returns(new MemoryStream().Length);                      

            _validMappingModel.RegistrationTypeStatus = BulkUploadRegistrationTypeStatus.UseDefault;
            _validMappingModel.HeaderMappings.Add(new HeaderMapping { ColumnIndex = 4, FieldId = 1 });
            _validMappingModel.HeaderMappings.Add(new HeaderMapping { ColumnIndex = 3, FieldId = 2 });

            awsConfiguration = new Dictionary<string, string>
                    {
                        {"AwsS3BulkUpload:ParentFolder", "Project"},
                        {"AwsS3BulkUpload:SubFolder", "BulkUpload"}
                    };
        }

        [Fact]
        public async void Handle_Result_Failed()
        {
            var command = new UploadFileCommand { File = _file.Object };

            var sut = new UploadFileCommand.Handler(_bulkUploadService.Object, _projectSettingsAccessor.Object, _fileStorage.Object, _configuration.Object);

            _bulkUploadService.Setup(bus => bus.BuildBulkUploadMapping(It.IsAny<Stream>())).ReturnsAsync(Result.Fail<ResponseContainer<BulkUploadMappingModel, BulkUploadMappingViewModel>>);

            var result = await sut.Handle(command, new System.Threading.CancellationToken());

            result.Failed.Should().BeTrue();
        }

        [Fact]
        public async void Handle_Result_NotEmpty()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddInMemoryCollection(awsConfiguration)
                .Build();

            var command = new UploadFileCommand { File = _file.Object };

            var sut = new UploadFileCommand.Handler(_bulkUploadService.Object, ProjectSettingsAccessor.Object, _fileStorage.Object, configurationBuilder);

            _bulkUploadService.Setup(bus => bus.BuildBulkUploadMapping(It.IsAny<Stream>()))
                               .ReturnsAsync(ResponseContainer.Ok(_validMappingModel, new BulkUploadMappingViewModel()));

            var result = await sut.Handle(command, new System.Threading.CancellationToken());

            result.Should().NotBeNull();
        }

        [Fact]
        public async void Handle_Result_Path_Present()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddInMemoryCollection(awsConfiguration)
                .Build();

            var command = new UploadFileCommand { File = _file.Object };

            var sut = new UploadFileCommand.Handler(_bulkUploadService.Object, ProjectSettingsAccessor.Object, _fileStorage.Object, configurationBuilder);

            _bulkUploadService.Setup(bus => bus.BuildBulkUploadMapping(It.IsAny<Stream>()))
                               .ReturnsAsync(ResponseContainer.Ok(_validMappingModel, new BulkUploadMappingViewModel()));

            var result = await sut.Handle(command, new System.Threading.CancellationToken());

            var path = result.Value.Model.UploadId;
            path.Should().NotBeNull();
        }

        [Fact]
        public async void Handle_Result_Path_Pattern()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddInMemoryCollection(awsConfiguration)
                .Build();

            var command = new UploadFileCommand { File = _file.Object };

            var sut = new UploadFileCommand.Handler(_bulkUploadService.Object, ProjectSettingsAccessor.Object, _fileStorage.Object, configurationBuilder);

            _bulkUploadService.Setup(bus => bus.BuildBulkUploadMapping(It.IsAny<Stream>()))
                               .ReturnsAsync(ResponseContainer.Ok(_validMappingModel, new BulkUploadMappingViewModel()));

            var result = await sut.Handle(command, new System.Threading.CancellationToken());

            var path = result.Value.Model.UploadId;
            
            path.Should().StartWith("Project");
            
        }

        [Fact]
        public async void Handle_Result_Path_Pattern_Guid()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddInMemoryCollection(awsConfiguration)
                .Build();

            ProjectSettings.UniqueId = new Guid("637F7F24-FF23-4474-8D2B-C8E20CAA23C4");

            var projGuid = ProjectSettings.UniqueId;

            var command = new UploadFileCommand { File = _file.Object };

            var sut = new UploadFileCommand.Handler(_bulkUploadService.Object, ProjectSettingsAccessor.Object, _fileStorage.Object, configurationBuilder);

            _bulkUploadService.Setup(bus => bus.BuildBulkUploadMapping(It.IsAny<Stream>()))
                               .ReturnsAsync(ResponseContainer.Ok(_validMappingModel, new BulkUploadMappingViewModel()));

            var result = await sut.Handle(command, new System.Threading.CancellationToken());

            var path = result.Value.Model.UploadId;

            path.Should().Contain(projGuid.ToString());

        }

        [Fact]
        public async void Handle_Result_File_Private()
        {           
            var configurationBuilder = new ConfigurationBuilder()
                .AddInMemoryCollection(awsConfiguration)
                .Build();
           
            var command = new UploadFileCommand { File = _file.Object };

            var sut = new UploadFileCommand.Handler(_bulkUploadService.Object, ProjectSettingsAccessor.Object, _fileStorage.Object, configurationBuilder);

            _bulkUploadService.Setup(bus => bus.BuildBulkUploadMapping(It.IsAny<Stream>()))
                               .ReturnsAsync(ResponseContainer.Ok(_validMappingModel, new BulkUploadMappingViewModel()));           

            var result = await sut.Handle(command, new System.Threading.CancellationToken());

            _fileStorage.Verify(f => f.UploadFile(It.IsAny<string>(), It.IsAny<Stream>(), It.Is<UploadFileStorageSettings>(fs => fs.IsPrivate == true)));
        }
    }
}
