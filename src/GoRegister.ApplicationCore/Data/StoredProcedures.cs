using Dapper;
using Microsoft.EntityFrameworkCore;
using GoRegister.ApplicationCore.Data.Models.Query;
using System.Collections.Generic;
using System.Data;

namespace GoRegister.ApplicationCore.Data
{
    public class StoredProcedures
    {
        private readonly ApplicationDbContext _context;

        public StoredProcedures(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<GetRegistrationFieldsForUser_Result> GetRegistrationFieldsForUser(int userId, int pageTypeId)
        {
            var connection = _context.Database.GetDbConnection();
   
                return connection.Query<GetRegistrationFieldsForUser_Result>("GetRegistrationFieldsForUser", new { userId, RegistrationPageTypeId = pageTypeId }, commandType: CommandType.StoredProcedure);

        }

        public IEnumerable<GetRegistrationFieldDropdownOptionsForUser_Result> GetRegistrationFieldDropdownOptionsForUser(int userId, int pageTypeId)
        {
            var connection = _context.Database.GetDbConnection();
                return connection.Query<GetRegistrationFieldDropdownOptionsForUser_Result>("GetRegistrationFieldDropdownOptionsForUser", new { userId, RegistrationPageTypeId = pageTypeId }, commandType: CommandType.StoredProcedure);

        }

        public IEnumerable<GetRegistrationPagesForUser_Result> GetRegistrationPagesForUser(int userId, int pageTypeId)
        {
            var connection = _context.Database.GetDbConnection();
            return connection.Query<GetRegistrationPagesForUser_Result>("GetRegistrationPagesForUser", new { userId, RegistrationPageTypeId = pageTypeId }, commandType: CommandType.StoredProcedure);
        }

        public IEnumerable<GetRegistrationFieldOptionRulesForUser_Result> GetRegistrationFieldOptionRulesForUser(int userId, int pageTypeId)
        {
       var connection = _context.Database.GetDbConnection();
         
                return connection.Query<GetRegistrationFieldOptionRulesForUser_Result>("GetRegistrationFieldOptionRulesForUser", new { userId, RegistrationPageTypeId = pageTypeId }, commandType: CommandType.StoredProcedure);
       
        }

        public IEnumerable<GetUserSession_Result> GetUserSession(int? userId, int projectId)
        {
            var connection = _context.Database.GetDbConnection();

            return connection.Query<GetUserSession_Result>("usp_GetUserSessions", new { userId, projectId }, commandType: CommandType.StoredProcedure);

        }
    }
}
