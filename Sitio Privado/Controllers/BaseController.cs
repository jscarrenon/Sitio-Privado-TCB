using Sitio_Privado.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace Sitio_Privado.Controllers
{
    public abstract class BaseController : Controller
    {
        public virtual new AppUser User
        {
            get { return new AppUser(base.User as ClaimsPrincipal); }
        }
    }
}