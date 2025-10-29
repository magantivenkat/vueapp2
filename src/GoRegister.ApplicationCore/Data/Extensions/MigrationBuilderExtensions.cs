using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace GoRegister.ApplicationCore.Data.Extensions
{
    public static class MigrationBuilderExtensions
    {
        /// <summary>
        /// Wraps the supplied sql in exec function. This is to 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static OperationBuilder<SqlOperation> SqlExec(this MigrationBuilder builder, string sql)
        {
            return builder.Sql("EXECUTE ('" + sql.Replace("'", "''") + "')");
        }
    }
}
