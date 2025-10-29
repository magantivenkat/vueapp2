namespace GoRegister.ApplicationCore.Data.Models
{
    public class CustomPageRegistrationType
    {
        public int CustomPageId { get; set; }

        public int RegistrationTypeId { get; set; }

        public CustomPage CustomPage { get; set; }

        public RegistrationType RegistrationType { get; set; }
    }
}
