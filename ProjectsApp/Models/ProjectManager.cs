using ProjectsApp.Models.DB;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System;
using ProjectsApp.Models;
using System.Web.Mvc;

namespace ProjectsApp.Models
{
    public class ProjectManager
    {
        public void AddProject(ProjectModel p, string s, string c)
        {
            using (ProjectsDBEntities db = new ProjectsDBEntities())
            {

                Projects proj = new Projects();
                proj.Title = p.Title;
                proj.Date_Created = DateTime.Now;
                proj.Date_From = p.Date_From;
                proj.Date_To = p.Date_To;
                proj.Cap = Convert.ToDecimal(p.Cap);
                proj.IsActive = 1;
                proj.IsDeleted = 0;
                proj.Client_Id = Convert.ToInt32(s);
                proj.CreatedBy = Convert.ToInt32(c);
                proj.ITsOnProject = 4;
                proj.ManualWorkersOnProject = 30;
                proj.EngineersOnProject = 5;
                proj.ArchitectsOnProject = 3;
                proj.EstimatedEarnings = 5000;
                proj.RiskFactor = 12;
                db.Projects.Add(proj);
                db.SaveChanges();


            }

        }

        public void AddClient(ClientModel p, string maincontact, string sid)
        {
            using (ProjectsDBEntities db = new ProjectsDBEntities())
            {

                Clients cli = new Clients();
                cli.Name = p.Name;
                cli.DateAdded = p.DateAdded;
                cli.MainContact = Convert.ToInt32(maincontact);
                cli.CreatedBy = Convert.ToInt32(sid);
                cli.PhoneNumber = p.PhoneNumber;
                cli.Address = p.Address;
                cli.IsDeleted = 0;

                db.Clients.Add(cli);
                db.SaveChanges();


            }

        }

        public bool DoesTitleExist(string newtitle)
        {
            using (ProjectsDBEntities db = new ProjectsDBEntities())
            {
               string  currenttitle = db.Projects.Where(o => o.Title.Equals(newtitle)).ToString();
                if (newtitle == currenttitle)
                    return true;
                return false;
            }


        }

        public bool DoesNameExist(string newtitle)
        {
            using (ProjectsDBEntities db = new ProjectsDBEntities())
            {
                return db.Clients.Where(o => o.Name.Equals(newtitle)).Any();
            }


        }

        public bool CheckPhoneNumber(string pn)
        {
            if (pn.Length > 20)
                return false;
            return true;
        }
        public bool CheckAddress(string ad)
        {
            if (ad.Length > 35)
                return false;
            return true;
        }
        public bool CheckName( string name)
        {
            if (name.Length > 50)
                return false;
            return true;
        }

        public bool CheckTitle(string title)
        {
            if (title.Length > 30)
                return false;
            return true;
        }
        
        public bool CheckCap( string cap)
        {
            if (cap.Length > 12)
                return false;
            return true;
        }
       
        public bool CheckCapFormat(string cap)
        {
            decimal dec;
            if (Decimal.TryParse(cap, out dec))
                return true;
            return false;
        }

        public Projects GetProjectById( int  id)
        {
            using (ProjectsDBEntities db = new ProjectsDBEntities())
            {
                Projects p = db.Projects.Find(id);
                return p;
            }


        }

        public int GetUserId( string username)
        {
            using (ProjectsDBEntities db = new ProjectsDBEntities())
            {
                int id = db.Database.ExecuteSqlCommand("select Id from Users where Username = " + username);

                return id;
            }

        }

        public List<Projects> FetchProjects()
        {
            using (ProjectsDBEntities db = new ProjectsDBEntities())
            {
                List<Projects> dels = db.Projects.Where(p=>p.IsDeleted!=1).ToList();
         
                return dels;
            }


        }


        public List<ProjectModel> GetProjects(List<Projects> cli)
        {
            List<ProjectModel> cm = new List<ProjectModel>();

            foreach (Projects c in cli)
            {
                ProjectModel temp = new ProjectModel();
                temp = convertProjectForView(c);
     
                cm.Add(temp);
            }
            
            return cm;
        }

        public double TotalCap()
        {
            double cap = 0;

            List<Projects> p = FetchProjects();
            for (int i = 0; i < p.Count; i++)
                if (p[i].Date_To>=DateTime.Now && p[i].Date_From<=DateTime.Now)
                cap = cap + Convert.ToDouble(p[i].Cap);

            return cap;
        }


