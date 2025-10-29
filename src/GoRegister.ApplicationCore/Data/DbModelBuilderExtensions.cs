using Microsoft.EntityFrameworkCore;

namespace GoRegister.ApplicationCore.Data
{
    public static class DbModelBuilderExtensions
    {
        public static ModelBuilder ApplyGoRegisterConfiguration(this ModelBuilder modelBuilder)
        {
            var assemblyWithConfigurations = typeof(ApplicationDbContext).Assembly;
            modelBuilder.ApplyConfigurationsFromAssembly(assemblyWithConfigurations);

            return modelBuilder;
        }
    }
}
