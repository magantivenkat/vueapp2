using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GoRegister.ApplicationCore.Data.Models
{
    public class TPNCountryClientEmail
    {
        public int Id { get; set; }
       
        public string TPNEmail { get; set; }
     
        public string TPNCountry { get; set; }
        public DateTime DateModified { get; set; }
        public int? ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public int ClientId { get; set; }
        public string ClientUuid { get; set; }
        public Client Client { get; set; }
        //public Country Countries { get; set; }
      
    }
}
