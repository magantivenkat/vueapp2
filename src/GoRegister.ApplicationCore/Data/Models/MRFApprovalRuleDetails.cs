using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GoRegister.ApplicationCore.Data.Models
{
    public class MRFApprovalRuleDetails : MustHaveProjectEntity
    {

        [Key]
        public int MRFApprovalId
        {
            get;
            set;
        }

        public string ClientUuid
        {
            get;
            set;
        }

        public int ApprovalFieldId
        {
            get;
            set;
        }

        public string ApproverEmailIds
        {
            get;
            set;
        }

        public string ApprovalRuleDetails
        {
            get;
            set;
        }

        public DateTime? DateCreated
        {
            get;
            set;
        }

        public int? CreatedBy
        {
            get; set;
        }

        public DateTime? DateModified
        {
            get;
            set;
        }

        public int? ModifiedBy
        {
            get; set;
        }

        public bool IsActive { get; set; }
        public bool IsDefault { get; set; }

    }
}
