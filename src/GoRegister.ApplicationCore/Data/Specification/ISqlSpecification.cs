using System;
using System.Collections.Generic;
using System.Text;

namespace GoRegister.ApplicationCore.Data.Specification
{
    public interface ISqlSpecification<T>
    {
        string Sql { get; }
        object Parameters { get; }
    }
}
