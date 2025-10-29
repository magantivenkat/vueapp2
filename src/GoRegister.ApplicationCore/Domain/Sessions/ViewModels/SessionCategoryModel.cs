using System;
using System.Collections.Generic;
using System.Text;

namespace GoRegister.ApplicationCore.Domain.Sessions.ViewModels
{
    public class SessionCategoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsSingleSession { get; set; }

        public int ProjectId { get; set; }
    }
}
