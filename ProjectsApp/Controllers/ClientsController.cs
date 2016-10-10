using System.Web.Mvc;
using ProjectsApp.Models;
using System.Web.Security;
using ProjectsApp.Models.DB;
using System.Linq;
using PagedList;
using System;

namespace ProjectsApp.Controllers
{
    [Authorize]
    public class ClientsController : AppController
    {
        // GET: Clients
        public ActionResult BrowseClients(string sortOrder, int? page, string searchString, string currentFilter)
        {
            ViewBag.Url = "/Images/" + User.Identity.Name + "_profile.jpg";
            DAL d = new DAL();
            ViewBag.RoleId = d.getRoleID(CurrentUser.Role);

            ProjectManager pm = new ProjectManager();

            int ipp = 6;
            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)

            var cli = pm.GetClients(pm.FetchClients());
            var onePageOfProducts = cli.ToPagedList(pageNumber, ipp);
            ViewBag.OnePageOfProducts = onePageOfProducts;

            ViewBag.CurrentSort = sortOrder;

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name" : "";
            ViewBag.MainContactSortParm = sortOrder == "MainContact" ? "maincontact_desc" : "MainContact";
            ViewBag.AddressSortParm = sortOrder == "Address" ? "address_desc" : "Address";
            ViewBag.DateAddedSortParm = sortOrder == "DateAdded" ? "date_added_desc" : "DateAdded";
            ViewBag.PhoneNumberSortParm = sortOrder == "PhoneNumber" ? "phone_desc" : "PhoneNumber";

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
                cli = cli.Where(s => s.Name.ToLower().Contains(searchString.ToLower())).ToList();
            }

            switch (sortOrder)
            {
                case "Name":
                    cli = cli.OrderByDescending(s => s.Name).ToList();
                    break;
                case "DateAdded":
                    cli = cli.OrderBy(s => s.DateAdded).ToList();
                    break;
                case "date_added_desc":
                    cli = cli.OrderByDescending(s => s.DateAdded).ToList();
                    break;
                case "Address":
                    cli = cli.OrderBy(s => s.Address).ToList();
                    break;
                case "address_desc":
                    cli = cli.OrderByDescending(s => s.Address).ToList();
                    break;
                case "PhoneNumber":
                    cli = cli.OrderBy(s => s.PhoneNumber).ToList();
                    break;
                case "phone_desc":
                    cli = cli.OrderByDescending(s => s.PhoneNumber).ToList();
                    break;
               
                case "CreatedBy":
                    cli = cli.OrderBy(s => s.CreatedBy).ToList();
                    break;
                case "createdby_desc":
                    cli = cli.OrderByDescending(s => s.CreatedBy).ToList();
                    break;
                case "MainContact":
                    cli = cli.OrderBy(s => s.MainContact).ToList();
                    break;
                case "maincontact_desc":
                    cli = cli.OrderByDescending(s => s.MainContact).ToList();
                    break;

                default:
                    cli = cli.OrderBy(s => s.Name).ToList();
                    break;
            }

            return View(cli.ToPagedList(pageNumber, ipp));
        }


        public ActionResult AddNewClient()
        {
            ViewBag.Url = "/Images/" + User.Identity.Name + "_profile.jpg";
            DAL d = new DAL();
            ViewBag.RoleId = d.getRoleID(CurrentUser.Role);
            ProjectManager proj = new ProjectManager();
            var c = proj.FetchUsers();
            ViewBag.Users = c;
            return View();
        }

        [HttpPost]

        public ActionResult AddNewClient(ClientModel np, string mainContact)
        {
            ViewBag.Url = "/Images/" + User.Identity.Name + "_profile.jpg";
            DAL d = new DAL();
            ViewBag.RoleId = d.getRoleID(CurrentUser.Role);
            if (ModelState.IsValid)
            {
                ProjectManager pm = new ProjectManager();
                var c = pm.FetchUsers();
                ViewBag.Users = c;
                if (!pm.DoesNameExist(np.Name))
                {
                    np.DateAdded = System.DateTime.Now;
                    string s = CurrentUser.Sid;
                    pm.AddClient(np, mainContact, s);

                    return RedirectToAction("BrowseClients");
                }
                else
                {
                    ModelState.AddModelError("", "Client name already exists.");
                    return View(np);
                }
            }

            return View();
        }


        public ActionResult Edit(int id)
        {
            DAL d = new DAL();
            ViewBag.RoleId = d.getRoleID(CurrentUser.Role);
            ViewBag.Url = "/Images/" + User.Identity.Name + "_profile.jpg";
            ProjectManager proj = new ProjectManager();
            var c = proj.FetchUsers();
            ViewBag.Users = c;

            Clients cli = proj.GetClientById(id);
            if (cli == null)
            {
                return HttpNotFound();
            }
            ClientModel cm = new ClientModel();
            cm = proj.convertClientForView(cli);
            return View(cm);
        }


        // POST: /User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ClientModel user, string mainContact)
        {
            ViewBag.Url = "/Images/" + User.Identity.Name + "_profile.jpg";
            DAL d = new DAL();
            ViewBag.RoleId = d.getRoleID(CurrentUser.Role);
            ProjectManager pm = new ProjectManager();
            var c = pm.FetchUsers();
            ViewBag.Users = c;
            if (ModelState.IsValid)
            {

                pm.UpdateClient(user, CurrentUser.Sid);
                TempData["Msg"] = "Data has been updated succeessfully";
                return RedirectToAction("BrowseClients");
            }
            return View(user);
        }

        
        public ActionResult Delete(int id)
        {
            ViewBag.Url = "/Images/" + User.Identity.Name + "_profile.jpg";
            DAL d = new DAL();
            ViewBag.RoleId = d.getRoleID(CurrentUser.Role);
            ProjectManager pm = new ProjectManager();
            Clients cli = pm.GetClientById(id);
            ClientModel cm = new ClientModel();
            cm = pm.convertClientForView(cli);
            cm.MainContact = pm.GetCreator(Convert.ToInt32(cli.MainContact));
            cm.CreatedBy = pm.GetCreator(Convert.ToInt32(cli.CreatedBy));
            return View(cm);


        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewBag.Url = "/Images/" + User.Identity.Name + "_profile.jpg";
            ProjectManager pm = new ProjectManager();
            pm.DeleteClient(id);
            return RedirectToAction("BrowseClients");

        }
    }
}