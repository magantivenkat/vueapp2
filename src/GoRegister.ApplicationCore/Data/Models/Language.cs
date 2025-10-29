using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoRegister.ApplicationCore.Data.Models
{
    public class Language
    {
        [Key]
        public int Id
        {
            get;
            set;
        }


        [MaxLength(50)]
        public string LanguageName
        {
            get;
            set;
        }

        [MaxLength(5)]
        public string LanguageCode
        {
            get;
            set;
        }

        public bool IsActive
        {
            get;
            set;
        }

        public int SortOrder
        {
            get;
            set;
        }

        public ICollection<MRFLanguage> MRFLanguages { get; set; }
    }

}
