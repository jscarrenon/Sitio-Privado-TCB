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
            InformacionClienteAgente.tann_info_cliente infocliente = new InformacionClienteAgente.tann_info_cliente();
            InformacionClienteAgente._agente agente = infocliente.cli_info_agente("8411855-9", 31);

            return Json(agente, JsonRequestBehavior.AllowGet);
        }
    }
}