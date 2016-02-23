using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Postal;

namespace Sitio_Privado.Extras
{
    public class Email : Postal.Email
    {
        public Email(string template) : base(template) { }

        public string UserPassword { get; set; }
        public string UserFullName { get; set; }
        public string UserEmail { get; set; }
    }
}