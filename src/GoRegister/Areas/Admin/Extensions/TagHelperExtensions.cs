using System.Linq;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GoRegister.Areas.Admin.Extensions
{
    public static class TagHelperExtensions {
        public static void AddClasses(this TagHelperOutput output, string classes) {
            var current = output.Attributes.FirstOrDefault(a => a.Name == "class")?.Value;
            output.Attributes.SetAttribute("class", $"{classes} {current}");
        }
    }
}
