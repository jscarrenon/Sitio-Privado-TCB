using Sitio_Privado.Extras;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sitio_Privado.Models
{
    public class PasswordRecoveryModel
    {
        [Required]
        [RegularExpression(@"^([0-9Kk.-]+)$", ErrorMessage = "El Rut contiene caracteres no válidos")]
        [RutValidation(ErrorMessage = "El Rut ingresado no es válido")]
        [DisplayName("RUT")]
        [StringLength(14, ErrorMessage = "Largo máximo de 14 caracteres")]
        public string Rut { get; set; }
    }
}