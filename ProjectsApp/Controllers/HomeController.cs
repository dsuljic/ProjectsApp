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
            DAL d = new DAL();
            ViewBag.RoleId = d.getRoleID(CurrentUser.Role);
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

