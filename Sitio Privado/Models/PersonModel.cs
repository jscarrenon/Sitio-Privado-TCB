using System.Security.Claims;
using System.Security.Principal;

namespace Sitio_Privado.Models
{
    /// <summary>
    /// Necesario para poder serializar los campos del usuario (evita referencias circulares de clase Usuario)
    /// </summary>
    public class Person
    {
        public bool Autenticado { get; set; }

        public string Nombres { get; set; }

        public string Apellidos { get; set; }

        public string Rut { get; set; } 

        public string DireccionComercial { get; set; }

        public string DireccionParticular { get; set; }

        public string Ciudad { get; set; }

        public string Pais { get; set; }

        public string TelefonoComercial { get; set; }

        public string TelefonoParticular { get; set; }

        public string Email { get; set; }

        public string CuentaCorriente { get; set; }

        public string Banco { get; set; }

        public string NombreCompleto { get; set; }

        public string CiudadPais { get; set; }

        public bool ContrasenaTemporal { get; set; }

    }
}