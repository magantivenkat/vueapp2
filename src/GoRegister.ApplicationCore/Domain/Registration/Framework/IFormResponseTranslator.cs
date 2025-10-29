using System;

namespace GoRegister.ApplicationCore.Domain.Registration.Framework
{
    public interface IFormResponseTranslator<T>
    {
        Type Type { get; }
        ResponseResult<T> Process(IUpdateModel updateModel, string key);
    }
}
