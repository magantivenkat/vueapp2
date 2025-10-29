using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoRegister.ApplicationCore.Data.Models
{
    public class MRFLanguage
    {
        [Key]
        public int Id
        {
            get;
            set;
        }

        // Foreign key
        public int ProjectId
        {
            get;
            set;
        }

        // Foreign key   
        public int LanguageId
        {
            get;
            set;
        }

        // Navigation property
        [ForeignKey("LanguageId")]
        public Language Language { get; set; }

        // Navigation property
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        public ICollection<MRFLanguageResource> MRFLanguageResources { get; set; }

    }
}
