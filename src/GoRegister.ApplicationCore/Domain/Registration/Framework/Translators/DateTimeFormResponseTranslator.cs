using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Registration.Framework.Translators
{
    public class DateTimeFormResponseTranslator : IFormResponseTranslator<DateTime>
    {
        public Type Type => typeof(DateTime);

        public ResponseResult<DateTime> Process(IUpdateModel updateModel, string key)
        {
            if (updateModel.Form.ContainsKey(key))
            {
                var val = updateModel.Form[key];
                if (DateTime.TryParse(val, out DateTime parsedVal))
                {
                    return ResponseResult.Ok(parsedVal);
                }

                return ResponseResult.Fail<DateTime>();
            }

            return ResponseResult.Fail<DateTime>();
        }
    }
}
