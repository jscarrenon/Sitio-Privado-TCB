using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sitio_Privado.Models
{
    /// <summary>
    /// Represents the info from a user from LDAP
    /// </summary>
    public class UserInfo
    {
        [Required]
        public string Rut { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string WorkAddress { get; set; }
        public string HomeAddress { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string WorkPhone { get; set; }
        public string HomePhone { get; set; }
        public string CheckingAccount { get; set; }
        public string Bank { get; set; }
       
    }
}