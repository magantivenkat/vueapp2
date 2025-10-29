using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoRegister.ApplicationCore.Data.Models
{
    public class MRFClientRequestCountry
    {
        [Key]
        public int CountryID
        {
            get;
            set;
        }

        public string CountryName
        {
            get;
            set;
        }

        public string Countryguid
        {
            get;
            set;
        }

        // Foreign key   
        [Display(Name = "ClientuniqueID")]
        public virtual int ClientuniqueID
        {
            get;
            set;
        }

        [ForeignKey("ClientuniqueID")]
        public virtual MRFClientRequest MRFClientRequest { get; set; }
    }
}
