using Dapper;
using EFCore.BulkExtensions;
using GoRegister.ApplicationCore.Data.Specification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Data
{
    public interface IRepository
    {
        ApplicationDbContext DbContext { get; }
        Task<List<T>> ToListAsync<T>() where T : class;
        Task<IEnumerable<T>> SqlQueryAsync<T>(ISqlSpecification<T> specification, object parameters = null);
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task BulkInsertAsync<T>(IList<T> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, CancellationToken cancellationToken = default) where T : class;
    }

    public class Repository : IRepository
    {
        private readonly IConfiguration _configuration;
        public ApplicationDbContext DbContext { get; }

        public Repository(IConfiguration configuration, ApplicationDbContext dbContext)
        {
            _configuration = configuration;
            DbContext = dbContext;
        }

        public Task<List<T>> ToListAsync<T>() where T : class
        {
            return DbContext.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> SqlQueryAsync<T>(ISqlSpecification<T> specification, object parameters = null)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                return await connection.QueryAsync<T>(specification.Sql, specification.Parameters);
            }
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await DbContext.Database.BeginTransactionAsync();
        }

        public async Task BulkInsertAsync<T>(IList<T> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, CancellationToken cancellationToken = default) where T : class
        {
            await DbContext.BulkInsertAsync(entities, bulkConfig, progress, cancellationToken);
        }
    }
}
