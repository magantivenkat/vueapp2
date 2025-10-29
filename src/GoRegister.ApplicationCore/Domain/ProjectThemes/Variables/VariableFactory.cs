using System;
using System.Collections.Generic;
using System.Text;

namespace GoRegister.ApplicationCore.Domain.ProjectThemes.Variables
{
    public static class VariableFactory
    {
        public static IEnumerable<ICssVariable> BuildVariableList()
        {
            return new List<ICssVariable>
            {
                // colours
                new BasicCssVariable("primary", "Primary Colour", "#007bff"),
                new BasicCssVariable("primaryLight", "Primary Colour (light)", "#94c0ee"),
                new BasicCssVariable("primaryDark", "Primary Colour (dark)", "#0069d9"),

                // general
                new BasicCssVariable("bodyColor", "Font Color", "#212529"),
                new FontCssVariable("bodyFont", "Font"),
                new BasicCssVariable("bodyBg", "Background Color", "#ffffff"),
                new BasicCssVariable("hColor", "Heading Colour", "var(--body-color)"),
                new FontCssVariable("hFont", "Heading Font"),

                //links
                new BasicCssVariable("linkColor", "Link Color", "#007bff"),
                new BasicCssVariable("linkHoverColor", "Link Hover Color", "#0056b3"),
                new BasicCssVariable("linkHoverDecoration", "Link Hover Decoration", "underline"),

                // forms
                new BasicCssVariable("labelColor", "Label Color", "var(--body-color)"),
                new BasicCssVariable("formWizardDotColor", "Wizard dot colour", "var(--primary)"),
                new BasicCssVariable("formWizardDotBgColor", "Wizard dot background colour", "var(--primary-light)"),

                // nav                
                new BasicCssVariable("navBgColor", "Nav Background Color", "transparent"),
                new BasicCssVariable("navItemColor", "Nav Color", "var(--body-color)"),
                new BasicCssVariable("navItemColorHover", "Nav Color Hover", "var(--nav-item-color)"),
            };
        }
    }
}
