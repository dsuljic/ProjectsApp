using ProjectsApp.Models.DB;
using System.Linq;
using System.Net;
using System.Net.Mail;

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

        public string GetUserPasswordFromEmail(string email)
        {
            using (ProjectsDBEntities db = new ProjectsDBEntities())
            {
                var user = db.Users.Where(o => o.Email.ToLower().Equals(email.ToLower()));
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

        public int GetCreatedByFromName(int id)
        {
            using (ProjectsDBEntities db = new ProjectsDBEntities())
            {
                var user = db.Projects.Where(o => o.Id == id );

                return user.FirstOrDefault().CreatedBy;

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

        public string GetUserRole (string username)
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
                um.Address = user.Address.ToString();
                um.PhoneNumber = user.PhoneNumber;
                return um;

            }
        }

        public int getRoleID(string n)
        {
            //List<Users> us =FetchUsers();
            //Users u = (Users)us.Where(x=>x.Id==n).FirstOrDefault();
            if (n == "Administrator")
                return 1;
            return 2;
        }
        
        public void DeleteUser(int id)
        {
            using (ProjectsDBEntities db = new ProjectsDBEntities())
            {
                db.Database.ExecuteSqlCommand("Update Users Set IsDeleted = 1 where id =" + id);
                db.SaveChanges();
            }
        }

        public int CheckEmail(string email)
        {
            using (ProjectsDBEntities db = new ProjectsDBEntities())
            {

                var user = db.Users.Where(o => o.Email.ToLower().Equals(email.ToLower()));
                if (user.Any())
                    return user.FirstOrDefault().Id;
                else
                    return 0;
            }
        }
        public bool SendEmailWithPass(string email)
        {
            if (CheckEmail(email) != 0)
            {
                using (ProjectsDBEntities db = new ProjectsDBEntities())
                {
                    SmtpClient client = new SmtpClient();
                    client.Port = 587;
                    client.Host = "smtp.gmail.com";
                      client.EnableSsl = true;
                    client.Credentials = new NetworkCredential("darko.bih@gmail.com", "powerslave");
                    MailMessage mailMessage = new MailMessage();
                    // mailMessage.From = "someone@somewhere.com";
                    mailMessage.From = new MailAddress("support@pms.com");
                    mailMessage.To.Add(email);
                    mailMessage.Subject = "Hello There";
                    mailMessage.Body = "Hello " + getUserById(CheckEmail(email)).Name + ", here is your password. Dont forget it again! \nYour password: " + GetUserPasswordFromEmail(email);
                    client.Send(mailMessage);
                    return true;
                }
            }
            return false;
        }

        public string GetEmail(int id)
        {
            using (ProjectsDBEntities db = new ProjectsDBEntities())
            {
                var user = db.Users.Find(id);
                return user.Email;
            }
        }
        public bool SendMessageToPM(int id, string text)
        {
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("darko.bih@gmail.com", "powerslave");
            MailMessage mailMessage = new MailMessage();
            // mailMessage.From = "someone@somewhere.com";
            mailMessage.From = new MailAddress("admin@pms.com");
            mailMessage.To.Add(GetEmail(id));
            mailMessage.Subject = "Hello There";
            mailMessage.Body = "Hello " + getUserById(CheckEmail(GetEmail(id))).Name + ", this is a message from your Administrator. \n" + text;
            client.Send(mailMessage);
            return true;
            
        }
    }


}