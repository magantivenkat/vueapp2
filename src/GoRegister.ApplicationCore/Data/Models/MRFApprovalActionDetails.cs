using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Data.Models
{
    public class MRFApprovalActionDetails
    {
        [Key]
        public int MRFActionId { get; set; }
        public int MRFClinetResponseId { get; set; }
        public string UserGUID { get; set; }
        public string ClientGUID { get; set; }
        public int ProjectId { get; set; }
        public int FormId { get; set; }
        public string? ApproverEmailId { get; set; }
        public string? Status { get; set; }
        public string? Comments { get; set; }
        public DateTime? DateCreated
        {
            get;
            set;
        }


    }
}
