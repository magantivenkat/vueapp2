namespace GoRegister.ApplicationCore.Services.FileStorage.S3
{
    public class S3Settings
    {
        public string BucketName { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string RegionEndpoint { get; set; }
        public string RegionDetail { get; set; }
        public string BucketMapURL { get; set; }
    }
}
