using System.Security.Claims;

namespace ProjectsApp.Models
{
    public class AppUser : ClaimsPrincipal
    {
        public AppUser(ClaimsPrincipal principal)
            : base(principal)
        {
        }

        public string Name
        {
            get
            {
                return this.FindFirst(ClaimTypes.Name).Value;
            }
        }

        public string Sid
        {
            get
            {
                return this.FindFirst(ClaimTypes.Sid).Value;
            }
        }

    }
}