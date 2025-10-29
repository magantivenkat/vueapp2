using FluentAssertions;
using GoRegister.ApplicationCore.BuildTests.Domain.Menus.Fakes;
using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Domain.Menus;
using System;
using System.Collections.Generic;
using Xunit;

namespace GoRegister.ApplicationCore.BuildTests.Domain.Menus
{
    public class MenuFactoryTests
    {
        [Theory]
        [InlineData(MenuItemType.Link, typeof(LinkHandlerFake))]
        [InlineData(MenuItemType.CustomPage, typeof(CustomPageHandlerFake))]
        public void Should_ReturnCorrectDriver_When_GivenType(MenuItemType type, Type handlerType)
        {
            var factory = GetFactory();
            factory.GetHandler(type).Should().BeOfType(handlerType);
        }

        public IMenuFactory GetFactory()
        {
            return new MenuFactory(new List<IMenuHandler>()
            {
                new LinkHandlerFake(),
                new CustomPageHandlerFake()
            });
        }
    }
}
