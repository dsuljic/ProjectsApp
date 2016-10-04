using System.Web.Mvc;
using ProjectsApp.Models;
using System.Security.Claims;
using System;
using PagedList;
using System.Linq;
using ProjectsApp.Models.DB;
using System.Collections.Generic;

namespace ProjectsApp.Controllers
{
    [AllowAnonymous]
    public class HomeController : AppController
    {
        public ActionResult Index()
        {
            ViewBag.Url = "/Images/" + User.Identity.Name + "_profile.jpg";

            return View();
        }
        [HttpGet]

        public JsonResult Active()
        {
            ProjectManager pm = new ProjectManager();
            List<Projects> p = pm.FetchProjects();
            int a = 0;
            int na = 0;
            for (int i = 0; i < p.Count; i++)
            {
                if (p[i].IsActive == 1)
                {
                    a++;
                }

                else
                {
                    na++;
                }
            }
            return Json(new { a = a, na = na }, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult MonthCap()
        //{
        //    ProjectManager pm = new ProjectManager();

        //    List<Projects> ps = pm.FetchProjects();
        //    List<double> cap = new List<double>(12);
        //    for (int i =0;i<ps.Count;i++)
        //    {
        //        for (int j=0;j<12;j++)
        //        {
        //            DateTime start = new DateTime();
        //            start.Month = 10;


        //        }
        //    }
        //}


        public ActionResult chartjs()
        {
            return View();
        }
        public ActionResult Home()
        {
            ViewBag.Url = "/Images/" + User.Identity.Name + "_profile.jpg";
            ProjectManager pm = new ProjectManager();
            
            ViewBag.TP = pm.FetchProjects().Count();
            ViewBag.TC = pm.FetchClients().Count();
            ViewBag.TPM = pm.getUsersCount();
            ViewBag.TCap = pm.TotalCap();

            return View();
        }

        [HttpGet]
        public JsonResult GetBestPM()
        {
            ProjectManager pm = new ProjectManager();
            List<string> ps = pm.GetPmOnProjects();
            List<int> psN = pm.GetPmProjectCount();

            return Json(new { ps = ps, psN = psN }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult GetWorkers()
        {
            ProjectManager pm = new ProjectManager();
            var pms = pm.GetProjects(pm.FetchProjects()).ToList();

            pms = pms.OrderBy(p => p.Cap).ToList();
            List<int> eng = new List<int>(5);
            List<int> it = new List<int>(5);
            List<int> mw = new List<int>(5);
            List<int> ar = new List<int>(5);
            List<string> names = new List<string>(5);
            for (int i = 0; i < 5; i++)
            {
                names.Add((string)pms[i].Title);
                eng.Add((int)pms[i].EngineersOnProject);
                it.Add((int)pms[i].ITsOnProject);
                mw.Add((int)pms[i].ManualWorkersOnProject);
                ar.Add((int)pms[i].ArchitectsOnProject);


            }
            return Json(new { eng = eng, it = it, mw = mw, ar = ar, names = names }, JsonRequestBehavior.AllowGet);
        }



        public ActionResult EmptyPage(string sortOrder, int? page, string searchString, string currentFilter)
        {

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
            ProjectManager proj = new ProjectManager();
            var c = proj.FetchClients();
            ViewBag.Clients = c;
            return View();
        }

        [HttpPost]

        public ActionResult AddNewProject(ProjectModel np, string selectClient)
        {
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
                        ModelState.AddModelError("", "Cap mus be a number");
                        return View(np);
                    }

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



        public JsonResult getCaps()
        {
            ProjectManager pm = new ProjectManager();
            var pms = pm.GetProjects(pm.FetchProjects()).ToList();
            List<double> caps = new List<double>(13);
            for (int i = 0; i < 13; i++)
            {
                caps.Add(0);
            }
            for (int i = 0; i < pms.Count; i++)
            {
                for (int j = 1; j <= 12; j++)
                {
                    DateTime date1 = new DateTime(2016, j, 1);
                    DateTime date2=new DateTime(2016,1,1);
                    switch (j)
                    {
                        case 1:
                        date2 = new DateTime(2016, j, 31);
                            break;
                        case 2:
                            date2 = new DateTime(2016, j, 29);
                            break;
                        case 3:
                            date2 = new DateTime(2016, j, 31);
                            break;
                        case 4:
                            date2 = new DateTime(2016, j, 30);
                            break;
                        case 5:
                            date2 = new DateTime(2016, j, 31);
                            break;
                        case 6:
                            date2 = new DateTime(2016, j, 30);
                            break;
                        case 7:
                            date2 = new DateTime(2016, j, 31);
                            break;
                        case 8:
                            date2 = new DateTime(2016, j, 31);
                            break;
                        case 9:
                            date2 = new DateTime(2016, j, 30);
                            break;
                        case 10:
                            date2 = new DateTime(2016, j, 31);
                            break;
                        case 11:
                            date2 = new DateTime(2016, j, 30);
                            break;
                        case 12:
                            date2 = new DateTime(2016, j, 31);
                            break;
                        default:
                            break;
                    }
                    if ((date1 <= pms[i].Date_From && date2 >= pms[i].Date_From) || (date1 <= pms[i].Date_To && date2 >= pms[i].Date_To) || (pms[i].Date_From < date1 && pms[i].Date_To > date2))
                        caps[j] = caps[j] + Convert.ToDouble(pms[i].Cap);
                }
                }
            return Json(new { caps=caps }, JsonRequestBehavior.AllowGet);



        }
        }

    }

