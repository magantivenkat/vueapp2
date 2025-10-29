namespace GoRegister.ApplicationCore.Data.Models
{
    public class CustomPageAuditRegistrationType
    {
        public int Id { get; set; }

        public int CustomPageAuditId { get; set; }

        public CustomPageAudit CustomPageAudit { get; set; }

        public string Name { get; set; }
    }
}