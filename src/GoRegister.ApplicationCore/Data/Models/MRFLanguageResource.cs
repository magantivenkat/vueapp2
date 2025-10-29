
using Microsoft.Build.Framework;

namespace GoRegister.ApplicationCore.Data.Models
{
    public class MRFLanguageResource
    {
        public int Id { get; set; }
        public int MRFLanguageId { get; set; }
        public int FieldId { get; set; }

        [Required]
        public string LanguageResource { get; set; }

        public MRFLanguage MRFLanguage { get; set; }
        public Field Field { get; set; }
    }
}
