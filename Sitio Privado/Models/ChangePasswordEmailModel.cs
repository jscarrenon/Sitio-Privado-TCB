using Postal;
using System;

namespace Sitio_Privado.Models
{
    public class ChangePasswordEmailModel : Email
    {
        public string From { get; set; }
        public GraphUserModel User { get; set; }
        public string IP { get; set; }
        public DateTime Timestamp { get; set; }
    }
}