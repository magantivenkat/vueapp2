using GoRegister.ApplicationCore.Framework.Dapper;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace GoRegister.ApplicationCore.Data
{
    public class SlickConnection : ISlickConnection
    {
        private readonly IConfiguration _configuration;


        public SlickConnection(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Slick Get()
        {
            return new Slick(new SqlConnection(_configuration.GetConnectionString("DefaultConnection")));
        }

        public Slick Get(string connectionString)
        {
            return new Slick(new SqlConnection(connectionString));
        }
    }
}
