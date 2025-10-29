using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Registration.Framework
{
    public class FormDriverResult : IFormDriverResult 
    {
        public FormDriverResult(string partial, IFieldDisplayModel model)
        {
            Partial = partial;
            Model = model;
        }

        public string Partial { get; set; }
        public IFieldDisplayModel Model { get; set; }
    }
}
