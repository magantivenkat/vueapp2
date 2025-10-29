using System;

namespace GoRegister.ApplicationCore.Domain.Registration.Framework
{
    public interface IFormDriverResult
    {
        string Partial { get; set; }
        IFieldDisplayModel Model { get; set; }
    }


}