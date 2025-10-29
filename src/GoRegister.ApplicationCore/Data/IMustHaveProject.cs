using GoRegister.ApplicationCore.Data.Models;

namespace GoRegister.ApplicationCore.Data
{
    public interface IMustHaveProject
    {
        int ProjectId { get; set; }
        Project Project { get; set; }
    }

    public abstract class MustHaveProjectEntity : IMustHaveProject
    {
        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }
    }
    public interface IMustHaveProjectForEmail
    {
        int ProjectId { get; set; }
        Project Project { get; set; }
    }

    public abstract class MustHaveProjectEntityForEmail : IMustHaveProjectForEmail
    {
        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }
    }
}
