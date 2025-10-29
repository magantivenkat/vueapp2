using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GoRegister.Areas.Admin.TagHelpers
{
    [HtmlTargetElement("select", Attributes = "auto-post-back")]
    public class AutoPostBackTagHelper : TagHelper
    {
        public bool AutoPostBack { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (AutoPostBack)
            {
                output.Attributes.SetAttribute("onchange", "this.form.submit();");
            }
        }
    }
}
