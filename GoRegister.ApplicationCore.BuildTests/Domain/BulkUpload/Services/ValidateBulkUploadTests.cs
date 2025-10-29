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
using GoRegister.ApplicationCore.Domain.BulkUpload.Specs;
using OfficeOpenXml;
using System.Linq;
using GoRegister.ApplicationCore.Domain.Delegates;

namespace GoRegister.ApplicationCore.BuildTests.Domain.BulkUpload.Services
{
    public class ValidateBulkUploadTests : DatabaseContextTest
    {
        private readonly Mock<IRepository> _repository = new Mock<IRepository>();
        private readonly Mock<IFormService> _formService = new Mock<IFormService>();

        private readonly Mock<IFormDriver> _mockEmailFieldDriver = new Mock<IFormDriver>();
        private readonly Mock<IFormDriver> _mockTextFieldDriver = new Mock<IFormDriver>();
        private readonly Mock<IAttendeeIdentifierService> _attendeeIdentifierService = new Mock<IAttendeeIdentifierService>();

        private readonly IFieldDriverAccessor _fieldDriverAccessor;

        private readonly BulkUploadMappingModel _validMappingModel = new BulkUploadMappingModel();

        public ValidateBulkUploadTests()
        {
            _formService.Setup(fs => fs.GetRegistrationForm()).ReturnsAsync(new FormBuilderModel
            {
                Form = new Form { Id = 1 },
                Fields = new List<Field>
                {
                    new EmailField { Id = 1, FieldTypeId = Data.Enums.FieldTypeEnum.Email, Name = "Email Address", DataTag = "Email" },
                    new TextField { Id = 2, FieldTypeId = Data.Enums.FieldTypeEnum.Textbox, Name = "Text field", DataTag = "text"  }
                }
            });

            _repository.Setup(mock => mock.ToListAsync<RegistrationType>()).ReturnsAsync(new List<RegistrationType>
            {
                new RegistrationType { Id = 1, Name = "RegType1" },
                new RegistrationType { Id = 2, Name = "RegType2" },
                new RegistrationType { Id = 3, Name = "RegType3" },
            });

            _repository.Setup(mock => mock.SqlQueryAsync(It.IsAny<DuplicateEmailsSpecification>(), It.IsAny<object>())).ReturnsAsync(new List<string>());

            // email driver
            _mockEmailFieldDriver.Setup(mock => mock.FieldType).Returns(Data.Enums.FieldTypeEnum.Email);
            // text driver
            _mockTextFieldDriver.Setup(mock => mock.FieldType).Returns(Data.Enums.FieldTypeEnum.Textbox);

            _fieldDriverAccessor = new FieldDriverAccessor(new List<IFormDriver> { _mockEmailFieldDriver.Object, _mockTextFieldDriver.Object });

            // valid mapping model
            _validMappingModel.RegistrationTypeStatus = BulkUploadRegistrationTypeStatus.UseDefault;
            _validMappingModel.HeaderMappings.Add(new HeaderMapping { ColumnIndex = 4, FieldId = 1 });
            _validMappingModel.HeaderMappings.Add(new HeaderMapping { ColumnIndex = 3, FieldId = 2 });
        }

        private BulkUploadService GetService()
        {
            return new BulkUploadService(ProjectSettingsAccessor.Object, _repository.Object, _formService.Object, _fieldDriverAccessor, _attendeeIdentifierService.Object);
        }

        [Fact]
        public async Task When_EmailNotMapped_Expect_Invalid()
        {
            // arrange
            var mappingModel = new BulkUploadMappingModel();
            var sut = GetService();

            using (var stream = File.OpenRead(GetFilePath("valid")))
            {
                //act
                var result = await sut.ValidateBulkUpload(mappingModel, stream);

                // assert
                result.IsValid.Should().BeFalse();
                result.Errors.Should().HaveCount(1);
            }
        }

        [Fact]
        public async Task When_DuplicateEmails_Expect_Invalid()
        {
            // arrange
            ProjectSettings.AllowDuplicateEmails = false;
            var sut = GetService();

            using (var stream = File.OpenRead(GetFilePath("duplicate_emails")))
            {
                //act
                var result = await sut.ValidateBulkUpload(_validMappingModel, stream);

                // assert
                result.IsValid.Should().BeFalse();
            }
        }

        [Fact]
        public async Task When_DuplicateEmailsAndAllowDuplicates_Expect_Valid()
        {
            // arrange
            ProjectSettings.AllowDuplicateEmails = true;
            var sut = GetService();

            using (var stream = File.OpenRead(GetFilePath("duplicate_emails")))
            {
                //act
                var result = await sut.ValidateBulkUpload(_validMappingModel, stream);

                // assert
                result.IsValid.Should().BeTrue();
            }
        }

