using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Nancy.Json;
using Newtonsoft.Json;
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
        [Authorize(Roles = "ADMIN")]
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
        public IActionResult Projection(int id)
        {
            Project p = projectmanager.getProjet(id);
            ViewData["Test"] = projectmanager.diagrame(id);
            
            return View(p.listProjectSoumission());
           
           
            
        }

        
        [Authorize]
        public IActionResult AddProjet()
        {
            ViewData["Users"] = usermanager.listeUser();
            return View();

        }


        [Authorize]
        public IActionResult AddProjection(int id, IFormCollection formCollection)
        {
            List<FollowInfo> list = new List<FollowInfo>();



            return RedirectToAction("Projet");
        }

        [HttpPost]
        public IActionResult Addprojet(IFormCollection formCollection)
        {
            Project p = new Project();
            p.ProjectName = formCollection["ProjectName"];
            p.ProjectDebut = Convert.ToDateTime(formCollection["project_debut"]);
            p.ProjectFin = Convert.ToDateTime(formCollection["project-fin"]);
            string s = formCollection["table"];
            s = s.Substring(2, s.Length - 2);
            s = s.Remove(s.Length - 1, 1);
            //TOFIX: La sérialisation JSON Échoue s'il y a plus qu'un mot dans le champ "Spending"
            FollowInfo[] follwlist = new JavaScriptSerializer().Deserialize<FollowInfo[]>(formCollection["table"]);
            List<FollowInfo> soumission = new List<FollowInfo>();
            foreach (var item in follwlist)
            {
                soumission.Add(item);
            }

            p.ProjectSoumission = JsonConvert.SerializeObject(soumission);
            Users user = new Users();
            user = usermanager.FindUserByID(formCollection["Users"]);
            projectmanager.addProjet(p);

            return RedirectToAction("Index");

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
            ViewData["id"] = id;
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

        [HttpPost]
        public IActionResult Saveee(string objectArray)
        {
            List<FollowInfo> followInfo = new List<FollowInfo>();
            followInfo = JsonConvert.DeserializeObject<List<FollowInfo>>(objectArray);
            return Redirect("/Admin/Usagers");
        }
        
    }
}