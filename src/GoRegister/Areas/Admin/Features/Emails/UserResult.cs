namespace GoRegister.Areas.Admin.Features.Emails
{
    public class UserResult
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string Email { get; set; }
        public int ProjectId { get; set; }
        public string RegistrationStatus { get; set; }
        public string RegistrationType { get; set; }
    }
}
