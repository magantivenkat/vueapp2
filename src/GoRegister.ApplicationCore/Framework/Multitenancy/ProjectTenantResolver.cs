using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using GoRegister.ApplicationCore.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using StackExchange.Profiling;
using System.Data;
using Microsoft.Data.Sqlite;
using GoRegister.ApplicationCore.Framework.Multitenancy.Internal;

namespace GoRegister.ApplicationCore.Framework.Multitenancy
{
    public class ProjectTenantResolver : ITenantResolver
    {
        private readonly ILogger _logger;
        private readonly AdminTenantSettings _adminTenantSettings;

        public ProjectTenantResolver(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            Configuration = configuration;
            _logger = loggerFactory.CreateLogger<ProjectTenantResolver>();

            _adminTenantSettings = Configuration.GetSection("AdminTenantSettings").Get<AdminTenantSettings>();
        }

        public IConfiguration Configuration { get; }

        public async Task<ProjectTenant> ResolveAsync(HttpContext context)
        {
            _ = context ?? throw new ArgumentNullException(nameof(context));

            var hostName = context.Request.Host.Value.ToLower();
            var appRelativePath = context.Request.Path.Value.TrimStart('/');
            var firstSegmentIndex = appRelativePath.Length > 0 ? appRelativePath.IndexOf('/', 0) : -1;
            string path;
            if (firstSegmentIndex > -1)
            {
                path = appRelativePath.Substring(0, firstSegmentIndex);
            }
            else
            {
                path = appRelativePath;
            }

            var tenantContext = Get(hostName, path);
            if (tenantContext == null)
                tenantContext = Get(hostName);

            if (tenantContext != null)
                return ConfigureAdminProject(tenantContext, appRelativePath);

            var project = await GetProject(hostName, path);
            if (project == null)
                return null;

            tenantContext = new ProjectTenant
            {
                Hostname = project.Host,
                Id = project.Id,
                Name = project.Name,
                Prefix = project.Prefix,
                //IsAdmin = project.Id == 1
            };

            // we will be deployed across multiple machines so cant use memory cache
            //_cache.Set(new TenantCacheKey { Host = tenantContext.Hostname, Prefix = project.Prefix }, tenantContext);

            return ConfigureAdminProject(tenantContext, appRelativePath);
        }

        private ProjectTenant Get(string host, string prefix = null)
        {
            // check for the admin tenant
            if(_adminTenantSettings.Host == host && _adminTenantSettings.Prefix == prefix)
            {
                return new ProjectTenant
                {
                    Id = 0,
                    IsAdmin = true,
                    Name = "Admin",
                    Hostname = _adminTenantSettings.Host,
                    Prefix = _adminTenantSettings.Prefix
                };
            }

            return null;

            // cant use memory cache
            //var tenantCacheKey = new TenantCacheKey() { Host = host, Prefix = prefix };
            //return _cache.Get(tenantCacheKey) as ProjectTenant;
        }

        private ProjectTenant ConfigureAdminProject(ProjectTenant tenantContext, string appRelativePath)
        {
            if(tenantContext.IsAdmin)
            {
                string prepend = !string.IsNullOrWhiteSpace(tenantContext.Prefix) ? tenantContext.Prefix + @"\/" : "";
                var regex = new Regex(prepend + @"project\/([0-9]+)");
                var match = regex.Match(appRelativePath);
                if (match.Success)
                {
                    var id = Int32.Parse(match.Groups[1].Value);
                    return tenantContext.Clone(id);
                }
            }

            return tenantContext;
        }

        private async Task<DbProject> GetProject(int id)
        {
            using (var connection = GetConnection())
            {
                return (await connection.QueryAsync<DbProject>("select [Id], [Name], [Host], [Prefix] from Project where id = @id", new { id }))
                    .FirstOrDefault();
            }
        }

        private async Task<DbProject> GetProject(string hostName, string prefix)
        {
            using (var connection = GetConnection())
            {
                var query = "select [Id], [Name], [Host], [Prefix] from Project where host = @hostName";
                if (!string.IsNullOrWhiteSpace(prefix))
                {
                    query += " and prefix = @prefix";
                }
                else
                {
                    query += " and (prefix is null or prefix = '')";
                }
                return await connection.QueryFirstOrDefaultAsync<DbProject>(query, new { hostName, prefix });
            }
        }

        private IDbConnection GetConnection()
        {
            if (Configuration.GetValue<bool>("UseSqlite"))
            {
                return new SqliteConnection(Configuration.GetConnectionString("DefaultConnection"));
            }
            else
            {
                return new SqlConnection(Configuration.GetConnectionString("DefaultConnection"));
            }

            //new StackExchange.Profiling.Data.ProfiledDbConnection(new SqlConnection(Configuration.GetConnectionString("DefaultConnection")), MiniProfiler.Current)
        }

        private class DbProject
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Host { get; set; }
            public string Prefix { get; set; }
        }

        private struct TenantCacheKey
        {
            public string Host { get; set; }
            public string Prefix { get; set; }
        }
    }
}