        public void UpdateProject(ProjectModel user, string selectClient,string sid, string active)
        {
            using (ProjectsDBEntities db = new ProjectsDBEntities())
            {
                db.Entry(convertProjectForDB(user, selectClient, sid, active)).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void DeleteProject(int id)
        {
            using (ProjectsDBEntities db = new ProjectsDBEntities())
            {
                db.Database.ExecuteSqlCommand("Update Projects Set IsDeleted = 1, IsActive = 0 where id =" + id);
                db.SaveChanges();
             
            }
        }
      
        public string GetMainContact(int a)
        {
            using (ProjectsDBEntities db = new ProjectsDBEntities())
            {
                var user = db.Users.Find(a);
                string maincontact = user.Name;

                return maincontact;
             }
        }

        public List<Clients> FetchClients()
        {
            using (ProjectsDBEntities db = new ProjectsDBEntities())
            {
                List<Clients> dels = db.Clients.SqlQuery("Select * from Clients where IsDeleted = 1").ToList();      ////////////dodati jos jednu listu i vracati ProjectModel listu
                List<Clients> clis = db.Clients.ToList().Except(dels).ToList();

                return clis;
            }
        }
        
        public List<ClientModel> GetClients(List<Clients> cli)
        {
            List<ClientModel> cm = new List<ClientModel>();
            
            foreach (Clients c in cli)
            {
                ClientModel temp = new ClientModel();
                temp.Id = c.Id.ToString();
                temp.Name = c.Name;
                temp.DateAdded = c.DateAdded;
                temp.MainContact = GetMainContact(c.MainContact);
                temp.CreatedBy = GetCreator(c.CreatedBy);
                temp.PhoneNumber = c.PhoneNumber;
                temp.Address = c.Address;
                cm.Add(temp);
            }
            
            return cm;
        }
        
        public string GetCreator(int id)
        {
            using (ProjectsDBEntities db = new ProjectsDBEntities())
            {
                var user = db.Users.Find(id);
                string creator = user.Name;
                return creator;




            }
        }

        public string GetClient (int id)
        {
            using (ProjectsDBEntities db = new ProjectsDBEntities())
            {
                var client = db.Clients.Find(id);
                return client.Name;
            }
        }

        public List<Users> FetchUsers()
        {
            using (ProjectsDBEntities db = new ProjectsDBEntities())
            {

                return db.Users.ToList();
            }
        }

        public Clients GetClientById(int id)
        {
            using (ProjectsDBEntities db = new ProjectsDBEntities())
            {
                Clients cli = db.Clients.Find(id);
                return cli;

            }
        }

        public ClientModel convertClientForView( Clients cl)
        {
            ClientModel cm = new ClientModel();
            cm.Address = cl.Address;
            cm.CreatedBy = cl.CreatedBy.ToString();
            cm.MainContact = cl.MainContact.ToString();
            cm.Name = cl.Name;
            cm.DateAdded = cl.DateAdded;
            cm.Id = cl.Id.ToString();
            cm.PhoneNumber = cl.PhoneNumber;

            return cm;
        }

        public Clients convertClientForDB(ClientModel cl, string sid)
        {
            Clients client = new Clients();
            client.Id = Convert.ToInt32(cl.Id);
            client.MainContact = Convert.ToInt32(cl.MainContact);
            client.Name = cl.Name;
            client.Address = cl.Address;
            client.PhoneNumber = cl.PhoneNumber;
            client.CreatedBy = Convert.ToInt32(sid);
            client.DateAdded = cl.DateAdded;

            return client;
        }

        public ProjectModel convertProjectForView(Projects pr)
        {
            ProjectModel pm = new ProjectModel();
            pm.Id = pr.Id.ToString();
            pm.Title = pr.Title;
            pm.Date_Created = pr.Date_Created;
            pm.Date_From = pr.Date_From;
            pm.Date_To = pr.Date_To;
            pm.Cap = pr.Cap.ToString();
            pm.Client_Id = GetClient(pr.Client_Id);
            pm.CreatedBy = GetCreator(pr.CreatedBy);
            pm.ITsOnProject = (int)pr.ITsOnProject;
            pm.ManualWorkersOnProject = (int)pr.ManualWorkersOnProject;
            pm.EngineersOnProject = (int)pr.EngineersOnProject;
            pm.ArchitectsOnProject = (int)pr.ArchitectsOnProject;


            if (pr.IsActive == 0)
                pm.IsActive = "No";
            else
                pm.IsActive = "Yes";

            return pm;
        }

        public int getUsersCount()
        {
            using (ProjectsDBEntities db = new ProjectsDBEntities())
                return db.Users.Count();
        }


        public Projects convertProjectForDB ( ProjectModel cl, string selectClient, string sid, string selectActive)
        {
            Projects client = new Projects();
            client.Id = Convert.ToInt32(cl.Id);
            client.Title = cl.Title;
            client.Date_Created = cl.Date_Created;
            client.Cap = Convert.ToDecimal(cl.Cap);
            if (selectActive == "Yes")
                client.IsActive = 1;
            else
                client.IsActive = 0;
           
            client.CreatedBy = Convert.ToInt32(sid);
            client.Date_From = cl.Date_From;
            client.Date_To = cl.Date_To;
            client.Client_Id = Convert.ToInt32(selectClient);

            return client;
        }
        
        public void UpdateClient (ClientModel c, string sid)
        {
            using (ProjectsDBEntities db = new ProjectsDBEntities())
            {
                db.Entry(convertClientForDB(c,sid)).State = EntityState.Modified;
                
                db.SaveChanges();

            }
        }

        public void UpdateUser(UserModel c, string sid)
        {
            using (ProjectsDBEntities db = new ProjectsDBEntities())
            {
               
               
            }
        }




        public void DeleteClient(int id)
        {
            using (ProjectsDBEntities db = new ProjectsDBEntities())
            {
                db.Database.ExecuteSqlCommand("Update Clients Set IsDeleted = 1 where id =" + id);
                db.SaveChanges();

            }
        }
        
        public int GetNumberOfProjects()
        {
            using (ProjectsDBEntities db = new ProjectsDBEntities())
            {
                int count =Convert.ToInt32(db.Projects.SqlQuery("select count(Id) from Projects where IsDeleted <> 1"));
                return count;
            }
        }

        public int GetNumberOfActiveProjects()
        {
            using (ProjectsDBEntities db = new ProjectsDBEntities())
            {
                int count = Convert.ToInt32(db.Projects.SqlQuery("select count(Id) from Projects where IsActive=1 and IsDeleted <> 1 "));
                return count;
            }
        }

        public List<string> GetPmOnProjects()
        {
            using (ProjectsDBEntities db = new ProjectsDBEntities())
            {
                List<string> a = new List<string>();
                var author = db.Database.SqlQuery<int>("SELECT CreatedBy FROM Projects where IsActive = 1 GROUP by CreatedBy order by count(CreatedBy) desc").ToList();

                foreach (var blj in author)
                {
                    a.Add(GetCreator(blj));
                }

                return a;
            }
        }


        public List<Projects> GetPforPM(int id)
        {
            using (ProjectsDBEntities db = new ProjectsDBEntities())
            {
                List<Projects> dels = db.Projects.SqlQuery("Select * from Projects where CreatedBy = "+id).ToList();      ////////////dodati jos jednu listu i vracati ProjectModel listu
                var projs = db.Projects.ToList().Except(dels).ToList();

                return projs;
            }
        }


        public List<int> GetPmProjectCount()
        {
            using (ProjectsDBEntities db = new ProjectsDBEntities())
            {
                List<int> a = new List<int>();
                var count = db.Database.SqlQuery<int>("SELECT count(CreatedBy) FROM Projects where IsActive = 1 GROUP by CreatedBy order by count(CreatedBy) desc").ToList();

                foreach (var blj in count)
                {
                    
                    a.Add(Convert.ToInt32(blj));
                }

                return a;
            }
        }

        public bool ChangePassword(string old_pass, string new_pass, int id)
        {
            if (GetUserPassword(id).Equals(old_pass))
            {
                using (ProjectsDBEntities db = new ProjectsDBEntities())
                {
                    db.Users.Find(id).Password = new_pass;
                    db.SaveChanges();
                    return true;
                }
            }return false;
        }
        public string GetUserPassword(int id)
        {
            using (ProjectsDBEntities db = new ProjectsDBEntities())
            {
                var user = db.Users.Find(id);
               return user.Password;
                
            }
        }
    }
}