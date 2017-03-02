using System.Web;
using System.Web.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Globalization;
using System.Configuration;
using Microsoft.AspNet.Identity;
using System.Net.Http;
using Sitio_Privado.Extras;
using Sitio_Privado.Helpers;
using Sitio_Privado.Filters;
using NLog;

namespace Sitio_Privado.Controllers
{
    public class AccountController : BaseController
    {
        SignInHelper signInHelper = new SignInHelper();
        private static readonly double passwordExpiresInHours = double.Parse(Startup.temporalPasswordTimeout, CultureInfo.InvariantCulture);
        private static readonly string baseURL = ConfigurationManager.AppSettings["web:BaseURL"];
        private static Logger logger = LogManager.GetLogger("SessionLog");
        private static readonly string contactPhoneNumber = ConfigurationManager.AppSettings["web:ContactPhoneNumber"];

        [SkipTemporaryPassword]
        public async Task<ActionResult> SignOut()
        {
            //Log
            var usuario =  GetUsuario();
            logger.Info("User signed out => Rut: " + ExtraHelpers.FormatRutToText(usuario.Rut) + "; Email: " +
                usuario.Email + "; IP: " + Request.ServerVariables["REMOTE_ADDR"] + ";");

            HttpContext.GetOwinContext().Authentication.SignOut();

            return Redirect(ConfigurationManager.AppSettings["web:PostLogoutRedirectUrl"]);
        }

        [SkipTemporaryPassword]
        [AllowAnonymous]
        [HttpGet]
        public ActionResult SignInExternal()
        {
            if (!Request.IsAuthenticated)
            {
                return Redirect(ConfigurationManager.AppSettings["web:PostLogoutRedirectUrl"]+"?action=login");
            }

            return RedirectToAction("Index", "Home");
        }

        [SkipTemporaryPassword]
        [AllowCrossSiteJson]
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> SignIn(string token)
        {
            IdToken idToken = new IdToken(token);

            var identity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie);
            await signInHelper.SetSignInClaims(identity, idToken);

            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;
            authManager.SignIn(identity);

            return RedirectToAction("Index", "Home");
        }
    }
}