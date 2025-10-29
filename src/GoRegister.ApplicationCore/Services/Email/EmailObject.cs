namespace GoRegister.ApplicationCore.Services.Email
{
    public class EmailObject
    {
        public string FromEmail { get; set; }

        public string FromEmailDisplayName { get; set; }

        public string To { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public string Cc { get; set; }

        public string Bcc { get; set; }
        public string[] AttachmentFilePaths { get; set; }
    }
}
