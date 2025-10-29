using Microsoft.AspNetCore.Mvc;
using GoRegister.ApplicationCore.Framework;

namespace GoRegister.Areas.Admin.ViewComponents {
    public class SidebarViewComponent : ViewComponent {
        private readonly IUrlHelper _urlHelper;
        private readonly ProjectTenant _projectTenant;

        public SidebarViewComponent(IUrlHelper urlHelper, ProjectTenant projectTenant) {
            _urlHelper = urlHelper;
            _projectTenant = projectTenant;
        }

        public IViewComponentResult Invoke(string filter) {
            return View(_projectTenant.IsProjectAdmin);
        }
    }
}
