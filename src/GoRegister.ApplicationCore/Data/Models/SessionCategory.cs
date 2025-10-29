using System.Collections.Generic;

namespace GoRegister.ApplicationCore.Data.Models
{
    public class SessionCategory : MustHaveProjectEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsSingleSession { get; set; }

        //public Project Project { get; set; }
        public List<Session> Sessions { get; set; }
        public ICollection<SessionFieldCategory> SessionFieldCategories { get; set; }
    }
}
