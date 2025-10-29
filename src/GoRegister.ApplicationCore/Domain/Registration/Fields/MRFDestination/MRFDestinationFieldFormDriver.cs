/*MRF Changes: Destination
  Modified Date: 11th November 2022
  Modified By : Mandar.Khade @amexgbt.com
 Team member : Harish.Rane @amexgbt.com
  JIRA Ticket No : GoRegister/GOR-240-new01 */



using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Registration.Framework;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Translators;
using System.Threading.Tasks;
using GoRegister.ApplicationCore.Data.Enums;
using OfficeOpenXml;
using GoRegister.ApplicationCore.Domain.Registration.Fields.Text;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Models;
using GoRegister.ApplicationCore.Data.Models.Fields;

namespace GoRegister.ApplicationCore.Domain.Registration.Fields.MRFDestination
{
    public class MRFDestinationFieldFormDriver : InputFormDriverBase<MRFDestinationField, MRFDestinationFieldEditorModel, string, MRFDestinationFieldDisplayModel>
    {
        protected override string EditorName => "Destination";
        protected override string OverrideName => "Destination";
        protected override string DataTagFixed => "DestinationExternalId";
        public override FieldTypeEnum FieldType => FieldTypeEnum.MRFDestination;
        protected override bool IsUnique => true;

        protected override string EditorTemplate => "";

        public override IFormResponseTranslator<string> FormResponseTranslator => new StringFormResponseTranslator();

        public override DataStorageStrategyEnum StorageStrategy => DataStorageStrategyEnum.String;

        public override Task<MRFDestinationFieldDisplayModel> Display(MRFDestinationFieldDisplayModel model, MRFDestinationField field, FieldDisplayContext context, int projectId = 0)
        {
            model.Type = "MRFDestination";
            return Task.FromResult(model);
        }

        //protected override void Process(MRFDestinationField field, ResponseResult<string> response, FieldResponseContext context)
        //{
        //    if (response.HasValue)
        //        context.SetStringValue(response.Value);
        //    else
        //        context.ClearStringValue();
        //}

        protected override void Process(MRFDestinationField field, ResponseResult<string> responseMRF, FieldResponseContext fieldResponseContextMRF)
        {
            if (responseMRF.HasValue)
                fieldResponseContextMRF.SetStringValue(responseMRF.Value);
            else
                fieldResponseContextMRF.ClearStringValue();
        }

        public override ResponseResult<string> GetExcelResponse(MRFDestinationField field, ExcelRange excelRange)
        {
            if (string.IsNullOrWhiteSpace(excelRange.Text))
            {
                return ResponseResult.Empty<string>();
            }

            return ResponseResult.Ok(excelRange.Text?.Trim());
        }

        //public override object BuildUserResponses(UserResponseContext context)
        //{
        //    var foResult = context.Response.GetStringValue(context.FieldId);
        //    if (foResult)
        //        return foResult.Value;

        //    return null;
        //}

        public override object BuildUserResponses(UserResponseContext userResponseContextMRF)
        {
            var foResult = userResponseContextMRF.UserFormResponseMRF.GetStringValue(userResponseContextMRF.FieldId);
            if (foResult)
                return foResult.Value;

            return null;
        }



    }
}
