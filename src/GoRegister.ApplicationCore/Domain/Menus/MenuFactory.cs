using GoRegister.ApplicationCore.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoRegister.ApplicationCore.Domain.Menus
{
    public interface IMenuFactory
    {
        IMenuHandler GetHandler(MenuItemType type);
    }

    public class MenuFactory : IMenuFactory
    {
        private readonly IEnumerable<IMenuHandler> _handlers;

        public MenuFactory(IEnumerable<IMenuHandler> handlers)
        {
            _handlers = handlers;
        }

        public IMenuHandler GetHandler(MenuItemType type)
        {
            return _handlers.First(h => h.MenuItemType == type);
        }
    }
}
