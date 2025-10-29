using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoRegister.ApplicationCore.Data.Models
{
    public class MRFServiceCountryMapping : MustHaveProjectEntity
    {
        [Key]
        public int MappingCountryId 
        {
            get;
            set;
        }

        public string ClientUuid
        {
            get;
            set;
        }

        public string ServiceCountry
        {
            get;
            set;
        }

        public string ServiceCountryUuid
        {
            get;
            set;
        }

        public string RequestCountry
        {
            get;
            set;
        }

        public string RequestCountryId
        {
            get;
            set;
        }

        public DateTime? DateCreated
        {
            get;
            set;
        }

        public int? CreatedBy { 
            get; set; 
        }

        public bool IsActive { get; set; }
        public bool IsDefault { get; set; }
    }
}
