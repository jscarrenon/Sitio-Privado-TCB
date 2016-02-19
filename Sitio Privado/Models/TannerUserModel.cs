using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitio_Privado.Models
{
    public class TannerUserModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string FullName { get; set; }
        public string RutID { get; set; }
        public string RutVD { get; set; }
        public string Rut { get { return RutID + "-" + RutVD; } }
        public string WorkAddress { get; set; }
        public string HomeAddress { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string WorkPhone { get; set; }
        public string HomePhone { get; set; }
        public string Email { get; set; }
        public string CheckingAccount { get; set; }
        public string Bank { get; set; }
        public string TemporalPassword { get; set; }
        public string UpdatedAt { get; set; }
    }
}