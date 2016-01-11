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
        public Usuario() { }

        public Usuario(ClaimsPrincipal principal) : base (principal) { }

        public bool Autenticado { get { return this.Identity.IsAuthenticated; } }

        public string Nombres { get { return this.Autenticado ? this.FindFirst(ClaimTypes.GivenName).Value : ""; } }

        public string Apellidos { get { return this.Autenticado ? this.FindFirst(ClaimTypes.Surname).Value : ""; } }

        public string NombreCompleto { get { return Nombres + " " + Apellidos; } }

    }

    /// <summary>
    /// Necesario para poder serializar los campos del usuario (evita referencias circulares de clase Usuario)
    /// </summary>
    public class UsuarioDTO
    {
        public bool Autenticado { get; set; }

        public string Nombres { get; set; }

        public string Apellidos { get; set; }

        public string NombreCompleto { get { return Nombres + " " + Apellidos; } }

        public UsuarioDTO(Usuario usuario)
        {
            Nombres = usuario.Nombres;
            Apellidos = usuario.Apellidos;
            Autenticado = usuario.Autenticado;
        }
    }
}