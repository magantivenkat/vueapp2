using GoRegister.ApplicationCore.Framework;
using GoRegister.ApplicationCore.Framework.Multitenancy;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Hangfire
{
    public interface IJob<T> where T : JobOptions
    {
        Task Execute(T options);
    }

    public abstract class JobOptions
    {
        public JobOptions(ProjectTenant projectTenant)
        {
            ProjectTenant = projectTenant;
        }

        public ProjectTenant ProjectTenant { get; }
    }

    public abstract class Job<T> : IJob<T> where T : JobOptions
    {
        protected readonly ITenantAccessor _tenantAccessor;

        public Job(ITenantAccessor tenantAccessor)
        {
            _tenantAccessor = tenantAccessor;
        }

        public async Task Execute(T options)
        {
            _tenantAccessor.SetTenant(options.ProjectTenant);
            await Handle(options);
        }

        protected abstract Task Handle(T options);
    }
}
