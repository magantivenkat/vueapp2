using GoRegister.ApplicationCore.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoRegister.ApplicationCore.Domain.Registration.Framework
{

    public interface IFormDriverAccessor
    {
        Dictionary<FieldTypeEnum, IFormDriver> DriverDictionary { get; }

        IFormDriver GetFormDriver(FieldTypeEnum fieldType);
    }

    public class FormDriverAccessor : IFormDriverAccessor
    {
        private readonly IEnumerable<IFormDriver> _formDrivers;

        public FormDriverAccessor(IEnumerable<IFormDriver> formDrivers)
        {
            _formDrivers = formDrivers;
            DriverDictionary = _formDrivers.ToDictionary(e => e.FieldType, e => e);
        }

        public Dictionary<FieldTypeEnum, IFormDriver> DriverDictionary { get; }

        public IFormDriver GetFormDriver(FieldTypeEnum fieldType)
        {
            return DriverDictionary[fieldType];
        }
    }
}
