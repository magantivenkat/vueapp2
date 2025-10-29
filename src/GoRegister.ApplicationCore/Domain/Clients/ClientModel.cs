using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Countries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GoRegister.ApplicationCore.Domain.Clients
{
    public class ClientModel
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "Please Enter Client Name")]
        [Display(Name = "Client Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please Enter Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please Enter Valid Email")]
        [Display(Name = "Client Email")]
        public string Email { get; set; }

        public List<ClientEmail> ClientEmails { get; set; }

        public DateTime DateCreated { get; set; }

        public string FormAction { get; set; }

        public List<TPNCountryClientEmail> TPNCountryClientEmails { get; set; }
        
        public string ClientUuid { get; set; }
        public int? ModifiedBy { get; set; }

        public List<TPNClientGAMappingModel> TPNClientGAMappings { get; set; }

        public List<TPNCountryAdminMapping> TPNCountryAdminMapping { get; set; }

    }
}
