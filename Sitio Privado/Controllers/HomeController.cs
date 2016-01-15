using Sitio_Privado.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sitio_Privado.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetUsuarioActual()
        {
            var usuario = this.Usuario;

            return Json(new UsuarioDTO(usuario), JsonRequestBehavior.AllowGet);
        }

        public JsonResult TestWebService()
        {
            Agente agente = new Agente(new AgenteInput() { _rut = "8411855-9", _sec = 31 });

            return Json(agente, JsonRequestBehavior.AllowGet);
        }
    }
}