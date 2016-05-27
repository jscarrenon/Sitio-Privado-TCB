using System.ComponentModel.DataAnnotations;

namespace Sitio_Privado.Models
{
    public class PasswordRecoveryModel
    {
        [Required]
        [DataType(DataType.Text)]
        public string Rut { get; set; }
    }
}