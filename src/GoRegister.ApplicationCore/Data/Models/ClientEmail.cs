namespace GoRegister.ApplicationCore.Data.Models
{
    public class ClientEmail
    {
        public int Id { get; set; }
        public string Email { get; set; }

        public Client Client{ get; set; }
    }
}
