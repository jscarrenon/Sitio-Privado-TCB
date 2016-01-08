using System.Security.Claims;
using System.Security.Principal;

namespace Sitio_Privado.Models
{
    interface ICustomPrincipal : IPrincipal
    {
        bool Autenticado { get; }
        string Nombres { get; }
        string Apellidos { get; }
    }

    public class Usuario : ClaimsPrincipal, ICustomPrincipal
    {
        public Usuario(ClaimsPrincipal principal) : base (principal) { }

        public bool Autenticado { get { return this.Identity.IsAuthenticated; } }

        public string Nombres { get { return this.Autenticado ? this.FindFirst(ClaimTypes.GivenName).Value : ""; } }

        public string Apellidos { get { return this.Autenticado ? this.FindFirst(ClaimTypes.Surname).Value : ""; } }

        public string NombreCompleto { get { return Nombres + " " + Apellidos; } }

    }
}