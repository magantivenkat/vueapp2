using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Registration.Framework;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Builder;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Registration.Fields
{
    public abstract class PresentationFieldDriver<TField, TFieldEditor> : FormDriverBase<TField, TFieldEditor, string> where TField : Field, new()
        where TFieldEditor : IFieldEditorModel, new()
    {
        public override bool IsForPresentation => true;
        public override IFormResponseTranslator<string> FormResponseTranslator => throw new NotImplementedException();

        public override DataStorageStrategyEnum StorageStrategy => throw new NotImplementedException();

        public override object BuildUserResponses(UserResponseContext context)
        {
            return null;
        }

        public override ResponseResult<string> GetExcelResponse(TField field, ExcelRange excelRange)
        {
            return null;
        }

        protected override void Process(TField field, ResponseResult<string> response, FieldResponseContext context)
        {
            throw new NotImplementedException();
        }
    }

    public class PresentationFieldDisplayModel : IFieldDisplayModel
    {
        public string Id { get; set; }
        public bool IsForPresentation => true;

        public string Key => Id;
        public string Type { get; set; }
        public string SummaryTemplate { get; set; }

        public FieldTypeEnum FieldTypeId { get; set; }

        public Dictionary<string, object> Validations => new Dictionary<string, object>();

        public IEnumerable<FieldRule> Rules { get; set; }
        public string PageId { get; set; }

        public void Build(Field field)
        {
            Id = field.GetRenderId();
            FieldTypeId = field.FieldTypeId;
        }
    }
}
