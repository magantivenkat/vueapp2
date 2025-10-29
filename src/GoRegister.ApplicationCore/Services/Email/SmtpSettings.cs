namespace GoRegister.ApplicationCore.Services.Email
{
    public class SmtpSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool UseSsl { get; set; }
        public bool UseAuthentication { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
