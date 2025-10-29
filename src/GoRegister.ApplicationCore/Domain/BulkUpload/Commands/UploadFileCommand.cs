using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using GoRegister.ApplicationCore.Domain.BulkUpload.Models;
using GoRegister.ApplicationCore.Domain.BulkUpload.Services;
using GoRegister.ApplicationCore.Domain.Settings.Services;
using GoRegister.ApplicationCore.Extensions;
using GoRegister.ApplicationCore.Framework.Domain;
using GoRegister.ApplicationCore.Services.FileStorage;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace GoRegister.ApplicationCore.Domain.BulkUpload.Commands
{
    public class UploadFileCommand : IRequest<Result<ResponseContainer<BulkUploadMappingModel, BulkUploadMappingViewModel>>>
    {
        public IFormFile File { get; set; }

        public class Handler : IRequestHandler<UploadFileCommand, Result<ResponseContainer<BulkUploadMappingModel, BulkUploadMappingViewModel>>>
        {
            private readonly IBulkUploadService _bulkUploadService;
            private readonly IProjectSettingsAccessor _projectSettingsAccessor;
            private readonly IFileStorage _fileStorage;
            private readonly IConfiguration _configuration;

            public Handler(IBulkUploadService bulkUploadService, IProjectSettingsAccessor projectSettingsAccessor, IFileStorage fileStorage, IConfiguration configuration)
            {
                _bulkUploadService = bulkUploadService;
                _projectSettingsAccessor = projectSettingsAccessor;
                _fileStorage = fileStorage;
                _configuration = configuration;
            }

            public async Task<Result<ResponseContainer<BulkUploadMappingModel, BulkUploadMappingViewModel>>> Handle(UploadFileCommand request, CancellationToken cancellationToken)
            {
                Result<ResponseContainer<BulkUploadMappingModel, BulkUploadMappingViewModel>> result;
                using (var stream = new MemoryStream())
                {
                    await request.File.CopyToAsync(stream);

                    result = await _bulkUploadService.BuildBulkUploadMapping(stream);
                    if (result.Failed)
                    {
                        return result;
                    }
                }

                var fileName = $"${Guid.NewGuid()}{Path.GetExtension(request.File.FileName)}";

                UploadFileStorageSettings storageSettings = new UploadFileStorageSettings { IsPrivate = true };
                var parentFolder = _configuration.GetValue<string>("AwsS3BulkUpload:ParentFolder");
                var subFolder = _configuration.GetValue<string>("AwsS3BulkUpload:SubFolder");

                var proj = await _projectSettingsAccessor.GetAsync();
                var path = StringExtensions.CombineWithSlash(
                    parentFolder,
                    proj.UniqueId.ToString(),
                    subFolder,
                    fileName);               

                using (var stream = new MemoryStream())
                {
                    await request.File.CopyToAsync(stream);
                    var uploadresult = await _fileStorage.UploadFile(path, stream, storageSettings);
                }

                result.Value.Model.UploadId = path;
                return result;
            }
        }
    }
}
