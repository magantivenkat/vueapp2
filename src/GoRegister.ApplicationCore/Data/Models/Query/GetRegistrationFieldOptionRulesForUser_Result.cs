using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Data.Models.Query
{
    public class GetRegistrationFieldOptionRulesForUser_Result
    {
        public int Id { get; set; }
        public int FieldOptionId { get; set; }
        public Nullable<int> NextFieldId { get; set; }
        public Nullable<int> NextFieldOptionId { get; set; }
        public int FieldOptionFieldId { get; set; }
    }
}
