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
        [HttpPost]
        public async Task<IActionResult> Login(Users user)
        {
            if (user.UserEmail == "test@test.com" && user.UserMdp == "test2")
            {
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, user.UserEmail));
                var principal = new ClaimsPrincipal(identity);
                //Creation du jeton
                await this.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Index", "Admin");
            }
            else

            {
                return RedirectToAction("Index", "Home");
            }

        }

    }    
}