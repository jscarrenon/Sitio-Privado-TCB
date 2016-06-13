using Postal;

namespace Sitio_Privado.Models
{
    public class PasswordRecoveryEmailModel : Email
    {
        public string From { get; set; }
        public GraphUserModel User { get; set; }
    }
}