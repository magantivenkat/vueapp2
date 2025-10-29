using GoRegister.ApplicationCore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Data
{
    public interface IMayHaveProject
    {
        int? ProjectId { get; set; }
        Project Project { get; set; }
    }

    public abstract class MayHaveProjectEntity : IMayHaveProject
    {
        public int? ProjectId { get; set; }
        public virtual Project Project { get; set; }
    }
}
