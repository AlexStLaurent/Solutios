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
using Solutios.Models.ViewModel;

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

            return View();
        }

        [Authorize]
        public IActionResult Projection(int id)
        {
            Project p = projectmanager.getProjet(id);
            ViewData["Test"] = projectmanager.diagrame(id);
            ViewData["id"] = id;

            return View(p.listProjectSoumission());


        }


        [Authorize]
        public IActionResult AddProjet()
        {
            ViewData["Users"] = usermanager.listeUser();
            return View();

        }


        [Authorize]
        [HttpPost]
        public IActionResult AddProjection(IFormCollection formCollection)
        {
            List<FollowInfo> list = new List<FollowInfo>();
            Project p = projectmanager.getProjet(Convert.ToInt32(formCollection["id"]));
            string[] Spending = formCollection["Spending"];
            string[] montant = formCollection["Montant"];
            int count = 0;
            foreach (var item in p.listProjectSoumission())
            {
                FollowInfo f = new FollowInfo();
                f.amount = Convert.ToDouble(montant[count]);
                f.Spending = Spending[count];
                list.Add(f);
                count++;
            }
            ProjectFollowUp projectFollowUp = new ProjectFollowUp();
            projectFollowUp.PfFollowUpId = Convert.ToInt32(formCollection["id"]);
            FollowUp follow = new FollowUp();
            follow.FuDate = DateTime.Now;
            follow.FuInfo = JsonConvert.SerializeObject(list);
            FollowUp ff = new FollowUp();
            ff = _context.FollowUp.Last();
           
            _context.Add(follow);
            _context.SaveChanges();
            projectFollowUp.PfFollowUpId = follow.FuId;
            
            projectFollowUp.PfProject = p;
            projectFollowUp.PfFollowUp = follow;
            _context.ProjectFollowUp.Add(projectFollowUp);
            _context.SaveChanges();

            return RedirectToAction("Projet", new { id = formCollection["id"] });
        }

        [HttpPost]
        public IActionResult Addprojet(IFormCollection formCollection)
        {
            ProjectFollowUp projectFollowUp = new ProjectFollowUp();

            Project p = new Project();
            p.ProjectName = formCollection["ProjectName"];
            p.ProjectDebut = Convert.ToDateTime(formCollection["project_debut"]);
            p.ProjectFin = Convert.ToDateTime(formCollection["project-fin"]);
            //TOFIX: La sérialisation JSON Échoue s'il y a plus qu'un mot dans le champ "Spending"
            //I know on va juste empecher avec des espaces
            FollowInfo[] follwlist = new JavaScriptSerializer().Deserialize<FollowInfo[]>(formCollection["table"]);
            List<FollowInfo> soumission = new List<FollowInfo>();
            foreach (var item in follwlist)
            {
                soumission.Add(item);
            }
            FollowUp follow = new FollowUp();
            follow.FuDate = DateTime.Now;
            follow.FuInfo = JsonConvert.SerializeObject(soumission);
        
            _context.Add(follow);
            _context.SaveChanges();
            projectFollowUp.PfFollowUpId = follow.FuId;
           
            projectFollowUp.PfProject = p;
            
           
            p.ProjectSoumission = JsonConvert.SerializeObject(soumission);
            Users user = new Users();
            user = usermanager.FindUserByID(formCollection["Users"]);
            projectmanager.addProjet(p);

            projectFollowUp.PfFollowUp = follow;
            _context.ProjectFollowUp.Add(projectFollowUp);
            _context.SaveChanges();
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
            ViewData["depense"] = id;
            if (projectmanager.GetLastProjection(id) != null)
            {
                ViewData["Projection"] = JsonConvert.DeserializeObject<List<FollowInfo>>(projectmanager.GetLastProjection(id).FuInfo);
            }
            else
            {
                ViewData["Projection"] = p.listProjectSoumission();
            }

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
        public IActionResult AddUsers(Users user)
        {

            if (ModelState.IsValid)
            {
                usermanager.AddUser(user);

                return Redirect("/Admin/Usagers");
            }

            else return RedirectToAction("Error");
        }

        [HttpPost]
        public IActionResult Saveee(string objectArray)
        {
            List<FollowInfo> followInfo = new List<FollowInfo>();
            followInfo = JsonConvert.DeserializeObject<List<FollowInfo>>(objectArray);
            return Redirect("/Admin/Usagers");
        }

        public IActionResult Error()
        {
            return View("Error", new ErrorViewModel());
        }
    }
}