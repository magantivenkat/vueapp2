using System.Collections.Generic;

namespace GoRegister.ApplicationCore.Domain.Registration.Fields.Session
{
    public class SessionFieldDisplayModel : BaseSingleFieldDisplayModel
    {
        public IEnumerable<SessionItem> Sessions { get; set; } = new List<SessionItem>();
        public override string SummaryTemplate { get; set; } = "SessionValueSummary";
        public string ReserveSessionUrl { get; set; }
        public bool IsAdmin { get; set; }
    }

    public class SessionItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DateStart { get; set; }
        public string DateEnd { get; set; }
        public int? SessionCategoryId { get; set; }
        public bool IsFull { get; set; }
        public bool IsSingleSession { get; set; }
        public string SessionCategoryName { get; internal set; }
        public List<int> RegTypeIds { get; set; }
    }
}
