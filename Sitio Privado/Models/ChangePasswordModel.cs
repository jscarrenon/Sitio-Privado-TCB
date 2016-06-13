using Sitio_Privado.Extras;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sitio_Privado.Models
{
    public class ChangePasswordModel
    {
        [Required]
        [DisplayName("Contraseña actual")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(16, ErrorMessage = "La contraseña debe tener entre {2} y {1} caracteres", MinimumLength = 6)]
        [RegularExpression(@"^([a-zA-Z0-9@#$%^&*\-_!+=[\]{}\|\\:´,.?\/`~""();]+)$", ErrorMessage = "La clave sólo puede contener mayúsculas, minúsculas, números y símbolos especiales")]
        [PasswordFormatValidation(ErrorMessage = "La clave debe contener mayúsculas, minúsculas, números y/o símbolos especiales")]
        [DisplayName("Nueva contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DisplayName("Nueva contraseña (repetir)")]
        [Compare("Password", ErrorMessage = "Las contraseñas ingresadas no coinciden")]
        [DataType(DataType.Password)]
        public string PasswordValidation { get; set; }
    }
}