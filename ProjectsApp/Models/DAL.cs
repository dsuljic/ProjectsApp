using ProjectsApp.Models.DB;
using System.Linq;

namespace ProjectsApp.Models
{
    public class DAL
    {
        public string GetUserPassword(string username)
        {
            using (ProjectsDBEntities db = new ProjectsDBEntities())
            {
                var user = db.Users.Where(o => o.Username.ToLower().Equals(username.ToLower()));
                if (user.Any())
                    return user.FirstOrDefault().Password;
                else
                    return string.Empty;
            }
        }

       

        public int GetUserId(string username)
        {
            using (ProjectsDBEntities db = new ProjectsDBEntities())
            {
                var user = db.Users.Where(o => o.Username.ToLower().Equals(username.ToLower()));
             
                    return user.FirstOrDefault().Id;
                
            }

        }

       

        public string GetUserScreenName(string username)
        {
            using (ProjectsDBEntities db = new ProjectsDBEntities())
            {

                var user = db.Users.Where(o => o.Username.ToLower().Equals(username.ToLower()));

                return user.FirstOrDefault().Name;
            }
        }

        public string GetUserRole ( string username)
        {
            using (ProjectsDBEntities db = new ProjectsDBEntities())
            {
                var user = db.Users.Where(o => o.Username.ToLower().Equals(username.ToLower()));
                byte role = user.FirstOrDefault().Role_Id;
                if (role == 1)
                    return "Administrator";
                return "ProjectManager";
            }
        }

        public UserModel getUserById(int id)
        {
            using (ProjectsDBEntities db = new ProjectsDBEntities())
            {
                var user = db.Users.Find(id);

                UserModel um = new UserModel();
                um.Id = user.Id;
                um.Name = user.Name;
                um.E_mail = user.Email;
                um.Address = user.Address;
                um.PhoneNumber = user.PhoneNumber;
                return um;

            }
        }
        

    }


}