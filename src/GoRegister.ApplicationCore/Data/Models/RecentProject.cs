using System;

namespace GoRegister.ApplicationCore.Data.Models
{
    public class RecentProject
    {
        public int Id { get; set; }
        public DateTime DateVisited { get; set; } = SystemTime.UtcNow;
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
