using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;
using Sitio_Privado.Models;
using System.Threading.Tasks;
using System.Globalization;
using System.Configuration;
using Microsoft.AspNet.Identity;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Text;
using HtmlAgilityPack;
using ScrapySharp.Extensions;
using Sitio_Privado.Extras;
using Newtonsoft.Json.Linq;
using Sitio_Privado.Helpers;
using System.Net.Configuration;
using Microsoft.Owin.Security;
using Sitio_Privado.Filters;
using System.Security.Principal;
using System.Runtime.Serialization.Json;
using NLog;

namespace Sitio_Privado.Controllers
{
    public class AccountController : BaseController
    {
        GraphApiClientHelper graphApiClient = new GraphApiClientHelper();
        SignInHelper signInHelper = new SignInHelper();
        private static readonly double passwordExpiresInHours = double.Parse(Startup.temporalPasswordTimeout, CultureInfo.InvariantCulture);
        private static readonly string baseURL = ConfigurationManager.AppSettings["web:BaseURL"];
        private static Logger logger = LogManager.GetLogger("SessionLog");
        private static readonly string contactPhoneNumber = ConfigurationManager.AppSettings["web:ContactPhoneNumber"];

        [SkipTemporaryPassword]
        public async Task<ActionResult> SignOut()
        {
            //Log
            var usuario = await GetUsuario();
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
                ViewBag.baseURL = baseURL;
                ViewBag.contactPhoneNumber = contactPhoneNumber;
                return View();
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