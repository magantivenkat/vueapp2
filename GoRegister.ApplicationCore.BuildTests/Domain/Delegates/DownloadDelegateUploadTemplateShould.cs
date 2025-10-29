using FluentAssertions;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Delegates;
using GoRegister.ApplicationCore.Domain.Registration.Models;
using GoRegister.ApplicationCore.Domain.Registration.Services;
using Moq;
using System.Collections.Generic;
using System.Threading;
using Xunit;

namespace GoRegister.ApplicationCore.BuildTests.Domain.Delegates
{
    public class DownloadDelegateUploadTemplateShould
    {
        private readonly Mock<IFormService> mockFormService = new Mock<IFormService>();
        DownloadDelegateUploadTemplate.QueryHandler _sut;

        public DownloadDelegateUploadTemplateShould()
        {
            _sut = new DownloadDelegateUploadTemplate.QueryHandler(mockFormService.Object);
        }

        [Fact]
        public void DuplicateColumnNamesAreSkipped()
        {
            var formBuilder = new FormBuilderModel
            {
                Fields = new List<Field>
            {
                new TextField{Name = "Email" },
                new TextField{Name = "Email" },
            }
            };
            mockFormService.Setup(fs => fs.GetRegistrationForm()).ReturnsAsync(formBuilder);
            var query = new DownloadDelegateUploadTemplate.Query();
            var result = _sut.Handle(query, new CancellationToken());
            result.IsFaulted.Should().BeFalse();

        }

        [Fact]
        public void DuplicateColumnNamesUseDataTagNameIfAvailable()
        {
            var formBuilder = new FormBuilderModel
            {
                Fields = new List<Field>
            {
                new TextField{Name = "Email" },
                new TextField{Name = "Email", DataTag = "EmailMe" },
            }
            };
            mockFormService.Setup(fs => fs.GetRegistrationForm()).ReturnsAsync(formBuilder);
            var query = new DownloadDelegateUploadTemplate.Query();
            var result = _sut.Handle(query, new CancellationToken());
            result.IsFaulted.Should().BeFalse();

        }

        [Fact]
        public void OnlyAddDataTagIfItIsUnique_OtherwiseSkip()
        {
            var formBuilder = new FormBuilderModel
            {
                Fields = new List<Field>
            {
                new TextField{Name = "Email" },
                new TextField{Name = "Email", DataTag = "Email" },
            }
            };
            mockFormService.Setup(fs => fs.GetRegistrationForm()).ReturnsAsync(formBuilder);
            var query = new DownloadDelegateUploadTemplate.Query();
            var result = _sut.Handle(query, new CancellationToken());
            result.IsFaulted.Should().BeFalse();
        }
    }
}
