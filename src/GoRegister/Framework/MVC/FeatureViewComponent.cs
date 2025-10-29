using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.Framework.MVC
{
    public abstract class FeatureViewComponent : ViewComponent
    {
        public ViewViewComponentResult FolderView()
        {
            return View(GetView(this.GetType().Name));
        }

        public ViewViewComponentResult FolderView(string view)
        {
            return View(GetView(view));
        }

        public ViewViewComponentResult FolderView<TModel>(TModel model)
        {
            return FolderView(this.GetType().Name, model);
        }

        public ViewViewComponentResult FolderView<TModel>(string view, TModel model)
        {
            return View(GetView(view), model);
        }

        private string GetView(string view)
        {
            var area = ViewContext.RouteData.Values["area"];
            if (area != null)
            {
                return $"~/Areas/{(string)area}/ViewComponents/{view}.cshtml";
            }

            return $"~/Views/Shared/Components/{view}.cshtml";
        }
    }
}
