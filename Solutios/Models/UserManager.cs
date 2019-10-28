using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Solutios.Models
{
    public class UserManager
    {
        private readonly ProjetSolutiosContext solutiosContext = new ProjetSolutiosContext();

        public UserManager()
        {

        }

        private Users FindUserByUserName(string userName)
        {
            Users usager = null;
            //usager = this.solutiosContext.GetAllUsers().Find(u => u.UserName.Equals(userName));
            return usager;
        }
        
        public async Task<bool> loginUser(HttpContext context, string userName, string password)
        {
            Users user = this.FindUserByUserName(userName);
            if (user != null && user.UserMdp.Equals(password))
            {
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, user.UserEmail));
                var principal = new ClaimsPrincipal(identity);
                // création du jeton d'authentification de type AuthenticationTicket
                await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return true;
            }
            else
                return false;
        }

        public void logoutUser()
        {

        }
      public void AddUser(Users user)
        {
            solutiosContext.Add(user);
            solutiosContext.SaveChanges();
        }

        public List<Project> UserProjet(Users user)
        {
            List<Project> listeProjetUser = new List<Project>();
            List<UserProjet> n = JsonConvert.DeserializeObject<List<UserProjet>>(user.UserProjet.ToString());
            foreach(UserProjet user_p in n)
            {
                Project p = solutiosContext.Project.Find(user_p.ProjectId);
                listeProjetUser.Add(p);
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

        }

    }
}
