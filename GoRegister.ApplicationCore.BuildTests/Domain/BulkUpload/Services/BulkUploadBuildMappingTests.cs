using AutoMapper;
using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Domain.Registration.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using GoRegister.ApplicationCore.Domain.Registration.Models;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Registration.Framework;
using Xunit;
using GoRegister.ApplicationCore.Domain.BulkUpload.Services;
using GoRegister.TestingCore;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using GoRegister.ApplicationCore.Domain.BulkUpload.Models;
using GoRegister.ApplicationCore.Domain.Delegates;

namespace GoRegister.ApplicationCore.BuildTests.Domain.BulkUpload.Services
{
    public class BulkUploadBuildMappingTests : DatabaseContextTest
    {
        private readonly Mock<IRepository> _repository = new Mock<IRepository>();
        private readonly Mock<IFormService> _formService = new Mock<IFormService>();
        private readonly Mock<IFieldDriverAccessor> _fieldDriverAccessor = new Mock<IFieldDriverAccessor>();
        private readonly Mock<IAttendeeIdentifierService> _attendeeIdentifierService = new Mock<IAttendeeIdentifierService>();

        public BulkUploadBuildMappingTests()
        {
            _formService.Setup(fs => fs.GetRegistrationForm()).ReturnsAsync(new FormBuilderModel
            {
                Fields = new List<Field>
                {
                    new EmailField { Id = 1, FieldTypeId = Data.Enums.FieldTypeEnum.Email, Name = "Email Address", DataTag = "Email" },
                    new TextField { Id = 2, FieldTypeId = Data.Enums.FieldTypeEnum.Textbox, Name = "Text field", DataTag = "text"  }
                }
            });

            _fieldDriverAccessor.Setup(mock => mock.GetFormDriver(Data.Enums.FieldTypeEnum.Email));
        }

        private BulkUploadService GetService()
        {
            return new BulkUploadService(ProjectSettingsAccessor.Object, _repository.Object, _formService.Object, _fieldDriverAccessor.Object, _attendeeIdentifierService.Object);
        }

        [Fact]
        public async Task When_EmptySheet_Expect_Invalid()
        {
            // arrange
            var sut = GetService();

            using (var stream = File.OpenRead(GetFilePath("empty_file")))
            {
                //act
                var result = await sut.BuildBulkUploadMapping(stream);

                // assert
                result.Failed.Should().BeTrue();
            }
        }

        [Fact]
        public async Task When_NoHeaders_Expect_Invalid()
        {
            // arrange
            var sut = GetService();

            using (var stream = File.OpenRead(GetFilePath("empty_header")))
            {
                //act
                var result = await sut.BuildBulkUploadMapping(stream);

                // assert
                result.Failed.Should().BeTrue();
            }
        }

        [Fact]
        public async Task When_NoDelegateData_Expect_Invalid()
        {
            // arrange
            var sut = GetService();

            using (var stream = File.OpenRead(GetFilePath("no_delegates")))
            {
                //act
                var result = await sut.BuildBulkUploadMapping(stream);

                // assert
                result.Failed.Should().BeTrue();
            }
        }

        [Fact]
        public async Task When_ValidFile_Expect_CorrectHeaderMappings()
        {
            // arrange
            var sut = GetService();

            using (var stream = File.OpenRead(GetFilePath("valid")))
            {
                //act
                using (var db = GetAdminDatabase())
                {
                    _repository.Setup(e => e.DbContext).Returns(db);
                    var result = await sut.BuildBulkUploadMapping(stream);
                    // assert
                    result.Failed.Should().BeFalse();
                    var headerMappings = result.Value.Model.HeaderMappings;
                    headerMappings.Should().HaveCount(5);

                    headerMappings[0].ColumnName.Should().Be("A1");
                    headerMappings[0].FieldId.Should().BeNull();

                    headerMappings[1].ColumnName.Should().Be("text");
                    headerMappings[1].FieldId.Should().Be(2);

                    headerMappings[2].ColumnName.Should().Be("Gender");
                    headerMappings[2].FieldId.Should().BeNull();

                    headerMappings[3].ColumnName.Should().Be("Email Address");
                    headerMappings[3].FieldId.Should().Be(1);

                    headerMappings[4].ColumnName.Should().Be("Department");
                    headerMappings[4].FieldId.Should().BeNull();
                }                
            }
        }

