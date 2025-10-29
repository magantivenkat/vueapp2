using System.Collections.Generic;
using System.Linq;

namespace GoRegister.ApplicationCore.Data.Models
{
    public class SessionField : Field
    {
        public bool HideFullSessions { get; set; }
        public ICollection<SessionFieldCategory> SessionFieldCategories { get; set; }

        public IEnumerable<int> GetCategoryIds()
        {
            if (SessionFieldCategories == null || !SessionFieldCategories.Any()) return new List<int>();

            return SessionFieldCategories.Select(e => e.SessionCategoryId);
        }
    }
}
