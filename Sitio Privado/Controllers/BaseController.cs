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
        public virtual Usuario Usuario
        {
            get { return new Usuario(base.User as ClaimsPrincipal); }
        }
    }
}