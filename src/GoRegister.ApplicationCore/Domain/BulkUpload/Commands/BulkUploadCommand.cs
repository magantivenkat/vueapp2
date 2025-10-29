using System.Threading;
using System.Threading.Tasks;
using GoRegister.ApplicationCore.Domain.BulkUpload.Models;
using GoRegister.ApplicationCore.Domain.BulkUpload.Services;
using GoRegister.ApplicationCore.Services.FileStorage;
using MediatR;
using Microsoft.AspNetCore.Hosting;

namespace GoRegister.ApplicationCore.Domain.BulkUpload.Commands
{
    public class BulkUploadCommand : IRequest<BulkUploadExecutionResult>
    {
        public BulkUploadMappingModel Model { get; set; }

        public BulkUploadCommand(BulkUploadMappingModel model)
        {
            Model = model;
        }

        public class Handler : IRequestHandler<BulkUploadCommand, BulkUploadExecutionResult>
        {
            private readonly IBulkUploadService _bulkUploadService;            
            private readonly IFileStorage _fileStorage;

            public Handler(IBulkUploadService bulkUploadService, IFileStorage fileStorage)
            {
                _bulkUploadService = bulkUploadService;               
                _fileStorage = fileStorage;
            }

            public async Task<BulkUploadExecutionResult> Handle(BulkUploadCommand request, CancellationToken cancellationToken)
            {                
                using (var stream = await _fileStorage.ReadFile(request.Model.UploadId))
                {
                    return await _bulkUploadService.ExecuteBulkUpload(request.Model, stream);
                }
            }
        }
    }
}
