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


        public async Task<bool> loginUser(HttpContext context, string email, string password)
        {
            Users user = this.FindUserByUserName(email);
            if (user != null && user.UserMdp.Equals(password, StringComparison.CurrentCulture))
            {
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, user.UserFirstName + " " + user.UserName));
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()));
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


    }
}