        [Fact]
        public async Task When_ValidFileWithSpacesAndCases_Expect_CorrectHeaderMappings()
        {
            // arrange
            var sut = GetService();

            using (var stream = File.OpenRead(GetFilePath("valid_with_spacesandcases")))
            {
                //act
                using (var db = GetAdminDatabase())
                {
                    _repository.Setup(e => e.DbContext).Returns(db);
                    var result = await sut.BuildBulkUploadMapping(stream);
                    // assert
                    result.Failed.Should().BeFalse();
                    var headerMappings = result.Value.Model.HeaderMappings;
                    headerMappings.Should().HaveCount(5);

                    headerMappings[0].ColumnName.Should().Be("A1");
                    headerMappings[0].FieldId.Should().BeNull();

                    headerMappings[1].ColumnName.Should().Be("TexT");
                    headerMappings[1].FieldId.Should().Be(2);

                    headerMappings[2].ColumnName.Should().Be("Gender");
                    headerMappings[2].FieldId.Should().BeNull();

                    headerMappings[3].ColumnName.Should().Be("EMAIl AddreSS");
                    headerMappings[3].FieldId.Should().Be(1);

                    headerMappings[4].ColumnName.Should().Be("DepartmenT");
                    headerMappings[4].FieldId.Should().BeNull();
                }
            }
        }

        [Fact]
        public async Task When_ValidFileWithRegType_Expect_RegTypeFromSheetStatus()
        {
            // arrange
            var sut = GetService();

            using (var stream = File.OpenRead(GetFilePath("valid_withRegistrationType")))
            {
                //act
                using (var db = GetAdminDatabase())
                {
                    _repository.Setup(e => e.DbContext).Returns(db);
                    var result = await sut.BuildBulkUploadMapping(stream);
                    // assert
                    result.Failed.Should().BeFalse();
                    result.Value.Model.RegistrationTypeStatus.Should().Be(BulkUploadRegistrationTypeStatus.UseFromUpload);
                    result.Value.Model.RegistrationTypeColumnIndex = 5;
                }
            }
        }

        [Fact]
        public async Task When_ValidFileWithoutRegTypeAnd1RegType_Expect_DefaultRegTypeStatus()
        {
            // arrange
            using (var db = GetAdminDatabase())
            {
                db.RegistrationTypes.Add(new RegistrationType());
                db.SaveChanges();
            }
            var sut = GetService();

            using (var stream = File.OpenRead(GetFilePath("valid_withoutRegistrationType")))
            {
                //act
                using (var db = GetAdminDatabase())
                {
                    _repository.Setup(e => e.DbContext).Returns(db);
                    var result = await sut.BuildBulkUploadMapping(stream);
                    // assert
                    result.Failed.Should().BeFalse();
                    result.Value.Model.RegistrationTypeStatus.Should().Be(BulkUploadRegistrationTypeStatus.UseDefault);
                    result.Value.Model.RegistrationTypeColumnIndex = null;
                }
            }
        }

        [Fact]
        public async Task When_ValidFileWithoutRegTypeAnd1RegType_Expect_SelectRegTypeStatus()
        {
            // arrange
            using (var db = GetAdminDatabase())
            {
                db.RegistrationTypes.Add(new RegistrationType());
                db.RegistrationTypes.Add(new RegistrationType());
                db.SaveChanges();
            }
            var sut = GetService();

            using (var stream = File.OpenRead(GetFilePath("valid_withoutRegistrationType")))
            {
                //act
                using (var db = GetAdminDatabase())
                {
                    _repository.Setup(e => e.DbContext).Returns(db);
                    var result = await sut.BuildBulkUploadMapping(stream);
                    // assert
                    result.Failed.Should().BeFalse();
                    result.Value.Model.RegistrationTypeStatus.Should().Be(BulkUploadRegistrationTypeStatus.PleaseSelect);
                    result.Value.Model.RegistrationTypeColumnIndex = null;
                }
            }
        }

        private string GetFilePath(string fileName, string ext = "xlsx") => $"./Domain/BulkUpload/MappingFiles/{fileName}.{ext}";
    }
}
