using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectsApp.Models;
using ProjectsApp.Models.DB;
using System.Data.SqlClient;
using System.Web.Security;
using ProjectsApp.Models.StartupLogin;
using System.Security.Claims;
using PagedList;

namespace ProjectsApp.Controllers
{
   // [InitializeSimpleMembership]

    
    public class UserController : AppController
    {
        // GET: User

     
        public ActionResult MyProfile(HttpPostedFileBase file)
        {
            ViewBag.Url = "/Images/" + User.Identity.Name + "_profile.jpg";
            DAL dal = new Models.DAL();
            UserModel um = dal.getUserById(Convert.ToInt32(CurrentUser.Sid));
            ViewBag.Name = um.Name;
            ViewBag.Address = um.Address;
            ViewBag.PhoneNumber = um.PhoneNumber;
            ViewBag.E_mail = um.E_mail;
            ViewBag.RoleId = dal.getRoleID(CurrentUser.Role);
            return View();
            
        }


        



        public ActionResult LogOut()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignOut("ApplicationCookie");
            return RedirectToAction("login", "auth");
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        

       // [HttpPost]

      
        //public ActionResult MyProfile(HttpPostedFileBase file)
        //{
        //    ViewBag.Url = "/Images/" + User.Identity.Name + "_profile.jpg";
        //    DAL dal = new Models.DAL();
        //    UserModel um = dal.getUserById(Convert.ToInt32(CurrentUser.Sid));
        //    ViewBag.Name = um.Name;
        //    ViewBag.Address = um.Address;
        //    ViewBag.PhoneNumber = um.PhoneNumber;
        //    ViewBag.E_mail = um.E_mail;
        //    return View();
        //}

        [HttpPost]
            public ActionResult ChangeProfile(HttpPostedFileBase file)
        {
            ViewBag.Url = "/Images/" + User.Identity.Name + "_profile.jpg";
            if (file != null && file.ContentLength > 0)
                try
                {
                    string path = System.IO.Path.Combine(Server.MapPath("~/Images/" + User.Identity.Name + "_profile.jpg"));
                    file.SaveAs(path);
                    ViewBag.Message = "File uploaded successfully";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }
            return RedirectToAction("MyProfile");

        
    }

       [HttpPost]
       public JsonResult ChangePass(string old_pass, string new_pass)
        {

            ProjectManager pm = new ProjectManager();
            string oks = "";
            bool ok = pm.ChangePassword(old_pass, new_pass, Convert.ToInt32(CurrentUser.Sid));
            if (ok == true)
                oks = "ok";
            else
                oks = "no";
            return Json(new { ok = oks}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SendPass(string email)
        {

            DAL d = new DAL();
            string oks = "";
            bool ok = d.SendEmailWithPass(email);
            if (ok == true)
                oks = "ok";
            else
                oks = "no";
            return Json(new { ok = oks }, JsonRequestBehavior.AllowGet);
        }



        public JsonResult UpdateUser(string name, string address, string phone, string email)
    {

        ProjectManager pm = new ProjectManager();
        string oks = "";
        bool ok = pm.UpdateUser(name, address, phone, email, Convert.ToInt32(CurrentUser.Sid));
        if (ok == true)
            oks = "ok";
        else
            oks = "no";
        return Json(new { ok = oks }, JsonRequestBehavior.AllowGet);
    }
}

}