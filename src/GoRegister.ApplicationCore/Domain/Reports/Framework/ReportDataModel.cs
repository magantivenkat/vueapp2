using System;
using System.Collections.Generic;
using System.Text;

namespace GoRegister.ApplicationCore.Domain.Reports.Framework
{
    public class ReportDataModel
    {
        public IEnumerable<IDictionary<string, object>> Results { get; set; }
        public Dictionary<string, string> Headers { get; set; }
    }
}
