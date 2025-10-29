namespace GoRegister.ApplicationCore.Data.Models
{
    public class CustomPageRegistrationStatus
    {
        public int CustomPageId { get; set; }

        public int RegistrationStatusId { get; set; }

        public CustomPage CustomPage { get; set; }

        public RegistrationStatus RegistrationStatus { get; set; }
    }
}
