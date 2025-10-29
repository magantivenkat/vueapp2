using GoRegister.ApplicationCore.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;



namespace GoRegister.ApplicationCore.Domain.Clients
{
    public class TPNCountryClientEmailModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Enter TPN Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please Enter Valid TPN Email")]
        [Display(Name = "TPN Country Client Email")]
        public string TPNEmail { get; set; }
        [Required(ErrorMessage = "Please Enter TPN Country")]
        public string TPNCountry { get; set; }
        public DateTime DateModified { get; set; }
        public int? ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public int ClientId { get; set; }
        public string ClientUuid { get; set; }
        public ClientModel Client { get; set; }
        public List<SelectListItem> TPNCountryList { get; set; }
        public string FormAction { get; set; }

    }
}
