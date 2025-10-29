namespace GoRegister.ApplicationCore.Domain.Registration.Models
{
    public class ContactListItemModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name => $"{FirstName} {LastName}";
        public string Email { get; set; }
        //public string RegistrationType { get; set; }
        //public string RegistrationStatus { get; set; }
        //public string Action { get; set; }
    }
}
