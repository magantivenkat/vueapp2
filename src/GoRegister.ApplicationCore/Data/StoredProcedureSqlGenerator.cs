using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Data
{
    public static class StoredProcedureSqlGenerator
    {
        public static MigrationBuilder CreateStoredProcedure(this MigrationBuilder builder, string storedProcedure)
        {
            builder.Sql(ReadFile(storedProcedure, "create"));
            return builder;
        }

        public static MigrationBuilder AlterStoredProcedure(this MigrationBuilder builder, string storedProcedure)
        {
            builder.Sql(ReadFile(storedProcedure, "alter"));
            return builder;
        }

        public static MigrationBuilder DropStoredProcedure(this MigrationBuilder builder, string storedProcedure)
        {
            builder.Sql($"DROP PROCEDURE [{storedProcedure}]");
            return builder;
        }

        private static string ReadFile(string storedProcedure, string fileName)
        {
            var buildDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var filePath = buildDir + $@"\Data\StoredProcs\{storedProcedure}\{fileName}.sql";
            return File.ReadAllText(filePath);
        }
    }
}
