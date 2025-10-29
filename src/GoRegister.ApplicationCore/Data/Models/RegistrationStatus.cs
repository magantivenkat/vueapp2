using System.Collections.Generic;

namespace GoRegister.ApplicationCore.Data.Models
{
    public class RegistrationStatus
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public List<CustomPageRegistrationStatus> CustomPageRegistrationStatuses { get; set; } = new List<CustomPageRegistrationStatus>();
    }
}
