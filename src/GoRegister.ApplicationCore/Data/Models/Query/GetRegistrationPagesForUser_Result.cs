using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Data.Models.Query
{
    public partial class GetRegistrationPagesForUser_Result
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int SortOrder { get; set; }
        public int RegistrationPageTypeId { get; set; }
        public System.Guid UniqueIdentifier { get; set; }
        public Nullable<int> ReaderUserTypeId { get; set; }
        public Nullable<int> WriterUserTypeId { get; set; }
        public bool IsInternalOnly { get; set; }
    }
}
