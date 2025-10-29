using GoRegister.ApplicationCore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Projects.Extensions
{
    public static class ProjectSettingsExtensions
    {
        public static string GetProjectUrl(this Project project)
        {
            return project.Host + "/" + project.Prefix;
        }
    }
}
