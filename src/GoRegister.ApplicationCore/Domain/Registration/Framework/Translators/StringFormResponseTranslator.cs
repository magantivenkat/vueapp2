using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Registration.Framework.Translators
{
    public class StringFormResponseTranslator : IFormResponseTranslator<string>
    {
        public Type Type => typeof(string);

        public ResponseResult<string> Process(IUpdateModel updateModel, string key)
        {
            return ResponseResult.Ok<string>(updateModel.Form[key]);
        }
    }
}
