﻿using System.Security.Claims;
using System.Security.Principal;

namespace Sitio_Privado.Models
{
    /// <summary>
    /// Claims retornadas por AD B2C
    /// </summary>
    public static class CustomClaimTypes
    {
        public const string Nombres = ClaimTypes.GivenName;
        public const string Apellidos = ClaimTypes.Surname;
        public const string Rut = "rut";
        public const string DireccionComercial = "direccioncomercial";
        public const string DireccionParticular = ClaimTypes.StreetAddress;
        public const string Ciudad = "city";
        public const string Pais = "country";
        public const string TelefonoComercial = "telefonocomercial";
        public const string TelefonoParticular = ClaimTypes.HomePhone;
        public const string Email = ClaimTypes.Email;
        public const string CuentaCorriente = "cuentacorriente";
        public const string Banco = "banco";
    }

    interface ICustomPrincipal : IPrincipal
    {
        bool Autenticado { get; }
        string Nombres { get; }
        string Apellidos { get; }
        string Rut { get; }
        string DireccionComercial { get; }
        string DireccionParticular { get; }
        string Ciudad { get; }
        string Pais { get; }
        string TelefonoComercial { get; }
        string TelefonoParticular { get; }
        string Email { get; }
        string CuentaCorriente { get; }
        string Banco { get; }
        string NombreCompleto { get; }
        string CiudadPais { get; }
    }

    public class Usuario : ClaimsPrincipal, ICustomPrincipal
    {//Obtener el aud desde Identity, obtener el usuario y guardar los extension attributes
        public Usuario() { }

        public Usuario(ClaimsPrincipal principal) : base (principal) { }

        public bool Autenticado { get { return this.Identity.IsAuthenticated; } }

        public string Nombres { get { return this.Autenticado ? this.FindFirst(CustomClaimTypes.Nombres) != null ? this.FindFirst(CustomClaimTypes.Nombres).Value : "" : ""; } }

        public string Apellidos { get { return this.Autenticado ? this.FindFirst(CustomClaimTypes.Apellidos) != null ? this.FindFirst(CustomClaimTypes.Apellidos).Value : "" : ""; } }

        public string Rut { get; set; }

        public string DireccionComercial { get; set; }

        public string DireccionParticular { get; set; }

        public string Ciudad { get { return this.Autenticado ? this.FindFirst(CustomClaimTypes.Ciudad) != null ? this.FindFirst(CustomClaimTypes.Ciudad).Value : "" : ""; } }

        public string Pais { get { return this.Autenticado ? this.FindFirst(CustomClaimTypes.Pais) != null ? this.FindFirst(CustomClaimTypes.Pais).Value : "" : ""; } }

        public string TelefonoComercial { get; set; }

        public string TelefonoParticular { get; set; }

        public string Email { get; set; }

        public string CuentaCorriente { get; set; }

        public string Banco { get; set; }

        public string NombreCompleto { get { return Nombres + " " + Apellidos; } }

        public string CiudadPais { get { return (!string.IsNullOrEmpty(Ciudad) || !string.IsNullOrEmpty(Pais)) ? Ciudad + ", " + Pais : ""; } }

    }

    /// <summary>
    /// Necesario para poder serializar los campos del usuario (evita referencias circulares de clase Usuario)
    /// </summary>
    public class UsuarioDTO
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

        public UsuarioDTO(Usuario usuario)
        {
            Autenticado = usuario.Autenticado;
            Nombres = usuario.Nombres;
            Apellidos = usuario.Apellidos;
            Rut = usuario.Rut;
            DireccionComercial = usuario.DireccionComercial;
            DireccionParticular = usuario.DireccionParticular;
            Ciudad = usuario.Ciudad;
            Pais = usuario.Pais;
            TelefonoComercial = usuario.TelefonoComercial;
            TelefonoParticular = usuario.TelefonoParticular;
            Email = usuario.Email;
            CuentaCorriente = usuario.CuentaCorriente;
            Banco = usuario.Banco;
            NombreCompleto = usuario.NombreCompleto;
            CiudadPais = usuario.CiudadPais;
        }
    }
}