using GoRegister.ApplicationCore.Domain.Settings.Services;
using GoRegister.ApplicationCore.Framework;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Registration.Services
{
    public interface IRegistrationLinkService
    {
        Task<string> GetInvitationLink(Guid id);
        string GetInvitationLink(Guid id, string host);
        Task<string> GetDeclineLink(Guid id);
        string GetDeclineLink(Guid id, string host);
        string GetCancelLink(Guid id);
        Task<string> GetSiteLink(Guid id);
        Task<string> GetPreviewLink(Guid id);
        Task<string> GetHostForRedirect(string scheme = "https");
    }

    public class RegistrationLinkService : IRegistrationLinkService
    {
        const string DirectLoginAction = "DirectLogin";

        private readonly IUrlHelper _urlHelper;
        private readonly LinkGenerator _linkGenerator;
        private readonly ProjectTenant _projectTenant;
        private readonly IProjectSettingsAccessor _projectSettingsAccessor;

        public RegistrationLinkService(IUrlHelper urlHelper, LinkGenerator linkGenerator, ProjectTenant projectTenant, IProjectSettingsAccessor projectSettingsAccessor)
        {
            _urlHelper = urlHelper;
            _linkGenerator = linkGenerator;
            _projectTenant = projectTenant;
            _projectSettingsAccessor = projectSettingsAccessor;
        }

        public string GetCancelLink(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetDeclineLink(Guid id)
        {
            var host = await GetHost();

            return GetDeclineLink(id, host);
        }

        public string GetDeclineLink(Guid id, string host)
        {
            // generate return url
            var redirectUrl = RemovePathBase(_urlHelper.Action("Decline", "Register", new { Area = "" }));

            // generate main link
            var url = RemovePathBase(_urlHelper.Action(DirectLoginAction, "User", new { returnUrl = redirectUrl, Area = "", id }));

            return AddHostAndScheme(host, url);
        }

        public async Task<string> GetSiteLink(Guid id)
        {
            var host = await GetHost();
            var url = RemovePathBase(_urlHelper.Action(DirectLoginAction, "User", new { Area = "", id }));

            return AddHostAndScheme(host, url);
        }

        public async Task<string> GetPreviewLink(Guid id)
        {
            var host = await GetHost();
            var link = _linkGenerator.GetUriByAction("Preview", "User", new { area = "", id = id }, "https", new HostString(host));
            return link;
        }

        public async Task<string> GetInvitationLink(Guid id)
        {
            var host = await GetHost();

            return GetInvitationLink(id, host);
        }

        public string GetInvitationLink(Guid id, string host)
        {
            // generate return url
            var redirectUrl = RemovePathBase(_urlHelper.Action("Index", "Register", new { Area = "" }));

            // generate main link
            var url = RemovePathBase(_urlHelper.Action(DirectLoginAction, "User", new { returnUrl = redirectUrl, Area = "", id }));

            return AddHostAndScheme(host, url);
        }

        private string RemovePathBase(string url)
        {
            // because urls are generated with the admin's PathBase we need to trim that from the front
            if(!string.IsNullOrWhiteSpace(_projectTenant.Prefix))
            {
                url = url.Substring(_projectTenant.Prefix.Length + 1);
            }

            return url;
        }

        private string AddHostAndScheme(string host, string path, string scheme = "https")
        {
            return $"{scheme}://{host}{path}";
        }
        public async Task<string> GetHostForRedirect(string scheme = "https")
        {
            var settings = await _projectSettingsAccessor.GetAsync();
            return $"{scheme}://{settings.Host}";        
        }
        private async Task<string> GetHost()
        {
            var settings = await _projectSettingsAccessor.GetAsync();
            string host;
            if (!string.IsNullOrWhiteSpace(settings.Prefix))
            {
                host = $"{settings.Host}/{settings.Prefix}";
            }
            else
            {
                host = settings.Host;
            }

            return host;
        }

    }
}
