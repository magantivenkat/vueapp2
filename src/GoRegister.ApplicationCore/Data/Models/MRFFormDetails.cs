using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GoRegister.ApplicationCore.Data.Models
{
    [NotMapped]
    public class MRFFormDetails
    {
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
        public string FieldDataTag { get; set; }
    }
}
