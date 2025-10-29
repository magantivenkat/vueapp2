/*  MRF Changes : Share Link functionality - add column and call function to get publish status
    Modified Date : 04nd Nov 2022
    Modified By : Mandar.Khade@amexgbt.com
    Team member : Harish.Rane@amexgbt.com
    JIRA Ticket No : GoRegister/GOR-242  
 */

using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Domain.Registration.Services;
using GoRegister.ApplicationCore.Domain.Settings.Services;
using GoRegister.Framework.MVC;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace GoRegister.Areas.Admin.ViewComponents
{
    public class DelegateViewComponent : FeatureViewComponent
    {
        private readonly IRegistrationService _registrationService;
        private readonly IRegistrationLinkService _registrationLinkService;
        private readonly IProjectSettingsAccessor _projectSettingsAccessor;

        public DelegateViewComponent(IRegistrationService registrationService, IRegistrationLinkService registrationLinkService, IProjectSettingsAccessor projectSettingsAccessor)
        {
            this._registrationService = registrationService;
            _registrationLinkService = registrationLinkService;
            _projectSettingsAccessor = projectSettingsAccessor;
        }

        public async Task<IViewComponentResult> InvokeAsync(int userId)
        {
            var du = await _registrationService.GetUser(userId);
           
            var vm = new ViewModel();
            vm.InvitationLink = await _registrationLinkService.GetInvitationLink(du.UniqueIdentifier);
            vm.SiteLink = await _registrationLinkService.GetSiteLink(du.UniqueIdentifier);
            vm.FirstName = du.ApplicationUser.FirstName;
            vm.LastName = du.ApplicationUser.LastName;
            vm.RegistrationStatus = (RegistrationStatus)du.RegistrationStatusId;
            vm.Id = du.Id;
            vm.IsTest = du.IsTest;
            vm.AcceptedPrivacyPolicy = du.AcceptedPrivacyPolicy;
                        
            var clientRequest = await _registrationService.GetMRFClientPublishStatus(du.ProjectId);

            if (clientRequest != null)
            {
                if (clientRequest.MRFClientStatus == "Not Published")
                {
                    vm.PublishStatus = false;
                }
                else if(clientRequest.MRFClientStatus == "Published")
                {
                    vm.PublishStatus = true;
                }
            }

            return FolderView(vm);
        }

        public class ViewModel
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Name => $"{FirstName} {LastName}";
            public string InvitationLink { get; set; }
            public string SiteLink { get; set; }

            public RegistrationStatus RegistrationStatus;
            public bool IsTest { get; set; }

            public bool AcceptedPrivacyPolicy { get; set; }

            public bool PublishStatus { get; set; }
        }
    }
}
