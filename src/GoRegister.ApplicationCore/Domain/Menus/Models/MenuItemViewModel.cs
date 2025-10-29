using GoRegister.ApplicationCore.Framework.Domain;
using System.Collections.Generic;

namespace GoRegister.ApplicationCore.Domain.Menus.Models
{
    public class MenuItemViewModel
    {
        public MenuItemModel Model { get; set; }
        public IEnumerable<TextValueModel> MenuItemTypes { get; set; }
        public List<TextValueModel> CustomPages { get; set; }
        public List<TextValueModel> RegistrationTypes { get; set; }
    }
}
