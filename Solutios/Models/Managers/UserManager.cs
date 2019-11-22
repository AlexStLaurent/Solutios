using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using Newtonsoft.Json;
using Solutios.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Solutios.Models
{
    public class UserManager
    {
        private readonly ProjetSolutiosContext solutiosContext;
        private ProjectManager ProjectManager;



        public UserManager(ProjetSolutiosContext context)
        {
            this.solutiosContext = context;
            ProjectManager = new ProjectManager(context);
        }

        private Users FindUserByUserName(string email)
        {
            Users usager = null;
            usager = this.solutiosContext.Users.ToList().Find(u => u.UserEmail.Equals(email));
            return usager;
        }
        public Users FindUserByID(string id)
        {
            Users usager = null;
            int d = int.Parse(id);
            usager = this.solutiosContext.Users.ToList().Find(u => u.UserId.Equals(d));
            return usager;
        }


        public async Task<string> loginUser(HttpContext context, string email, string password)
        {
            Users user = this.FindUserByUserName(email);
            if (user != null && user.UserMdp.Equals(password, StringComparison.CurrentCulture))
            {
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, user.UserFirstName + " " + user.UserName));
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()));
                identity.AddClaim(new Claim(ClaimTypes.Role, user.UserRole.ToString()));
                var principal = new ClaimsPrincipal(identity);
                // création du jeton d'authentification de type AuthenticationTicket
                await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return user.UserRole;
            }
            else
                return "null";
        }

        public void logoutUser()
        {

        }
      public void AddUser(Users user)
        {
           
                solutiosContext.Add(user);
                solutiosContext.SaveChanges();
            
            
        }

        public List<Project> UserProjet(Users userr)
        {
            List<Project> listeProjetUser = new List<Project>();
            if(userr.UserProjet != null)
            {
                List<UserProjet> n = JsonConvert.DeserializeObject<List<UserProjet>>(userr.UserProjet);
                foreach (UserProjet user_p in n)
                {
                    Project p = solutiosContext.Project.Find(user_p.ProjectId);
                    listeProjetUser.Add(p);
                }
            }          

            return listeProjetUser;
        }

        public List<Users> listeUser()
        {
            return solutiosContext.Users.ToList();
        }

        //public users finduser (int id)
        //{

        //}

        public List<Project> showAllProject()
        {
            return solutiosContext.Project.ToList();
        }

        public string SerialiseUserProjet(List<UserProjet> userProjets)
        {
            return (JsonConvert.SerializeObject(userProjets));
        }

        public List<ViewIndex> showIndexProjet(List<Project> projects)
        {
            List<ViewIndex> viewIndexs = new List<ViewIndex>();
            foreach(var item in projects)
            {
                ViewIndex view = new ViewIndex();
                view.ProjectId = item.ProjectId;
                view.ProjectFin = Convert.ToDateTime(item.ProjectFin);
                view.ProjectDebut = Convert.ToDateTime(item.ProjectDebut);
                view.ProjectName = item.ProjectName;

                view.margeprojeter = ProjectManager.Getmarge(item.ProjectId);
                view.margesoumis = ProjectManager.Getmarge(item.ProjectId);
                view.Lastupdate = Convert.ToDateTime(ProjectManager.GetLastProjection(item.ProjectId).FuDate).ToShortDateString();
                view.echeance = view.ProjectFin.ToShortDateString();
                view.graph = ProjectManager.graphbar(item.ProjectId);

                viewIndexs.Add(view);
            }
            return viewIndexs;
        }


    }
}
