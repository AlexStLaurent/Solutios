using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Solutios.Models;

namespace Solutios.Controllers
{
    public class LoginController : Controller
    {
        private UserManager usermanager;
        private readonly ProjetSolutiosContext _context;
        private readonly IConfiguration Configuration;
        public LoginController( ProjetSolutiosContext context)
        {

            _context = context;
            this.usermanager = new UserManager(_context);

        }

        [HttpPost]
        public async Task<IActionResult> Login(Users user)
        {
            string rep = await this.usermanager.loginUser(this.HttpContext, user.UserEmail, user.UserMdp);
            if (rep == "ADMIN")
            {
                return RedirectToAction("Index", "Admin");
            }
            else if (rep == "MANAGER")
            {
                return RedirectToAction("Index", "User");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Erreur de login.");
                return  RedirectToAction("Login", "Home");

            }
        }
        
        //[HttpPost]
        //public async Task<IActionResult> forgotPassword()
        //{
            
        //}

    }    
}