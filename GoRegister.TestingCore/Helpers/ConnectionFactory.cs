using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Framework.Multitenancy;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;

namespace GoRegister.TestingCore.Helpers
{
    public class ConnectionFactory : IDisposable
    {
        private bool disposedValue = false; // To detect redundant calls
        public readonly string DatabaseName;

        public ConnectionFactory()
        {
            DatabaseName = Guid.NewGuid().ToString();
        }

        public ApplicationDbContext CreateContextForInMemory(ITenantAccessor tenantAccessor)
        {
            var option = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: DatabaseName).Options;

            var context = new ApplicationDbContext(tenantAccessor, option);
            if (context != null)
            {
                //context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            return context;
        }

        public ApplicationDbContext CreateContextForSQLite()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var option = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(connection).Options;

            var context = new ApplicationDbContext(option);

            if (context != null)
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            return context;
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }

}
