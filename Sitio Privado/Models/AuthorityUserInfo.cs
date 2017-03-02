using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitio_Privado.Models
{
    /// <summary>
    /// User information taken from the authority server
    /// </summary>
    public class AuthorityUserInfo
    {
        public string AuthorityId { get; set; }
        public string Rut { get; set; }
        public string GivenName { get; set; }
        public string LastName { get; set; }
        public string MothersLastName { get; set; }
    }
}