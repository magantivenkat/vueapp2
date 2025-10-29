using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace GoRegister.ApplicationCore.Domain.ProjectThemes.Models
{
    public class FileManagerEntryModel
    {
        public string Name { get; set; }
        public long Size { get; set; }
        public string Path { get; set; }
        public string Extension { get; set; }
        public bool IsDirectory { get; set; }
        public bool HasDirectories { get; set; }
        public DateTime Created { get; set; }

        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}")]
        public DateTime CreatedUtc { get; set; }
        public DateTime Modified { get; set; }
        public DateTime ModifiedUtc { get; set; }
    }
}
