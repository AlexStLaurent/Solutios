using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Solutios.Models;

namespace Solutios.Controllers
{
    public class LoginController : Controller
    {
        private UserManager usermanager;
        public LoginController()
        {
            this.usermanager = new UserManager();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Users user)
        {
            bool rep = await this.usermanager.loginUser(this.HttpContext, user.UserName, user.UserMdp);
            if (rep == true)
            {                
                return Redirect("/Home/Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Erreur de login.");
                return View();

            }
        }

    }    
}