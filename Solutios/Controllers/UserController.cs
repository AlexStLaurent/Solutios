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
            return View();
        }

        public IActionResult Param()
        {
            return View();
        }

        public IActionResult Projet()
        {
            return View();
        }
        public IActionResult AddProjet()
        {
            return View();
        }

    }
}