using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
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
        private ProjetSolutiosContext solutiosContext;
        public UserManager()
        {
            this.solutiosContext = new ProjetSolutiosContext("Data Source = localhost; Initial Catalog = DbLogin; User ID = sa; Password = sql");
        }
        private Users FindUserByUserName(string userName)
        {
            Users usager = null;
            usager = this.solutiosContext.GetAllUsers().Find(u => u.UserName.Equals(userName));
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
            solutiosContext.Users.Add(user);
        }
        public void UserProjet(Users user)
        {
           Project listP = JsonConvert.DeserializeObject<Project>(user.UserProjet);

            solutiosContext.Project.Select("")
        }


    }
}
