using System;
using System.Collections.Generic;
using System.Text;

namespace GoRegister.ApplicationCore.Domain.Reports.Models
{
    public class GeneratedReportViewModel
    {
        public IEnumerable<IDictionary<string, object>> Results { get; set; }
        public List<IEnumerable<object>> Table { get; set; }
        public long QueryTimeElapsed { get; set; }
        public IEnumerable<string> Headers { get; set; }
    }
}
