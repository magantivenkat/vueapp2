namespace GoRegister.ApplicationCore.Domain.AdminUsers
{
    public class AdminUserListItemModel
    {
        public int Id { get; set; }
        public string Name => $"{FirstName} {LastName}";
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
