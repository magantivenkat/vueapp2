using GoRegister.Areas.Admin.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.Areas.Admin.TagHelpers
{
    [HtmlTargetElement("help-text")]
    public class HelpTextTagHelper : TagHelper
    {
        public string Icon { get; set; } = "question-circle";

        public async override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "span";

            var actualContent = await output.GetChildContentAsync();

            var content = $@"<i class='far fa-{Icon}' data-toggle='tooltip' data-placement='right' title='{actualContent.GetContent()}'></i>";

            output.Content.SetHtmlContent(content);
        }
    }
}