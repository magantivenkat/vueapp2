/*  MRF Changes : Change display name of Clone Registration option available on Create MRF form
    Modified Date : 16th September 2022
    Modified By : Mandar.Khade @amexgbt.com
    Team member : Harish.Rame @amexgbt.com
    JIRA Ticket No : GoRegister/GOR-213 , GOR - 245 (Set clone project properties)  */

using System.Collections.Generic;
using System;
using System.ComponentModel;
using MediatR;

namespace GoRegister.ApplicationCore.Domain.Projects.Models
{
    public class Response
    {
        public List<ProjectModel> RecentProjects { get; set; }
        public List<ProjectModel> AllProjects { get; set; }
        public List<ClientModelMRF> AllClientsMRF { get; set; }
    }

    public class ProjectModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }

        public string ClientName { get; set; }
        public bool IsActive { get; set; }
    }

    public class ClientModelMRF
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime DateCreated { get; set; }

        public string Uuid { get; set; }

        public DateTime? deletedAt { get; set; }

        public string MRFClientStatus { get; set; }

    }
    public class CloneProjectModel
    {
        bool isCloneRegistration;
        bool isCloneRegistrationTypes;
        bool isClonePages;

        [DisplayName("Clone Theme")]
        public bool CloneTheme { get; set; }

        [DisplayName("Clone Forms")]
        public bool CloneRegistration   // property
        {
            get { return isCloneRegistration; }   // get method
            set { isCloneRegistration = false; }  // set method
        }

        [DisplayName("Clone Registration Types")]
        public bool CloneRegistrationTypes   // property
        {
            get { return isCloneRegistrationTypes; }   // get method
            set { isCloneRegistrationTypes = false; }  // set method
        }

        [DisplayName("Clone Pages")]
        public bool ClonePages   // property
        {
            get { return isClonePages; }   // get method
            set { isClonePages = false; }  // set method
        }

        [DisplayName("Clone Menu Items")]
        public bool CloneMenuItems { get; set; }
        public bool CloneSessions { get; set; }
        public bool CloneReports { get; set; }

    }
}
