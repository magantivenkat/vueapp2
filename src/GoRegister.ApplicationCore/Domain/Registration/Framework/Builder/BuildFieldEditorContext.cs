using AutoMapper;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Registration.Framework.Builder
{
    public class BuildFieldEditorContext
    {
        public JToken Model { get; set; }
        public IMapper Mapper { get; set; }
    }
}
