using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Solutios.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Thanksdata"] = "nop";
            return View();
        }

        public IActionResult Param()
        {
            ViewData["Thanksdata"] = "nop";
            return View();
        }

        public IActionResult Projet()
        {
            ViewData["Thanksdata"] = "nop";
            return View();
        }
        public IActionResult AddProjet()
        {
            ViewData["Thanksdata"] = "nop";
            return View();
        }

    }
}