        [Fact]
        public async Task When_EmailsExistInSystem_Expect_Invalid()
        {
            // arrange
            ProjectSettings.AllowDuplicateEmails = false;
            var sut = GetService();

            _repository.Setup(mock => mock.SqlQueryAsync(It.IsAny<DuplicateEmailsSpecification>(), It.IsAny<object>())).ReturnsAsync(new List<string>() { "test@banks-sadler.com" });

            using (var stream = File.OpenRead(GetFilePath("emails_exist_in_project")))
            {
                //act
                var result = await sut.ValidateBulkUpload(_validMappingModel, stream);

                // assert
                result.IsValid.Should().BeFalse();
            }
        }

        [Fact]
        public async Task When_EmailsExistInSystemAndAllowDuplicateEmails_Expect_Valid()
        {
            // arrange
            ProjectSettings.AllowDuplicateEmails = true;
            var sut = GetService();

            _repository.Setup(mock => mock.SqlQueryAsync(It.IsAny<DuplicateEmailsSpecification>(), It.IsAny<object>())).ReturnsAsync(new List<string>() { "test@banks-sadler.com" });

            using (var stream = File.OpenRead(GetFilePath("emails_exist_in_project")))
            {
                //act
                var result = await sut.ValidateBulkUpload(_validMappingModel, stream);

                // assert
                result.IsValid.Should().BeTrue();
            }
        }

        [Fact]
        public async Task When_Valid_Expect_Valid_ProcessingSettingsPassedToFormDrivers()
        {
            // arrange
            ProjectSettings.AllowDuplicateEmails = true;
            var sut = GetService();


            using (var stream = File.OpenRead(GetFilePath("valid_single")))
            {
                //act
                var result = await sut.ValidateBulkUpload(_validMappingModel, stream);

                // assert
                _mockEmailFieldDriver.Verify(mock => mock.ValidateResponse(It.IsAny<EmailField>(), It.Is<FormResponseContext>(
                    frc => frc.SkipIsMandatoryCheck == true &&
                    frc.FormExecutionFrom == FormExecutionFrom.BulkUpload
                ), It.IsAny<FormValidationContext>(), It.IsAny<ExcelRange>()));
            }
        }

        [Fact]
        public async Task When_InvalidDriverResponse_Expect_Invalid()
        {
            // arrange
            ProjectSettings.AllowDuplicateEmails = true;
            var sut = GetService();

            _mockEmailFieldDriver.Setup(mock => mock.ValidateResponse(It.IsAny<Field>(), It.IsAny<FormResponseContext>(), It.IsAny<FormValidationContext>(), It.IsAny<ExcelRange>()))
                .Callback<Field, FormResponseContext, FormValidationContext, ExcelRange>((_, _1, validationContext, _2) =>
                {
                    validationContext.AddError("Error");
                });


            using (var stream = File.OpenRead(GetFilePath("valid_single")))
            {
                //act
                var result = await sut.ValidateBulkUpload(_validMappingModel, stream);

                // assert
                result.IsValid.Should().BeFalse();
                result.Errors.Should().HaveCount(1);
                var error = result.Errors.First();
                error.ErrorDetails.Should().HaveCount(1);
                error.ErrorDetails.First().Should().Be("D2: Error");
            }
        }

        [Fact]
        public async Task When_ValidDriverResponse_Expect_Valid()
        {
            // arrange
            var sut = GetService();

            _mockEmailFieldDriver.Setup(mock => mock.ValidateResponse(It.IsAny<Field>(), It.IsAny<FormResponseContext>(), It.IsAny<FormValidationContext>(), It.IsAny<ExcelRange>()));


            using (var stream = File.OpenRead(GetFilePath("valid_single")))
            {
                //act
                var result = await sut.ValidateBulkUpload(_validMappingModel, stream);

                // assert
                result.IsValid.Should().BeTrue();
            }
        }

        [Fact]
        public async Task When_UnmappedHeaders_Expect_OnlyProcessMappedHeaders()
        {
            const int GENDER_FIELD_ID = 3;

            // arrange
            _formService.Setup(fs => fs.GetRegistrationForm()).ReturnsAsync(new FormBuilderModel
            {
                Form = new Form { Id = 1 },
                Fields = new List<Field>
                {
                    new EmailField { Id = 1, FieldTypeId = Data.Enums.FieldTypeEnum.Email, Name = "Email Address", DataTag = "Email" },
                    new TextField { Id = 2, FieldTypeId = Data.Enums.FieldTypeEnum.Textbox, Name = "Text field", DataTag = "text"  },
                    new TextField { Id = GENDER_FIELD_ID, FieldTypeId = Data.Enums.FieldTypeEnum.Textbox, Name = "Gender", DataTag = "gender" }
                }
            });
            _validMappingModel.HeaderMappings.Add(new HeaderMapping
            {
                FieldId = null,
                ColumnIndex = 3 // the gender column in valid_single
            });
            var sut = GetService();


            using (var stream = File.OpenRead(GetFilePath("valid_single")))
            {
                //act
                var result = await sut.ValidateBulkUpload(_validMappingModel, stream);

                // assert that we never try to run the form driver for field Gender
                _mockTextFieldDriver.Verify(mock => mock.ValidateResponse(It.Is<TextField>(f => f.Id != GENDER_FIELD_ID),
                    It.IsAny<FormResponseContext>(), It.IsAny<FormValidationContext>(), It.IsAny<ExcelRange>()));
            }
        }

