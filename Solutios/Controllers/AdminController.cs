using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Solutios.Models;

namespace Solutios.Controllers
{
    public class AdminController : Controller
    {
       private readonly ProjetSolutiosContext _context;
        UserManager usermanager;
        ProjectManager projectmanager = new ProjectManager();    
        
        public AdminController(ProjetSolutiosContext context)
        {
            this._context = context;
            usermanager = new UserManager(_context);
            projectmanager = new ProjectManager(_context);
        }
        [Authorize]
        public IActionResult Index()
        {
            
            ViewData["test"] = "[31784.17, 52359.54, 19534.54, 2354.18, 6168.3, 0.19]";
            return View(usermanager.showAllProject());
        }

        [Authorize]
        public IActionResult Param()
        {

            ViewData["Thanksdata"] = "data";
            return View();
        }

        [Authorize]
        public IActionResult AddProjet()
        {
            Project j = new Project();
            projectmanager.addProjet(j);
            ViewData["Users"] = usermanager.listeUser();
            return View();
        }

        [Authorize]
        public IActionResult Archive()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult Projet(int id)
        {
            Project p = projectmanager.getProjet(id);
            ViewData["Test"] = projectmanager.diagrame(id);
            return View(p.listProjectSoumission());
        }
        [Authorize]
        public IActionResult profil()
        {
            string user = User.FindFirstValue(ClaimTypes.NameIdentifier);            
            return View(usermanager.FindUserByID(user));
        }
        public IActionResult Activitylog()
        {
            return View();
        }
        public IActionResult Usagers()
        {
            return View(usermanager.listeUser());
        }

        [HttpPost]
        public IActionResult AddUsers(Users user){

            usermanager.AddUser(user);

            return Redirect("/Admin/Usagers");
        }
        
    }
}