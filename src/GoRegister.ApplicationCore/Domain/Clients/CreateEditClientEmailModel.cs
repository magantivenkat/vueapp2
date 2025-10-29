using System.ComponentModel.DataAnnotations;

namespace GoRegister.ApplicationCore.Domain.Clients
{
    public class CreateEditClientEmailModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Enter Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please Enter Valid Email")]    
        public string Email { get; set; }
        public int ClientId { get; set; }
        
        //public string TPNEmail { get; set; }
        //public string TPNCountry { get; set; }
    }
}
