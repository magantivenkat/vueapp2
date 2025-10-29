namespace GoRegister.ApplicationCore.Domain.BulkUpload.Models
{
    public class BulkUploadExecutionResult
    {
        public BulkUploadExecutionResult(BulkUploadValidationResult validationResult)
        {
            ValidationResult = validationResult;
        }

        public bool IsValid => ValidationResult.IsValid;

        public BulkUploadValidationResult ValidationResult { get; }
    }
}
