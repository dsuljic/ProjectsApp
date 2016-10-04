using ProjectsApp.Models.StartupLogin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using ProjectsApp.Models;

namespace ProjectsApp.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        public ActionResult LogIn()
        {
            return View();
        }
        


        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            var model = new LogInModel
            {
                ReturnUrl = returnUrl
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Login(LogInModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            DAL d = new DAL();
            string img = "/Images/"+model.Username+"_profile.jpg";

            if (model.Password == d.GetUserPassword(model.Username))
            {
                var identity = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Name, d.GetUserScreenName(model.Username)),
                new Claim(ClaimTypes.Sid, d.GetUserId(model.Username).ToString()),
                new Claim(ClaimTypes.Role, d.GetUserRole(model.Username))
            },
                    "ApplicationCookie");

                var ctx = Request.GetOwinContext();
                var authManager = ctx.Authentication;

                authManager.SignIn(identity);

                return RedirectToAction("Home", "Home");
            }

            // user authN failed
            ModelState.AddModelError("", "Invalid Username or Password");
            return View();
        }


 


        private string GetRedirectUrl(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                return Url.Action("browseprojects", "projects");
            }

            return returnUrl;
        }

        public ActionResult LogOut()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignOut("ApplicationCookie");
            return RedirectToAction("login", "auth");
        }
    }
}