namespace GoRegister.ApplicationCore.Domain.Clients
{
    public class ClientEmailModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public ClientModel Client { get; set; }
    }
}
