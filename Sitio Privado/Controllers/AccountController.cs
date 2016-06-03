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

namespace Sitio_Privado.Controllers
{
    public class AccountController : BaseController
    {
        GraphApiClientHelper graphApiClient = new GraphApiClientHelper();
        SignInHelper signInHelper = new SignInHelper();
        private static readonly double passwordExpiresInHours = double.Parse(Startup.temporalPasswordTimeout, CultureInfo.InvariantCulture);

        [SkipTemporaryPassword]
        public ActionResult SignOut()
        {
            HttpContext.GetOwinContext().Authentication.SignOut();

            return Redirect(ConfigurationManager.AppSettings["web:PostLogoutRedirectUrl"]);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult SignInExternal()
        {
            if (!Request.IsAuthenticated)
            {
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        [AllowCrossSiteJson]
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> SignIn(IdToken token)
        {
            var identity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie);
            await signInHelper.SetSignInClaims(identity, token);

            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;
            authManager.SignIn(identity);

            return RedirectToAction("Index", "Home");
        }
    }
}