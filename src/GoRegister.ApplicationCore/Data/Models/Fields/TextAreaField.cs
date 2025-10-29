using System.ComponentModel.DataAnnotations.Schema;

namespace GoRegister.ApplicationCore.Data.Models
{
    public class TextAreaField : Field
    {
      public string Placeholder { get; set; }
      
    }
}
