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

            return Redirect("https://www.tanner.cl");
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

        [SkipTemporaryPassword]
        [HttpGet]
        public ActionResult ChangePassword()
        {
            IPrincipal user = this.User; 

            Claim claim = ((ClaimsIdentity)user.Identity).Claims.Where(c => c.Type == Startup.isTemporalPasswordClaimKey).First();
            bool isTemporalPassword = bool.Parse(claim.Value);

            claim = ((ClaimsIdentity)user.Identity).Claims.Where(c => c.Type == Startup.temporalPasswordTimestampClaimKey).First();
            DateTime temporalPasswordTimestamp = DateTime.Parse(claim.Value);

            if (isTemporalPassword)
            {
                DateTime limit = temporalPasswordTimestamp.AddHours(passwordExpiresInHours);

                if (DateTime.Now > limit)
                {
                    ViewBag.Message = "Su contraseña temporal ha caducado. Por favor solicite una nueva.";
                }
            }

            return View();
        }

        [SkipTemporaryPassword]
        [HttpPost]
        public async Task<ActionResult> ChangePassword(ChangePasswordModel model)
        {
            IPrincipal user = this.User;

            if (ModelState.IsValid)
            {
                //Retrieve user info
                Claim idClaim = ((ClaimsIdentity)user.Identity).Claims.Where(c => c.Type == Startup.objectIdClaimKey).First();
                GraphApiResponseInfo getUserResponse = await graphApiClient.GetUserByObjectId(idClaim.Value);
                if (getUserResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    getUserResponse.User.TemporalPassword = model.Password;
                    getUserResponse.User.IsTemporalPassword = false;
                    getUserResponse.User.TemporalPasswordTimestamp = DateTime.MinValue.ToString();

                    var apiResponse = await graphApiClient.ResetUserPassword(getUserResponse.User.ObjectId, getUserResponse.User);

                    if (apiResponse.StatusCode != System.Net.HttpStatusCode.NoContent)
                    {
                        ModelState.AddModelError("", "Error al intentar cambiar la contraseña. Intente otra vez.");
                        return View(model);
                    }
                    else
                    {
                        Claim isTemporalPasswordClaim = ((ClaimsIdentity)user.Identity).Claims.Where(c => c.Type == Startup.isTemporalPasswordClaimKey).First();
                        Claim temporalPasswordTimestampClaim = ((ClaimsIdentity)user.Identity).Claims.Where(c => c.Type == Startup.temporalPasswordTimestampClaimKey).First();

                        //Update claims
                        ((ClaimsIdentity)user.Identity).RemoveClaim(isTemporalPasswordClaim);
                        ((ClaimsIdentity)user.Identity).AddClaim(new Claim(Startup.isTemporalPasswordClaimKey, bool.FalseString));

                        ((ClaimsIdentity)user.Identity).RemoveClaim(temporalPasswordTimestampClaim);
                        ((ClaimsIdentity)user.Identity).AddClaim(new Claim(Startup.temporalPasswordTimestampClaimKey, DateTime.MinValue.ToString()));

                        var ctx = Request.GetOwinContext();
                        var authManager = ctx.Authentication;
                        authManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(new ClaimsPrincipal(user.Identity), new AuthenticationProperties() { IsPersistent = true });

                        //Sign out B2C TODO

                        //Display success message: "Su contraseña ha sido modificadda con éxito" TODO

                        //Send mail
                        try
                        {
                            SendMail(getUserResponse.User);
                        }
                        catch(Exception e)
                        {
                            //TODO
                        }

                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    throw new NullReferenceException("No se encontró información asociada al usuario.");
                }
            }

            return View(model);
        }

        private void SendMail(GraphUserModel user)
        {
            SmtpSection settings = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
            var email = new ChangePasswordEmailModel
            {
                From = settings.From,
                User = user
            };
            email.Send();
        }
    }
}