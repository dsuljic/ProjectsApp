using System.Web.Mvc;
using ProjectsApp.Models;
using ProjectsApp.Models.DB;
using System.Linq;
using PagedList;
using System;


namespace ProjectsApp.Controllers
{
    
    public class ProjectsController : AppController
    {


        
        public ActionResult BrowseProjects(string sortOrder, int? page, string searchString, string currentFilter)
        {

            ViewBag.Url = "/Images/" + User.Identity.Name + "_profile.jpg";
            DAL d = new DAL();
            ViewBag.RoleId = d.getRoleID(CurrentUser.Role);
            ProjectManager proj = new ProjectManager();


            int ipp = 6;
            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var p = proj.GetProjects(proj.FetchProjects());


            int z = Convert.ToInt32(Session["SessionUserId"]);

            var onePageOfProducts = p.ToPagedList(pageNumber, ipp);
            ViewBag.OnePageOfProducts = onePageOfProducts;

            ViewBag.CurrentSort = sortOrder;

            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "Title" : "";
            ViewBag.DateFromSortParm = sortOrder == "Date_From" ? "date_desc" : "Date_From";
            ViewBag.DateToSortParm = sortOrder == "Date_To" ? "date_to_desc" : "Date_To";
            ViewBag.DateCreatedSortParm = sortOrder == "Date_Created" ? "date_created_desc" : "Date_Created";
            ViewBag.CapSortParm = sortOrder == "Cap" ? "cap_desc" : "Cap";
            ViewBag.ClientIdSortParm = sortOrder == "Client_Id" ? "client_id_desc" : "Client_Id";
            ViewBag.IsActiveSortParm = sortOrder == "IsActive" ? "isactive_desc" : "IsActive";
            ViewBag.CreatedBySortParm = sortOrder == "CreatedBy" ? "createdby_desc" : "CreatedBy";

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
                p = p.Where(s => s.Title.ToLower().Contains(searchString.ToLower())).ToList();
            }

            switch (sortOrder)
            {
                case "Title":
                    p = p.OrderByDescending(s => s.Title).ToList();
                    break;
                case "Date_From":
                    p = p.OrderBy(s => s.Date_From).ToList();
                    break;
                case "date_desc":
                    p = p.OrderByDescending(s => s.Date_From).ToList();
                    break;
                case "Date_To":
                    p = p.OrderBy(s => s.Date_To).ToList();
                    break;
                case "date_to_desc":
                    p = p.OrderByDescending(s => s.Date_To).ToList();
                    break;
                case "Date_Created":
                    p = p.OrderBy(s => s.Date_Created).ToList();
                    break;
                case "date_created_desc":
                    p = p.OrderByDescending(s => s.Date_Created).ToList();
                    break;
                case "Cap":
                    p = p.OrderBy(s => s.Cap).ToList();
                    break;
                case "cap_desc":
                    p = p.OrderByDescending(s => s.Cap).ToList();
                    break;
                case "IsActive":
                    p = p.OrderBy(s => s.IsActive).ToList();
                    break;
                case "isactive_desc":
                    p = p.OrderByDescending(s => s.IsActive).ToList();
                    break;
                case "Client_Id":
                    p = p.OrderBy(s => s.Client_Id).ToList();
                    break;
                case "client_id_desc":
                    p = p.OrderByDescending(s => s.Client_Id).ToList();
                    break;
                case "CreatedBy":
                    p = p.OrderBy(s => s.CreatedBy).ToList();
                    break;
                case "createdby_desc":
                    p = p.OrderByDescending(s => s.CreatedBy).ToList();
                    break;

                default:
                    p = p.OrderBy(s => s.Title).ToList();
                    break;
            }

            return View(p.ToPagedList(pageNumber, ipp));
        }

        public ActionResult AddNewProject()
        {
            ViewBag.Url = "/Images/" + User.Identity.Name + "_profile.jpg";
            DAL d = new DAL();
            ViewBag.RoleId = d.getRoleID(CurrentUser.Role);
            ProjectManager proj = new ProjectManager();
            var c = proj.FetchClients();
            ViewBag.Clients = c;
            return View();
        }

        [HttpPost]

        public ActionResult AddNewProject(ProjectModel np, string selectClient)
        {
            ViewBag.Url = "/Images/" + User.Identity.Name + "_profile.jpg";
            DAL d = new DAL();
            ViewBag.RoleId = d.getRoleID(CurrentUser.Role);
            if (ModelState.IsValid)
            {

                ProjectManager pm = new ProjectManager();
                var c = pm.FetchClients();
                ViewBag.Clients = c;
                if (!pm.DoesTitleExist(np.Title))
                {
                    if (!pm.CheckTitle(np.Title))
                    {

                        ModelState.AddModelError("", "Project title is too long.");
                        return View(np);
                    }
                    else
                        if (!pm.CheckCap(np.Cap))
                    {
                        ModelState.AddModelError("", "Cap is too large.");
                        return View(np);
                    }
                    else
                        if (!pm.CheckCapFormat(np.Cap))
                    {
                        ModelState.AddModelError("", "Cap must be a number");
                        return View(np);
                    }
                    string s = CurrentUser.Sid;
                    pm.AddProject(np, selectClient, s);
                    return RedirectToAction("BrowseProjects");
                    
                }


                else
                {
                    ModelState.AddModelError("", "Project title already taken.");
                    return View(np);

                }

            }
            return View(np);
    }


        public ActionResult Details(int id)
        {
            ViewBag.Url = "/Images/" + User.Identity.Name + "_profile.jpg";
            DAL d = new DAL();
            ViewBag.RoleId = d.getRoleID(CurrentUser.Role);
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
            DAL d = new DAL();
            ViewBag.RoleId = d.getRoleID(CurrentUser.Role);
            ViewBag.Url = "/Images/" + User.Identity.Name + "_profile.jpg";

            ProjectManager proj = new ProjectManager();
            var c = proj.FetchClients();
            ViewBag.Clients = c;

            Projects user = proj.GetProjectById(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ProjectModel pm = new ProjectModel();
            pm = proj.convertProjectForView(user);
            return View(pm);
        }

        // POST: /User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProjectModel user, string selectClient, string selectActive)
        {
            ViewBag.Url = "/Images/" + User.Identity.Name + "_profile.jpg";
            DAL d = new DAL();
            ViewBag.RoleId = d.getRoleID(CurrentUser.Role);
            ProjectManager pm = new ProjectManager();
            var c = pm.FetchClients();
            ViewBag.Clients = c;
            if (ModelState.IsValid)
            {

                pm.UpdateProject(user,selectClient, CurrentUser.Sid, selectActive);
                TempData["Msg"] = "Data has been updated succeessfully";
                return RedirectToAction("BrowseProjects");
            }
            return View(user);
        }

        
        public ActionResult Delete(int id)
        {
            ViewBag.Url = "/Images/" + User.Identity.Name + "_profile.jpg";
            DAL d = new DAL();
            ViewBag.RoleId = d.getRoleID(CurrentUser.Role);
            ProjectManager pm = new ProjectManager();
            Projects proj = pm.GetProjectById(id);
            ProjectModel p = new ProjectModel();
            p = pm.convertProjectForView(proj);
            return View(p);


        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewBag.Url = "/Images/" + User.Identity.Name + "_profile.jpg";

            ProjectManager pm = new ProjectManager();
            pm.DeleteProject(id);
            return RedirectToAction("BrowseProjects");

        }


        public ActionResult Added()
        {
            ViewBag.Url = "/Images/" + User.Identity.Name + "_profile.jpg";


            return View();
        }
    }
}