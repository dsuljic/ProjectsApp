using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectsApp.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string E_mail {get;set;}
        public string Username { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

    }
}