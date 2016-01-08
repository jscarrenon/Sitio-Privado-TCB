using System.Security.Claims;
using System.Security.Principal;

namespace Sitio_Privado.Models
{
    interface ICustomPrincipal : IPrincipal
    {
        bool IsAuthenticated { get; }
        string GiveName { get; }
    }

    public class AppUser : ClaimsPrincipal, ICustomPrincipal
    {
        public AppUser(ClaimsPrincipal principal) : base (principal)
        {

        }

        public bool IsAuthenticated
        {
            get
            {
                return this.Identity.IsAuthenticated;
            }
        }

        public string GiveName
        {
            get
            {
                return this.IsAuthenticated ? this.FindFirst(ClaimTypes.GivenName).Value : "";
            }
        }
    }
}