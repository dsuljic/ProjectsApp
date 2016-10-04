using ProjectsApp.Models;
using System.Security.Claims;
using System.Web.Mvc;

namespace ProjectsApp.Controllers
{
    public abstract class AppController : Controller
    {
        public AppUser CurrentUser
        {
            get
            {
                return new AppUser(this.User as ClaimsPrincipal);
            }
        }
    }
}