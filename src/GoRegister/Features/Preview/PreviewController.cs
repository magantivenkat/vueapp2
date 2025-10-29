using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.Features.Preview
{
    public class PreviewController : Controller
    {
        public IActionResult Elements()
        {
            return View();
        }
    }
}
