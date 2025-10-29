using FluentValidation;
using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Framework.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoRegister.ApplicationCore.Domain.Menus.Models
{
    public class MenuItemModel
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public MenuItemType MenuItemType { get; set; }
        public bool OpenInNewTab { get; set; }
        public string CssClass { get; set; }
        public string AnchorLink { get; set; }
        public int? CustomPageId { get; set; }
        public FormType? FormType { get; set; }
        public int? FormId { get; set; }
        public string Fragment { get; set; }
        public IEnumerable<int> RegistrationTypes { get; set; } = new List<int>();
        public IEnumerable<RegistrationStatus> RegistrationStatuses { get; set; } = new List<RegistrationStatus>();
    }

    public class MenuItemModelValidator : AbstractValidator<MenuItemModel>
    {
        public MenuItemModelValidator()
        {
            When(e => e.MenuItemType != MenuItemType.CustomPage, () =>
            {
                RuleFor(e => e.Label).NotEmpty();
            });

            // custom page
            When(e => e.MenuItemType == MenuItemType.CustomPage, () =>
            {
                RuleFor(e => e.CustomPageId).NotNull();
            });

            // link
            When(e => e.MenuItemType == MenuItemType.Link, () =>
            {
                RuleFor(e => e.AnchorLink).NotNull().IsAbsoluteUri();
            });
        }
    }

}
