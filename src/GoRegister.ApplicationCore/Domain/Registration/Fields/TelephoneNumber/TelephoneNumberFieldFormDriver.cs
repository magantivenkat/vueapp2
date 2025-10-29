using GoRegister.ApplicationCore.Domain.Registration.Framework;
using System;
using System.Threading.Tasks;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Translators;
using OfficeOpenXml;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Models;

namespace GoRegister.ApplicationCore.Domain.Registration.Fields.TelephoneNumber
{
    public class TelephoneFieldFormDriver : FormDriverBase<TelephoneField, TelephoneFieldInputModel>
    {
        protected override string EditorName => "Phone Number";

        public override Data.Enums.FieldTypeEnum FieldType => Data.Enums.FieldTypeEnum.PhoneNumber;
        public override IFormResponseTranslator<TelephoneFieldInputModel> FormResponseTranslator => new ObjectFormResponseTranslator<TelephoneFieldInputModel>();

        public override DataStorageStrategyEnum StorageStrategy => throw new NotImplementedException();

        public override async Task<IFormDriverResult> Display(TelephoneField field, FieldDisplayContext context, int projectId = 0)
        {
            var model = new TelephoneFieldDisplayModel();
            return new FormDriverResult("_RegistrationFormTelephone", model);
        }

        protected override void Process(TelephoneField field, ResponseResult<TelephoneFieldInputModel> response, FieldResponseContext context)
        {

        }

        public override ResponseResult<TelephoneFieldInputModel> GetExcelResponse(TelephoneField field, ExcelRange excelRange)
        {
            throw new NotImplementedException();
        }

        public override object BuildUserResponses(UserResponseContext context)
        {
            throw new NotImplementedException();
        }
    }
}
