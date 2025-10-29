using GoRegister.Areas.Admin.Extensions;
using GoRegister.ApplicationCore.Framework;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace GoRegister.Areas.Admin.TagHelpers
{
    [HtmlTargetElement("side-nav-item")]
    public class SideNavItemTagHelper : AnchorTagHelper
    {

        private readonly ProjectTenant _project;

        public SideNavItemTagHelper(IHtmlGenerator generator, ProjectTenant project)
            : base(generator)
        {
            _project = project;
        }

        public string Icon { get; set; }

        public bool ProjectLink { get; set; }

        public override void Init(TagHelperContext context)
        {
            base.Init(context);
            if (ProjectLink)
                RouteValues.Add("projectid", _project.Id.ToString());
        }

        public async override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            var classes = "nav-link";
            var wrapperClasses = "nav-item";
            if (IsActive())
            {
                wrapperClasses += " active";
            }

            var actualContent = await output.GetChildContentAsync();

            output.TagName = "a";

            var inner = $"<i class='fas fa-fw fa-{Icon}'></i><span>&nbsp;{actualContent.GetContent()}</span>"; // &nbsp; hack until using tag helpers -- if the span is on a new line you get the extra space that is needed :)

            output.Content.SetHtmlContent(inner);


            output.PreElement.SetHtmlContent($"<li class='{wrapperClasses}'>");
            output.AddClasses(classes);
            output.PostElement.SetHtmlContent("</li>");
        }

        private bool IsActive()
        {
            if (!ViewContext.RouteData.Values.ContainsKey("Controller")) return false;

            var currentController = ViewContext.RouteData.Values["Controller"].ToString();
            var currentAction = ViewContext.RouteData.Values["Action"].ToString();

            var show = false;
            if (!String.IsNullOrWhiteSpace(Controller) &&
                Controller.Equals(currentController, StringComparison.CurrentCultureIgnoreCase))
            {
                show = true;
            }
            if (show &&
                !String.IsNullOrWhiteSpace(Action) &&
                Action.Equals(currentAction, StringComparison.CurrentCultureIgnoreCase))
            {
                show = true;
            }
            else
            {
                show = false;
            }

            return show;
        }
    }
}
