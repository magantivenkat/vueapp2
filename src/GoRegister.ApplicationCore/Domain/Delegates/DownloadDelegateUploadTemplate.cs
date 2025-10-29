using GoRegister.ApplicationCore.Domain.Registration.Services;
using MediatR;
using OfficeOpenXml;
using System.Data;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Delegates
{
    public static class DownloadDelegateUploadTemplate
    {
        public class Query : IRequest<MemoryStream> { }

        public class QueryHandler : IRequestHandler<Query, MemoryStream>
        {
            private readonly IFormService _formService;

            public QueryHandler(IFormService formService)
            {
                _formService = formService;
            }

            public async Task<MemoryStream> Handle(Query request, CancellationToken cancellationToken)
            {
                DataTable table = new DataTable();

                var form = await _formService.GetRegistrationForm();
                foreach (var field in form.Fields)
                {
                    if (!table.Columns.Contains(field.Name))
                    {
                        table.Columns.Add(field.Name, typeof(string));
                    }
                    else if (!string.IsNullOrEmpty(field.DataTag) && !table.Columns.Contains(field.DataTag))
                    {
                        table.Columns.Add(field.DataTag, typeof(string));
                    }
                }

                var result = new StringBuilder();
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    result.Append(table.Columns[i].ColumnName);
                    result.Append(i == table.Columns.Count - 1 ? "\n" : ",");
                }

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Delegate List");
                    worksheet.Cells["A1"].LoadFromDataTable(table, PrintHeaders: true);
                    for (var col = 1; col < table.Columns.Count + 1; col++)
                    {
                        worksheet.Column(col).AutoFit();
                    }
                    return new MemoryStream(package.GetAsByteArray());
                }
            }
        }
    }
}
