using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GoRegister.Areas.Admin.ViewModels;
using GoRegister.ApplicationCore.Data;

namespace GoRegister.Areas.Admin.Controllers
{
    public class DesignerController : ProjectAdminControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DesignerController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Edit(int id)
        {
            var page = _context.CustomPages.FirstOrDefault(e => e.Id == id);
            if(page == null)
            {
                return NotFound();
            }

            var vm = new CustomPageEditorViewModel()
            {
                Id = page.Id,
                Slug = page.Slug,
                Document = page.Content,
                Title = page.Title
            };

            return View(vm);
        }

        [HttpPost, ActionName("Edit")]
        public IActionResult EditPOST(CustomPageEditorViewModel vm)
        {
            var page = _context.CustomPages.FirstOrDefault(e => e.Id == vm.Id);
            if (page == null)
            {
                return NotFound();
            }

            page.Slug = vm.Slug;
            page.Title = vm.Title;
            page.Content = vm.Document;

            _context.SaveChanges();

            return RedirectToAction("Edit", vm.Id);
        }
    }
}