using System;
using System.Collections.Generic;
using System.Text;

namespace GoRegister.ApplicationCore.Data.Models.Emails
{
    public class EmailLayout : MustHaveProjectEntityForEmail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Html { get; set; }
    }
}
