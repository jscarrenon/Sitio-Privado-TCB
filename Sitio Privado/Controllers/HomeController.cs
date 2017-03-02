using Microsoft.IdentityModel.Protocols;
using Newtonsoft.Json.Linq;
using NLog;
using Sitio_Privado.Filters;
using Sitio_Privado.Helpers;
using Sitio_Privado.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Sitio_Privado.Controllers
{
    public class HomeController : BaseController
    {

        private string asd = "";
        //[SkipTemporaryPassword]
        public ActionResult Index()
        {
            return View();
        }

        [SkipTemporaryPassword]
        public ActionResult GetUsuarioActual()
        {
            var usuario = GetUsuario();
            if (usuario != null && usuario.Autenticado)
            {
                //logger.Info("New connection => Rut: " + usuario.Rut + "; Email: " + 
                //    usuario.Email + "; IP: " + Request.ServerVariables["REMOTE_ADDR"] + ";");
            }
            return Json(new UsuarioDTO(usuario), JsonRequestBehavior.AllowGet);
        }
    }
}
