using ProjectsApp.Models;
using ProjectsApp.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace ProjectsApp.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ProjectManagerController : AppController
    {
        // GET: ProjectManager
        public ActionResult AddNewProjectManager()
        {
            ViewBag.Url = "/Images/" + User.Identity.Name + "_profile.jpg";
            ProjectManager proj = new ProjectManager();
            var c = proj.FetchUsers();
            ViewBag.Users = c;
            return View();
        }

        [HttpPost]
        public ActionResult AddNewProjectManager(UserModel um)
        {
            ViewBag.Url = "/Images/" + User.Identity.Name + "_profile.jpg";
            if (ModelState.IsValid)
            {
                ProjectManager pm = new ProjectManager();
                pm.AddUser(um);

                return RedirectToAction("BrowseProjectManagers");
            }
            return View();

        }

        public ActionResult BrowseProjectManagers(string sortOrder, int? page, string searchString, string currentFilter)
        {

            ViewBag.Url = "/Images/" + User.Identity.Name + "_profile.jpg";
            ProjectManager proj = new ProjectManager();


            int ipp = 6;
            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var p = proj.FetchPM();


            int z = Convert.ToInt32(Session["SessionUserId"]);

            var onePageOfProducts = p.ToPagedList(pageNumber, ipp);
            ViewBag.OnePageOfProducts = onePageOfProducts;

            ViewBag.CurrentSort = sortOrder;

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name" : "";
            ViewBag.PhoneNumberSortParm = sortOrder == "PhoneNumber" ? "phn_desc" : "PhoneNumber";
            ViewBag.AddressSortParm = sortOrder == "Address" ? "address_desc" : "Address";
            ViewBag.EmailSortParm = sortOrder == "Email" ? "email_desc" : "Email";
            ViewBag.ProjectsOnSortParm = sortOrder == "ProjectsOn" ? "projectson_desc" : "ProjectsOn";
   

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                p = p.Where(s => s.Name.ToLower().Contains(searchString.ToLower())).ToList();
            }

            switch (sortOrder)
            {
                case "Name":
                    p = p.OrderByDescending(s => s.Name).ToList();
                    break;
                case "PhoneNumber":
                    p = p.OrderBy(s => s.PhoneNumber).ToList();
                    break;
                case "phn_desc":
                    p = p.OrderByDescending(s => s.PhoneNumber).ToList();
                    break;
                case "Address":
                    p = p.OrderBy(s => s.Address).ToList();
                    break;
                case "address_desc":
                    p = p.OrderByDescending(s => s.Address).ToList();
                    break;
                case "Email":
                    p = p.OrderBy(s => s.E_mail).ToList();
                    break;
                case "emaildesc":
                    p = p.OrderByDescending(s => s.E_mail).ToList();
                    break;
                case "ProjectsOn":
                    p = p.OrderBy(s => s.PojectsOn).ToList();
                    break;
                case "projectson_desc":
                    p = p.OrderByDescending(s => s.PojectsOn).ToList();
                    break;

                default:
                    p = p.OrderBy(s => s.Name).ToList();
                    break;
            }

            return View(p.ToPagedList(pageNumber, ipp));
        }
        public ActionResult Details(int id)
        {
            ViewBag.Url = "/Images/" + User.Identity.Name + "_profile.jpg";
            ProjectManager pm = new ProjectManager();

            Projects p = pm.GetProjectById(id);
            if (p == null)
            {
                return HttpNotFound();
            }
            ProjectModel proj = new ProjectModel();
            proj = pm.convertProjectForView(p);

            return View(proj);

        }

        public ActionResult Edit(int id)
        {
            ViewBag.Url = "/Images/" + User.Identity.Name + "_profile.jpg";
            
            DAL d = new DAL();
            UserModel user = d.getUserById(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            
            return View(user);
        }

        // POST: /User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserModel user)
        {
            ViewBag.Url = "/Images/" + User.Identity.Name + "_profile.jpg";
            ProjectManager pm = new ProjectManager();
            
            if (ModelState.IsValid)
            {

                pm.UpdateUser(user.Name, user.Address, user.PhoneNumber, user.E_mail, user.Id);
                TempData["Msg"] = "Data has been updated succeessfully";
                return RedirectToAction("BrowseProjectManagers");
            }
            return View(user);
        }

        public ActionResult Delete(int id)
        {
            ViewBag.Url = "/Images/" + User.Identity.Name + "_profile.jpg";
            DAL d = new DAL();
            UserModel p = d.getUserById(id);
            return View(p);


        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewBag.Url = "/Images/" + User.Identity.Name + "_profile.jpg";
            DAL d = new DAL();
            d.DeleteUser(id);
            return RedirectToAction("BrowseProjectManagers");

        }

        [HttpPost]
        public JsonResult SendMail(int id, string text)
        {

            DAL d = new DAL();
            string oks = "";
            bool ok = d.SendMessageToPM(id, text);
            if (ok == true)
                oks = "ok";
            else
                oks = "no";
            return Json(new { ok = oks }, JsonRequestBehavior.AllowGet);
        }
      


    }




}