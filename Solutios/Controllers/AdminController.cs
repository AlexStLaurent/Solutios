using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Solutios.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Thanksdata"] = "data";
            return View();
        }
    }
}