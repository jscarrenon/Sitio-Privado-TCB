using Microsoft.IdentityModel.Protocols;
using Newtonsoft.Json.Linq;
using Sitio_Privado.Helpers;
using Sitio_Privado.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Sitio_Privado.Controllers
{
    public class HomeController : BaseController
    {
        GraphApiClientHelper graphApiHelper = new GraphApiClientHelper();

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> GetUsuarioActual()
        {
            var usuario = this.Usuario;
            await SetUserExtendedAttributes(usuario);
            return Json(new UsuarioDTO(usuario), JsonRequestBehavior.AllowGet);
        }

        private async Task SetUserExtendedAttributes(Usuario usuario)
        {
            IEnumerable<Claim> asd = ((ClaimsIdentity)usuario.Identity).Claims.Where(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier");
            Claim claim = asd.First();
            string id = claim.Value;
            HttpResponseMessage response = await graphApiHelper.GetUserByObjectId(id);
            JObject graphApiResponseContent = (JObject)await response.Content.ReadAsAsync(typeof(JObject));
            usuario.Banco = graphApiResponseContent.Value<string>(ConfigurationManager.AppSettings["b2c:Extensions"] + "Bank");
            usuario.CuentaCorriente = graphApiResponseContent.Value<string>(ConfigurationManager.AppSettings["b2c:Extensions"] + "CheckingAccount");
            usuario.Email = graphApiResponseContent.Value<string>(ConfigurationManager.AppSettings["b2c:Extensions"] + "Email");
            usuario.TelefonoComercial = graphApiResponseContent.Value<string>(ConfigurationManager.AppSettings["b2c:Extensions"] + "WorkPhoneNumber");
            usuario.TelefonoParticular = graphApiResponseContent.Value<string>(ConfigurationManager.AppSettings["b2c:Extensions"] + "HomePhoneNumber");
            usuario.DireccionComercial = graphApiResponseContent.Value<string>(ConfigurationManager.AppSettings["b2c:Extensions"] + "WorkAddress");
            usuario.DireccionParticular = graphApiResponseContent.Value<string>(ConfigurationManager.AppSettings["b2c:Extensions"] + "HomeAddress");
            usuario.Rut = graphApiResponseContent.Value<string>(ConfigurationManager.AppSettings["b2c:Extensions"] + "RUT");
        }
    }
}