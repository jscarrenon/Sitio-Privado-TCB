using Sitio_Privado.Extras;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sitio_Privado.Models
{
    public class LoginModel
    {
        [Required]
        [RegularExpression(@"^([0-9Kk.-]+)$", ErrorMessage = "El Rut contiene caracteres no válidos")]
        [RutValidation(ErrorMessage = "El Rut ingresado no es válido")]
        [DisplayName("RUT")]
        [StringLength(14, ErrorMessage = "Largo máximo de 14 caracteres")]
        public string Rut { get; set; }

        [Required]
        [StringLength(16, ErrorMessage = "La contraseña debe tener entre {2} y {1} caracteres", MinimumLength = 8)]
        [RegularExpression(@"^([a-zA-Z0-9@#$%^&*\-_!+=[\]{}\|\\:´,.?\/`~""();]+)$", ErrorMessage = "La clave sólo puede contener mayúsculas, minúsculas, números y símbolos especiales")]
        [PasswordFormatValidation(ErrorMessage = "La clave debe contener mayúsculas, minúsculas, números y/o símbolos especiales")]
        [DisplayName("Nueva contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}