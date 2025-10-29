using System.ComponentModel.DataAnnotations;
namespace GoRegister.ApplicationCore.Domain.Clients
{
    public class CreateEditTPNClientEmailModel
    {
        public int Id { get; set; }
        public int ClientId { get; set; }

        [Required(ErrorMessage = "Please Enter Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please Enter Valid Email")]
        public string TPNEmail { get; set; }
        public string TPNCountry { get; set; }
        public string ClientUuid { get; set; }
        public int? ModifiedBy { get; set; }
        public string FormAction { get; set; }
    }
}
