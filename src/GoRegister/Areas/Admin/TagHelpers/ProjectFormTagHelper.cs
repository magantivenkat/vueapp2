using GoRegister.ApplicationCore.Framework;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GoRegister.Areas.Admin.TagHelpers
{
    [HtmlTargetElement("form-proj")]
    public class ProjectFormTagHelper : FormTagHelper
    {
        private readonly ProjectTenant _project;

        public ProjectFormTagHelper(IHtmlGenerator generator, ProjectTenant project) : base(generator)
        {
            _project = project;
        }

        public override void Init(TagHelperContext context)
        {
            base.Init(context);
            RouteValues.Add("projectid", _project.Id.ToString());
        }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "form";
            base.Process(context, output);
        }
    }
}
