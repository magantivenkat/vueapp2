using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoRegister.ApplicationCore.Data.Models
{
    public class ApplicationUser : IdentityUser<int>, IMayHaveProject
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public string Name { get; set; }
        public string TimeZone { get; set; }
        public string DateFormat { get; set; }

        public int? ProjectId { get; set; }

        public virtual Project Project { get; set; }

        [InverseProperty("ApplicationUser")]
        public virtual DelegateUser DelegateUser { get; set; }
        public ICollection<DataQuery> DataQueries { get; set; }
        public ICollection<RecentProject> RecentProjects { get; set; } = new HashSet<RecentProject>();
        public ICollection<UserAction> UserActions { get; set; } = new HashSet<UserAction>();
        public ICollection<UserProjectMapping> UserProjectMapping { get; set; } = new HashSet<UserProjectMapping>();

        public ICollection<Project> CreatedProjects { get; set; } = new HashSet<Project>();

        /// <summary>
        /// Creates an application user with a guid for a username.
        /// </summary>
        /// <returns>Said user</returns>
        public static ApplicationUser Create()
        {
            var username = Guid.NewGuid().ToString();
            var user = new ApplicationUser();
            user.UserName = username;
            user.NormalizedUserName = username.ToUpper();
            user.SecurityStamp = Guid.NewGuid().ToString();
            return user;
        }
    }
}
