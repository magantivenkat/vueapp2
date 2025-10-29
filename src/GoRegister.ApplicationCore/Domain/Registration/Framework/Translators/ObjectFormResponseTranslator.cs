using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Registration.Framework.Translators
{
    public class ObjectFormResponseTranslator<T> : IFormResponseTranslator<T> where T : class, new()
    {
        public Type Type => typeof(T);

        public ResponseResult<T> Process(IUpdateModel updateModel, string key)
        {
            var model = new T();
            Task.Run(async () => await updateModel.TryUpdateModelAsync(model, key));
            return ResponseResult.Ok(model);
        }
    }
}
