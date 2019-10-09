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

        public IActionResult Param()
        {

            ViewData["Thanksdata"] = "data";
            return View();
        }

        public IActionResult AddProjet()
        {

            ViewData["Thanksdata"] = "data";
            return View();
        }

        public IActionResult Archive()
        {

            ViewData["Thanksdata"] = "data";
            return View();
        }

        public IActionResult Projet()
        {

            ViewData["Thanksdata"] = "data";
            return View();
        }

        public IActionResult Usagers()
        {
            ViewData["Thanksdata"] = "data";
            return View();
        }
    }
}