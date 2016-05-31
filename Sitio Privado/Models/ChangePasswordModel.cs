using Sitio_Privado.Extras;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sitio_Privado.Models
{
    public class ChangePasswordModel
    {
        [Required]
        [DisplayName("Contraseña actual")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(16, ErrorMessage = "La contraseña debe tener entre {2} y {1} caracteres", MinimumLength = 8)]
        [RegularExpression(@"^([a-zA-Z0-9]+)$", ErrorMessage = "La clave sólo puede contener letras mayúsculas, minúsculas y números")]
        [PasswordFormatValidation(ErrorMessage = "La clave debe contener mayúsculas, minúsculas y números")]
        [DisplayName("Nueva contraseña")]
        public string Password { get; set; }

        [Required]
        [DisplayName("Nueva contraseña (repetir)")]
        [Compare("Password", ErrorMessage = "Las contraseñas ingresadas no coinciden")]
        public string PasswordValidation { get; set; }
    }
}