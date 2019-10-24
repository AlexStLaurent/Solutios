using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solutios.Models;

namespace Solutios.Controllers
{
    public class AdminController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {

            ViewData["Thanksdata"] = "data";
            ViewData["test"] = "[31784.17, 52359.54, 19534.54, 2354.18, 6168.3, 0.19]";
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
        public IActionResult profil()
        {

            ViewData["Thanksdata"] = "data";
            return View();
        }
        public IActionResult Activitylog()
        {

            ViewData["Thanksdata"] = "data";
            return View();
        }
        public IActionResult Usagers()
        {

            ViewData["Thanksdata"] = "data";
            return View();
        }

        [HTTPpost]
        public IActionResult AddUser(Users user){
            UserManager.AddUser(user);

            return Redirect("/Admin/Usagers");
        }
        
    }
}