using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Registration.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoRegister.ApplicationCore.Domain.Registration.Framework.Builder
{
    public interface IFormBuilderModel
    {
        Form Form { get; }
        List<Field> Fields { get;  }
        List<FormRuleModel> Rules { get;  }
        List<RegistrationPage> Pages { get;  }
        bool IsAdmin { get;  }
    }
}
