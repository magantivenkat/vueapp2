using System;

namespace GoRegister.ApplicationCore.Domain.Projects.Models
{
    public class ProjectSettingsLiquidModel
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Tel { get; set; }
        public string EmailReplyTo { get; set; }
    }
}
