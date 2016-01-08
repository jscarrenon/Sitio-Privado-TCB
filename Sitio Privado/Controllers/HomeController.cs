using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sitio_Privado.Controllers
{
    public class HomeController : BaseController
    {
        [Policies.PolicyAuthorize(Policy = "B2C_1_SignIn")]
        public ActionResult Index()
        {
            if(User != null)
            {
                string nombre = User.GiveName;
            }

            return View();
        }
    }
}