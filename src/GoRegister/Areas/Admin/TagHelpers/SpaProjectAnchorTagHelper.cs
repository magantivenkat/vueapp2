using GoRegister.ApplicationCore.Framework;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;

namespace GoRegister.Areas.Admin.TagHelpers
{
    [HtmlTargetElement("spa-proj")]
    public class SpaProjectAnchorTagHelper : TagHelper
    {
        private readonly ProjectTenant _project;
        private IDictionary<string, string> _routeValues;

        /// <summary>
        /// Gets or sets the <see cref="Rendering.ViewContext"/> for the current request.
        /// </summary>
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName("spa-path")]
        public string Path { get; set; }

        public SpaProjectAnchorTagHelper(IHtmlGenerator generator, ProjectTenant project)
        {
            Generator = generator;
            _project = project; 
            _routeValues = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        public IHtmlGenerator Generator { get; }

        public override void Init(TagHelperContext context)
        {
        }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            _routeValues.Add("projectid", _project.Id.ToString());
            var tagBuilder = Generator.GenerateActionLink(
                   ViewContext,
                   linkText: string.Empty,
                   actionName: "App",
                   controllerName: "Home",
                   protocol: null,
                   hostname: null,
                   fragment: null,
                   routeValues: _routeValues,
                   htmlAttributes: null);

            var path = Path?.TrimStart('/') ?? "";

            var href = tagBuilder.Attributes["href"];
            href = $"{href.TrimEnd('/')}/{path}";
            tagBuilder.Attributes["href"] = href;

            output.TagName = "a";
            output.MergeAttributes(tagBuilder);
        }
    }
}
