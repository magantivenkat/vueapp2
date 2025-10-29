using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Registration.Framework.Translators
{
    public class IntFormResponseTranslator : IFormResponseTranslator<int>
    {
        public Type Type => typeof(int);

        public ResponseResult<int> Process(IUpdateModel updateModel, string key)
        {
            if(updateModel.Form.ContainsKey(key))
            {
                var val = updateModel.Form[key];
                if(int.TryParse(val, out int parsedVal))
                {
                    return ResponseResult.Ok(parsedVal);
                }

                return ResponseResult.Fail<int>();
            }

            return ResponseResult.Fail<int>();
        }
    }
}
