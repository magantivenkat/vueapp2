using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Registration.Framework
{
    public interface IDelegateUserCache
    {
        int Id { get; set; }
        string RegistrationDocument { get; set; }
        Guid UniqueIdentifier { get; set; }
    }
}
