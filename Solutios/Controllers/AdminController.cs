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
        //Variable commune
        const int Archived = 5;
        private readonly ProjetSolutiosContext _context;
        UserManager usermanager;
        ProjectManager projectmanager = new ProjectManager();

        /// <summary>
        /// database context
        /// </summary>
        /// <param name="context"></param>
        public AdminController(ProjetSolutiosContext context)
        {
            this._context = context;
            usermanager = new UserManager(_context);
            projectmanager = new ProjectManager(_context);
        }

        //Page d'accueil pour admin
        [Authorize(Roles = "ADMIN")]
        public IActionResult Index()
        {
            return View(usermanager.showIndexProjet(usermanager.showAllProject()));
        }

        /// <summary>
        /// Logout du Admin
        /// </summary>
        /// <returns></returns>
        public IActionResult Logout()
        {
            usermanager.logoutUser(this.HttpContext);
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Voir les paramêtre du compte
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public IActionResult Param()
        {
            var Claim = ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");

            string id = Claim.Value;
            Users user = usermanager.FindUserByID(id);
            return View(user);
        }

        /// <summary>
        /// Voir les projection et en ajouter
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        public IActionResult Projection(int id)
        {
            Project p = projectmanager.getProjet(id);
            ViewData["tendance"] = projectmanager.graphiqueLigne(id);
            ViewData["id"] = id;

            return View(p.listProjectSoumission());


        }


        /// <summary>
        /// Ajouter un projet
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public IActionResult AddProjet()
        {
            ViewData["Users"] = usermanager.listeUser();
            return View();

        }


        /// <summary>
        /// Ajoute une projection dans un projet
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public IActionResult AddProjection(IFormCollection formCollection)
        {
            List<FollowInfo> list = new List<FollowInfo>();
            Project p = projectmanager.getProjet(Convert.ToInt32(formCollection["id"]));
            string[] Spending = formCollection["Spending"];
            string[] montant = formCollection["Montant"];
            string[] color = formCollection["color"];
            int count = 0;
            foreach (var item in p.listProjectSoumission())
            {
                FollowInfo f = new FollowInfo();
                f.amount = Convert.ToDouble(montant[count]);
                f.Spending = Spending[count];
                f.color = color[count];
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



        /// <summary>
        /// Ajouter une dépense
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public IActionResult AddExpense(IFormCollection formCollection)
        {


            //variables
            //liste de depenses
            List<subExpense> subExpenses = new List<subExpense>();//liste de sous-depenses
            int count = 0;//pour aller chercher chaque element du formulaire
            double costTotal = 0;//pour le prix d'une depenses(addition de toute les sous-depenses d'une depense)

            //récupération de valeurs
            Project p = projectmanager.getProjet(Convert.ToInt32(formCollection["id"]));//va chercher le bon projet dans la BD
            List<ExpenseInfo> expenseInfos = new List<ExpenseInfo>();
            int counter = 1;
            foreach (var expense in p.listProjectSoumission())
            {
                ExpenseInfo ex = new ExpenseInfo();
                FollowInfo[] follwlist = new JavaScriptSerializer().Deserialize<FollowInfo[]>(formCollection[counter.ToString()]);
                List<FollowInfo> subex = new List<FollowInfo>();
                double total = 0;
                foreach (var item in follwlist)
                {
                    total += item.amount;
                    subex.Add(item);
                }
                ex.Spending = expense.Spending;
                ex.subExpenses = subex;
                ex.amount = total;
                expenseInfos.Add(ex);
                counter++;
            }

            Expense saveEx = new Expense();
            saveEx.ExpenseDate = DateTime.Now;
            saveEx.JsonExpenseInfo = JsonConvert.SerializeObject(expenseInfos);

            ProjectExpense projectExpense = new ProjectExpense();
            projectExpense.PeExpenseId = Convert.ToInt32(formCollection["id"]);


            _context.Add(saveEx);
            _context.SaveChanges();
            projectExpense.PeExpenseId = saveEx.ExpenseId;

            projectExpense.PeProject = p;
            projectExpense.PeExpense = saveEx;
            _context.ProjectExpense.Add(projectExpense);
            _context.SaveChanges();


            return RedirectToAction("Projet", new { id = formCollection["id"] });
        }



        /// <summary>
        /// POST Ajouter un projet dans la DB
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public IActionResult Addprojet(IFormCollection formCollection)
        {
            ProjectFollowUp projectFollowUp = new ProjectFollowUp();
            ProjectExpense projectExpense = new ProjectExpense();

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

            List<ExpenseInfo> expenseInfo = new List<ExpenseInfo>();
            foreach (var item in soumission)
            {
                ExpenseInfo expenses = new ExpenseInfo();
                List<FollowInfo> subex = new List<FollowInfo>();
                FollowInfo ex = new FollowInfo();
                expenses.Spending = item.Spending;
                expenses.amount = 0;
                ex.Spending = "Title";
                ex.amount = 0;
                subex.Add(ex);
                expenses.subExpenses = subex;
                expenseInfo.Add(expenses);
            }

            Expense expense = new Expense();
            expense.ExpenseDate = DateTime.Now;
            expense.JsonExpenseInfo = JsonConvert.SerializeObject(expenseInfo);

            _context.Add(follow);
            _context.Add(expense);
            _context.SaveChanges();

            projectFollowUp.PfFollowUpId = follow.FuId;
            projectFollowUp.PfProject = p;

            projectExpense.PeExpenseId = expense.ExpenseId;
            projectExpense.PeProject = p;



            p.ProjectSoumission = JsonConvert.SerializeObject(soumission);
            Users user = new Users();
            user = usermanager.FindUserByID(formCollection["Users"]);
            projectmanager.addProjet(p);

            projectFollowUp.PfFollowUp = follow;
            projectExpense.PeExpense = expense;
            _context.ProjectFollowUp.Add(projectFollowUp);
            _context.ProjectExpense.Add(projectExpense);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Voir les archives
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public IActionResult Archive()
        {
            return View(usermanager.showArchivedProject());
        }

        public IActionResult ProjetArchive(int id)
        {
            Project p = projectmanager.getProjet(id);
            ViewData["tendance"] = projectmanager.graphiqueLigne(id);
            ViewData["date"] = projectmanager.date(id);
            ViewData["id"] = id;
            ViewData["graphbar"] = projectmanager.graphbar(id);
            ViewData["Nomdepense"] = projectmanager.nomdépense(id);
            ViewData["soumission"] = projectmanager.soumission(id);
            if (projectmanager.GetLastProjection(id) != null)
            {
                ViewData["Projection"] = JsonConvert.DeserializeObject<List<FollowInfo>>(projectmanager.GetLastProjection(id).FuInfo);
            }
            else
            {
                ViewData["Projection"] = p.listProjectSoumission();
            }

            return View(projectmanager.viewProjet(id));
        }

        /// <summary>
        /// Archive un projet
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public IActionResult ArchiverProjet(int id)
        {
            Project projet = projectmanager.getProjet(id);
            projet.ProjectStatus = Archived;
            _context.Update(projet);
            _context.SaveChanges();
            return RedirectToAction("Index");

        }

        /// <summary>
        /// Voir les détails d'un projet
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public IActionResult Projet(int id)
        {
            Project p = projectmanager.getProjet(id);
            ViewData["tendance"] = projectmanager.graphiqueLigne(id);
            ViewData["date"] = projectmanager.date(id);
            ViewData["id"] = id;
            ViewData["graphbar"] = projectmanager.graphbar(id);
            ViewData["Nomdepense"] = projectmanager.nomdépense(id);
            ViewData["margeS"] = projectmanager.Getmarge(id);
            ViewData["margeE"] = projectmanager.GetmargeEstime(id);
            ViewData["completion"] = projectmanager.getCompletion(id);
            ViewData["Test"] = projectmanager.GetAllProjection(id);
            if (projectmanager.GetLastProjection(id) != null)
            {
                ViewData["Projection"] = JsonConvert.DeserializeObject<List<FollowInfo>>(projectmanager.GetLastProjection(id).FuInfo);
            }
            else
            {
                ViewData["Projection"] = p.listProjectSoumission();
            }
            if (projectmanager.GetLastExpense(id) != null)
            {
                ViewData["Expense"] = JsonConvert.DeserializeObject<List<ExpenseInfo>>(projectmanager.GetLastExpense(id).JsonExpenseInfo);
            }
            else
            {
                ViewData["Expense"] = p.ListProjectExpense();
            }


            return View(projectmanager.viewProjet(id));
        }

        /// <summary>
        /// Voir les anciennes projection d'un projet
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public IActionResult ProjectionProjet(IFormCollection formCollection)
        {
            Project p = projectmanager.getProjet(Convert.ToInt32(formCollection["id"]));
            ViewData["tendance"] = projectmanager.graphiqueLigne(Convert.ToInt32(formCollection["id"]));
            ViewData["date"] = projectmanager.date(Convert.ToInt32(formCollection["id"]));
            ViewData["id"] = Convert.ToInt32(formCollection["id"]);
            ViewData["graphbar"] = projectmanager.OldGraph(Convert.ToInt32(formCollection["id"]), Convert.ToInt32(formCollection["projection"]));
            ViewData["Nomdepense"] = projectmanager.nomdépense(Convert.ToInt32(formCollection["id"]));
            ViewData["margeS"] = projectmanager.Getmarge(Convert.ToInt32(formCollection["id"]));
            ViewData["margeE"] = projectmanager.GetmargeEstime(Convert.ToInt32(formCollection["id"]));
            ViewData["completion"] = projectmanager.getCompletion(Convert.ToInt32(formCollection["id"]));
            ViewData["Test"] = projectmanager.GetAllProjection(Convert.ToInt32(formCollection["id"]));
            if (projectmanager.GetLastProjection(Convert.ToInt32(formCollection["id"])) != null)
            {
                ViewData["Projection"] = JsonConvert.DeserializeObject<List<FollowInfo>>(projectmanager.GetLastProjection(Convert.ToInt32(formCollection["id"])).FuInfo);
            }
            else
            {
                ViewData["Projection"] = p.listProjectSoumission();
            }
            if (projectmanager.GetLastExpense(Convert.ToInt32(formCollection["id"])) != null)
            {
                ViewData["Expense"] = JsonConvert.DeserializeObject<List<ExpenseInfo>>(projectmanager.GetLastExpense(Convert.ToInt32(formCollection["id"])).JsonExpenseInfo);
            }
            else
            {
                ViewData["Expense"] = p.ListProjectExpense();
            }


            return View("Projet", projectmanager.viewProjetHistorique(Convert.ToInt32(formCollection["id"]), Convert.ToInt32(formCollection["projection"])));
        }

        /// <summary>
        /// Voir le profil
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public IActionResult profil()
        {
            string user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(usermanager.FindUserByID(user));
        }

        /// <summary>
        /// VOir l'activity log
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public IActionResult Activitylog()
        {
            return View();
        }

        /// <summary>
        /// Voir les usagers
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public IActionResult Usagers()
        {
            return View(usermanager.listeUser());
        }

        /// <summary>
        /// Ajouter un usager 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [Authorize]
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

        /// <summary>
        /// Modifier un Usager
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public IActionResult EditUsers(Users user)
        {
            if (ModelState.IsValid)
            { 
                usermanager.UpdateUser(user);
                return Redirect("/Admin/Param");

            }
            else return RedirectToAction("Error");
        }

        /// <summary>
        /// Page d'erreur
        /// </summary>
        /// <returns></returns>
        public IActionResult Error()
        {
            return View("Error", new ErrorViewModel());
        }

        /// <summary>
        /// Changer le password
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [Authorize]
        public IActionResult ChangePasswordAdmin(IFormCollection form)
        {
            if (form != null)
            {
                ViewData["Id"] = form["Id"];
                return View(ViewData);
            }

            return Error();
        }

        /// <summary>
        /// Supprimer un projet de la DB
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        public IActionResult SupressionProjet(int id)
        {

            Project projet = projectmanager.getProjet(id);

            _context.Remove(projet);
            _context.SaveChanges();
            return RedirectToAction("Archive");

        }

        /// <summary>
        /// POST Changer le mot de passe d'un admin
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [Authorize]
        public IActionResult ChangerMdPAdmin(IFormCollection form)
        {
            Users user = usermanager.FindUserByID(form["id"]);
         

                if (form["NewPassword"] == form["ConfirmPassword"])
                {
                    user.UserMdp = form["NewPassword"];
                    usermanager.UpdateUser(user);
                    return RedirectToAction("Usagers");
                }
            return RedirectToAction("Usagers");
        }

        /// <summary>
        /// POST Changer le mot de passe d'un non-admin
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [Authorize]
        public IActionResult ChangerMdP(IFormCollection form)
        {
            Users user = usermanager.FindUserByID(form["id"]);
            if (form["OldPassWord"] == user.UserMdp)
            {

                if (form["NewPassword"] == form["ConfirmPassword"])
                {
                    user.UserMdp = form["NewPassword"];
                    usermanager.UpdateUser(user);
                    return RedirectToAction("Param");
                }

                return RedirectToAction("Param");
            }
            return RedirectToAction("Param");
        }
    }
}