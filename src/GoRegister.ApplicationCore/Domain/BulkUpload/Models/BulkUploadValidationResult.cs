using System.Collections.Generic;
using System.Linq;

namespace GoRegister.ApplicationCore.Domain.BulkUpload.Models
{
    public class BulkUploadValidationResult
    {
        public bool IsValid => !Errors.Any();
        public List<BulkUploadValidationError> Errors { get; set; } = new List<BulkUploadValidationError>();

        public void AddError(string message)
        {
            Errors.Add(new BulkUploadValidationError
            {
                Message = message
            });
        }

        public void AddError(string message, List<string> details)
        {
            Errors.Add(new BulkUploadValidationError
            {
                Message = message,
                ErrorDetails = details
            });
        }
    }

    public class BulkUploadValidationError
    {
        public string Message { get; set; }
        public List<string> ErrorDetails { get; set; } = new List<string>();
    }
}
