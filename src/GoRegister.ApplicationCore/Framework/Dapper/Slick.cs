using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Framework.Dapper
{
    public class Slick : IDisposable
    {
        private readonly IDbConnection _dbConnection;

        public Slick() { }

        public Slick(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public IDbConnection DbConnection => _dbConnection;

        public virtual Task<IEnumerable<dynamic>> QueryAsync(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null) => _dbConnection.QueryAsync(sql, param, transaction, commandTimeout, commandType);
        public virtual Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null) => _dbConnection.QueryAsync<T>(sql, param, transaction, commandTimeout, commandType);
        public virtual Task<int> ExecuteAsync(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
            => _dbConnection.ExecuteAsync(sql, param, transaction, commandTimeout, commandType);

        public virtual void Dispose() => _dbConnection?.Dispose();
    }
}
