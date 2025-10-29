using System.Threading;
using System.Threading.Tasks;
using GoRegister.ApplicationCore.Domain.BulkUpload.Models;
using GoRegister.ApplicationCore.Domain.BulkUpload.Services;
using GoRegister.ApplicationCore.Domain.Settings.Services;
using GoRegister.ApplicationCore.Services.FileStorage;
using MediatR;
using Microsoft.AspNetCore.Hosting;

namespace GoRegister.ApplicationCore.Domain.BulkUpload.Queries
{
    public class ValidateUploadQuery : IRequest<BulkUploadValidationResult>
    {       
        public BulkUploadMappingModel Model { get; set; }

        public ValidateUploadQuery(BulkUploadMappingModel model)
        {
            Model = model;           
        }

        public class Handler : IRequestHandler<ValidateUploadQuery, BulkUploadValidationResult>
        {
            private readonly IBulkUploadService _bulkUploadService;            
            private readonly IFileStorage _fileStorage;            

            public Handler(IBulkUploadService bulkUploadService, IFileStorage fileStorage)
            {
                _bulkUploadService = bulkUploadService;                
                _fileStorage = fileStorage;                
            }

            public async Task<BulkUploadValidationResult> Handle(ValidateUploadQuery request, CancellationToken cancellationToken)
            {                                               
                using (var stream = await _fileStorage.ReadFile(request.Model.UploadId))
                {
                    return await _bulkUploadService.ValidateBulkUpload(request.Model, stream);
                    
                }
            }
        }
    }
}
