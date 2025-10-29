using System.Linq;
using Microsoft.AspNetCore.Razor.TagHelpers;
using GoRegister.Areas.Admin.Extensions;

namespace GoRegister.Areas.Admin.TagHelpers {
    [HtmlTargetElement("side-nav-group")]
    public class SideNavGroupTagHelper : TagHelper {
        public override void Process(TagHelperContext context, TagHelperOutput output) {
            output.TagName = "ul";

            output.AddClasses("nav flex-column");
        }
    }
}