        [Fact]
        public async Task When_RegTypeFromSheetWithIncorrectName_Expect_Invalid()
        {
            // arrange
            _validMappingModel.RegistrationTypeStatus = BulkUploadRegistrationTypeStatus.UseFromUpload;
            _validMappingModel.RegistrationTypeColumnIndex = 6;
            var sut = GetService();

            using (var stream = File.OpenRead(GetFilePath("invalid_regtype")))
            {
                //act
                var result = await sut.ValidateBulkUpload(_validMappingModel, stream);

                // assert
                result.IsValid.Should().BeFalse();
                result.Errors.Should().HaveCount(1);
                result.Errors.First().Message.Should().Be("Row 2: This row does not have a valid Registration Type");
            }
        }

        [Fact]
        public async Task When_RegTypeFromSheetWithCasesAndSpaces_Expect_Valid()
        {
            // arrange
            _validMappingModel.RegistrationTypeStatus = BulkUploadRegistrationTypeStatus.UseFromUpload;
            _validMappingModel.RegistrationTypeColumnIndex = 6;
            var sut = GetService();

            using (var stream = File.OpenRead(GetFilePath("cases_and_spaces_regtype")))
            {
                //act
                var result = await sut.ValidateBulkUpload(_validMappingModel, stream);

                // assert
                result.IsValid.Should().BeTrue();
            }
        }

        [Theory]
        [InlineData("regtype1", 1)]
        [InlineData("regtype2", 2)]
        [InlineData("regtype3", 3)]
        public async Task When_RegTypeFromSheet_Expect_Valid_DelegateHasCorrectRegType(string fileName, int regTypeId)
        {
            // arrange
            _validMappingModel.RegistrationTypeStatus = BulkUploadRegistrationTypeStatus.UseFromUpload;
            _validMappingModel.RegistrationTypeColumnIndex = 6;
            var sut = GetService();

            using (var stream = File.OpenRead(GetFilePath(fileName)))
            {
                //act
                var result = await sut.ValidateBulkUpload(_validMappingModel, stream);

                // assert
                result.IsValid.Should().BeTrue();

                // verify reg type for delegate is correct
                //_mockEmailFieldDriver.Verify(mock => mock.ValidateResponse(It.IsAny<EmailField>(), It.Is<FormResponseContext>(
                //    frc => frc.Response.DelegateUser.RegistrationTypeId == regTypeId
                //), It.IsAny<FormValidationContext>(), It.IsAny<ExcelRange>()));

                _mockEmailFieldDriver.Verify(mock => mock.ValidateResponse(It.IsAny<EmailField>(), It.Is<FormResponseContext>(
                   frc => frc.UserFormResponseMRF.DelegateUser.RegistrationTypeId == regTypeId
               ), It.IsAny<FormValidationContext>(), It.IsAny<ExcelRange>()));
            }
        }

        [Fact]
        public async Task When_RegTypeSelect_Expect_Valid_DelegateHasCorrectRegType()
        {
            const int REG_TYPE_ID = 2;

            // arrange
            _validMappingModel.RegistrationTypeStatus = BulkUploadRegistrationTypeStatus.PleaseSelect;
            _validMappingModel.RegistrationTypeId = REG_TYPE_ID;
            var sut = GetService();

            using (var stream = File.OpenRead(GetFilePath("cases_and_spaces_regtype")))
            {
                //act
                var result = await sut.ValidateBulkUpload(_validMappingModel, stream);

                // assert
                result.IsValid.Should().BeTrue();

                // verify reg type for delegate is correct
                //_mockEmailFieldDriver.Verify(mock => mock.ValidateResponse(It.IsAny<EmailField>(), It.Is<FormResponseContext>(
                //    frc => frc.Response.DelegateUser.RegistrationTypeId == REG_TYPE_ID
                //), It.IsAny<FormValidationContext>(), It.IsAny<ExcelRange>()));

                _mockEmailFieldDriver.Verify(mock => mock.ValidateResponse(It.IsAny<EmailField>(), It.Is<FormResponseContext>(
                    frc => frc.UserFormResponseMRF.DelegateUser.RegistrationTypeId == REG_TYPE_ID
                ), It.IsAny<FormValidationContext>(), It.IsAny<ExcelRange>()));
            }
        }

        [Fact]
        public async Task When_UseRegTypeFromSheetButNoColumnSpecified_Expect_Invalid()
        {
            // arrange
            _validMappingModel.RegistrationTypeStatus = BulkUploadRegistrationTypeStatus.UseFromUpload;
            var sut = GetService();

            using (var stream = File.OpenRead(GetFilePath("cases_and_spaces_regtype")))
            {
                //act
                var result = await sut.ValidateBulkUpload(_validMappingModel, stream);

                // assert
                result.IsValid.Should().BeFalse();
            }
        }

        private string GetFilePath(string fileName, string ext = "xlsx") => $"./Domain/BulkUpload/ValidateFiles/{fileName}.{ext}";
    }
}
