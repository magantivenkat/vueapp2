using GoRegister.ApplicationCore.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Registration.Framework
{
    public interface IFieldDriverAccessor
    {
        Dictionary<FieldTypeEnum, IFormDriver> GetFormDrivers();
        IFormDriver GetFormDriver(FieldTypeEnum fieldType);
    }

    public class FieldDriverAccessor: IFieldDriverAccessor
    {
        private readonly IEnumerable<IFormDriver> _formDrivers;

        public FieldDriverAccessor(IEnumerable<IFormDriver> formDrivers)
        {
            _formDrivers = formDrivers;
        }

        public IFormDriver GetFormDriver(FieldTypeEnum fieldType)
        {
            return GetFormDrivers()[fieldType];
        }

        public Dictionary<FieldTypeEnum, IFormDriver> GetFormDrivers()
        {
            return _formDrivers.ToDictionary(e => e.FieldType, e => e);
        }
    }
}
